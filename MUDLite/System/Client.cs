using MattPruett.MUDLite.Data.DataModel.Models;
using MattPruett.MUDLite.Libraries;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace MattPruett.MUDLite.System
{
    internal enum ClientStatus
    {
        Guest = 0,
        CreatingUser,
        CreatingUserName,
        CreatingUserPassword,
        ConfirmingUserPassword,
        EnteringUserName,
        EnteringUserPassword,
        Authenticating,
        CreatingCharacterName,
        ConfirmingCharacter,
        SelectingCharacter,
        LoggedIn,
    }

    internal class Client
    {
        private uint _id;
        private IPEndPoint _remoteAddr;
        private DateTime _connectedAt;
        private ClientStatus _status;
        private string _receivedData;

        public Hashtable State { get; set; }
        public User User { get; set; }
        public Creature Character
        {
            get
            {
                return User?.Character;
            }
        }

        public Client(uint clientId, IPEndPoint remoteAddress)
        {
            _id = clientId;
            _remoteAddr = remoteAddress;
            _connectedAt = DateTime.Now;
            _status = ClientStatus.Guest;
            _receivedData = string.Empty;
            State = new Hashtable();
        }

        #region Properties
        public uint ClientID
        {
            get { return _id; }
        }


        public DateTime ConnectionTime
        {
            get { return _connectedAt; }
        }

        public bool IsAuthenticating
        {
            get 
            { 
                return 
                    _status == ClientStatus.CreatingUserPassword ||
                    _status == ClientStatus.ConfirmingUserPassword ||
                    _status == ClientStatus.EnteringUserPassword;
            }
        }

        public bool IsLoggedIn
        {
            get { return _status == ClientStatus.LoggedIn; }
        }
        public IPEndPoint RemoteAddress
        {
            get { return _remoteAddr; }
        }

        public Socket Socket
        {
            get
            {
                return Globals.Server.GetSocketByClient(this);
            }
        }

        public ClientStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string ReceivedData
        {
            get { return _receivedData; }
            set { _receivedData = value; }
        }
        #endregion

        public void AppendReceivedData(string dataToAppend)
        {
            _receivedData += dataToAppend;
        }

        public void ClearScreen()
        {
            Send(Terminal.ClearScreen);
        }

        public void RemoveLastCharacterReceived()
        {
            _receivedData = _receivedData.Substring(0, _receivedData.Length - 1);
        }

        public void ResetReceivedData()
        {
            _receivedData = string.Empty;
        }

        public void Send(params string[] messages)
        {
            Globals.Server.SendMessage(this, messages);
        }

        public void SendLine(string message)
        {
            Globals.Server.SendMessage(this, message + Constants.END_LINE);
        }

        public override string ToString()
        {
            return $"Client #{_id} (From: {_remoteAddr.Address}:{_remoteAddr.Port}, Status: {_status}, Connection time: {_connectedAt})";
        }
    }

    internal static class StateKeys
    {
        internal const string UserName = "UserName";
        internal const string UserPassword = "UserPassword";
    }
}