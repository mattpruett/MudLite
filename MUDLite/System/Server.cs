using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using MattPruett.MUDLite.Libraries;

using static MattPruett.MUDLite.Libraries.Terminal;

namespace MattPruett.MUDLite.System
{
    internal class Server
    {

        private const int PORT = 23;
        private Socket _serverSocket;
        private IPAddress _ip;
        private readonly int _dataSize;
        private byte[] _data;
        private bool _acceptIncomingConnections;
        private Dictionary<Socket, Client> _clients;

        public delegate void ConnectionEventHandler(Client client);
        public event ConnectionEventHandler ClientConnected;
        public event ConnectionEventHandler ClientDisconnected;
        public delegate void ConnectionBlockedEventHandler(IPEndPoint endPoint);
        public event ConnectionBlockedEventHandler ConnectionBlocked;
        public delegate void MessageReceivedEventHandler(Client client, string message);
        public event MessageReceivedEventHandler MessageReceived;

        public Server(IPAddress ip, int dataSize = 1024)
        {
            _ip = ip;
            _dataSize = dataSize;
            _data = new byte[dataSize];
            _clients = new Dictionary<Socket, Client>();
            _acceptIncomingConnections = true;
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public bool IncomingConnectionsAllowed
        {
            get { return _acceptIncomingConnections; }
        }

        public int Port
        {
            get { return PORT; }
        }

        public void DenyIncomingConnections()
        {
            _acceptIncomingConnections = false;
        }

        public void AllowIncomingConnections()
        {
            _acceptIncomingConnections = true;
        }

        public void BroadcastMessage(string message)
        {
            foreach (Socket socket in _clients.Keys)
            {
                try
                {
                    var client = _clients[socket];

                    if (client.IsLoggedIn)
                    {
                        SendMessage(socket, Constants.END_LINE + message + Constants.END_LINE + Constants.CURSOR);
                        client.ResetReceivedData();
                    }
                }

                catch
                {
                    _clients.Remove(socket);
                }
            }
        }

        public void ClearClientScreen(Client client)
        {
            client.ClearScreen();
        }

        private void CloseSocket(Socket clientSocket)
        {
            clientSocket.Close();
            _clients.Remove(clientSocket);
        }

        private Client GetClientBySocket(Socket clientSocket)
        {
            return _clients.TryGetValue(clientSocket, out Client client)
                ? client
                : null;
        }

        public Socket GetSocketByClient(Client client)
        {
            return _clients.FirstOrDefault(x => x.Value.ClientID == client.ClientID).Key;
        }

        private void HandleIncomingConnection(IAsyncResult result)
        {
            try
            {
                var oldSocket = (Socket)result.AsyncState;

                if (_acceptIncomingConnections)
                {
                    Socket newSocket = oldSocket.EndAccept(result);

                    uint clientID = (uint)_clients.Count + 1;
                    var client = new Client(clientID, (IPEndPoint)newSocket.RemoteEndPoint);
                    _clients.Add(newSocket, client);

                    SendBytes(
                        newSocket,
                        NewClientMessage
                    );

                    client.ResetReceivedData();

                    ClientConnected(client);

                    _serverSocket.BeginAccept(new AsyncCallback(HandleIncomingConnection), _serverSocket);
                }
                else
                {
                    ConnectionBlocked((IPEndPoint)oldSocket.RemoteEndPoint);
                }
            }

            catch { }
        }

        public void KickClient(Client client)
        {
            CloseSocket(GetSocketByClient(client));
            ClientDisconnected(client);
        }

        private void ReceiveData(IAsyncResult result)
        {
            try
            {
                var clientSocket = (Socket)result.AsyncState;
                var client = GetClientBySocket(clientSocket);

                int bytesReceived = clientSocket.EndReceive(result);

                if (bytesReceived == 0)
                {
                    CloseSocket(clientSocket);
                    _serverSocket.BeginAccept(new AsyncCallback(HandleIncomingConnection), _serverSocket);
                }

                else if (_data[0] < ByteConst.ú)
                {
                    string receivedData = client.ReceivedData;

                    if ((_data[0] == ByteConst.Period && _data[1] == ByteConst.CR && receivedData.Length == 0) ||
                        (_data[0] == ByteConst.CR && _data[1] == ByteConst.LF))
                    {
                        MessageReceived(client, client.ReceivedData);
                        client.ResetReceivedData();
                    }
                    else
                    {
                        if (_data[0] == ByteConst.BackSpace)
                        {
                            if (receivedData.Length > 0)
                            {
                                client.RemoveLastCharacterReceived();
                                SendBytes(clientSocket, new byte[] { ByteConst.BackSpace, ByteConst.Space, ByteConst.BackSpace });
                            }
                            else
                            {
                                clientSocket.BeginReceive(_data, 0, _dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                            }
                        }
                        else if (_data[0] == ByteConst.Delete)
                        {
                            clientSocket.BeginReceive(_data, 0, _dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                        }
                        else
                        {
                            client.AppendReceivedData(Encoding.ASCII.GetString(_data, 0, bytesReceived));

                            // Echo back the received character
                            // if client is not writing any password
                            if (!client.IsAuthenticating)
                            {
                                SendBytes(clientSocket, new byte[] { _data[0] });
                            }
                            // Echo back asterisks if client is
                            // writing a password
                            else
                            {
                                SendMessage(clientSocket, "*");
                            }

                            clientSocket.BeginReceive(_data, 0, _dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                        }
                    }
                }

                else
                {
                    clientSocket.BeginReceive(_data, 0, _dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
                }
            }
            catch
            {
                // Om nom nom nom.
            }
        }

        public void Remove(Socket socket)
        {
            if (_clients.ContainsKey(socket))
            {
                _clients.Remove(socket);
            }
        }

        public void SendBytes(Socket socket, byte[] data)
        {
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendData), socket);
        }

        private void SendData(IAsyncResult result)
        {
            try
            {
                var clientSocket = (Socket)result.AsyncState;

                clientSocket.EndSend(result);

                clientSocket.BeginReceive(_data, 0, _dataSize, SocketFlags.None, new AsyncCallback(ReceiveData), clientSocket);
            }

            catch { }
        }

        public void SendLine(Client client, params string[] messages)
        {
            if (messages.Length > 0)
            {
                messages[messages.Length-1] += Constants.END_LINE;
                var clientSocket = GetSocketByClient(client);
                SendMessage(clientSocket, messages);
            }
        }

        public void SendMessage(Client client, params string[] messages)
        {
            var clientSocket = GetSocketByClient(client);
            SendMessage(clientSocket, messages);
        }

        private void SendMessage(Socket socket, params string[] messages)
        {
            byte[] data;
            foreach (var message in messages)
            {
                data = Encoding.ASCII.GetBytes(message);
                SendBytes(socket, data);
            }
        }

        public void Start()
        {
            _serverSocket.Bind(new IPEndPoint(_ip, PORT));
            _serverSocket.Listen(0);
            _serverSocket.BeginAccept(new AsyncCallback(HandleIncomingConnection), _serverSocket);
        }

        public void Stop()
        {
            _serverSocket.Close();
        }
    }
}
