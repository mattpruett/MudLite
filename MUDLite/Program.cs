using MattPruett.MUDLite;
using MattPruett.MUDLite.Libraries;
using MattPruett.MUDLite.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MUDLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Globals.Server = new Server(IPAddress.Any);
            Globals.Server.ClientConnected += clientConnected;
            Globals.Server.ClientDisconnected += clientDisconnected;
            Globals.Server.ConnectionBlocked += connectionBlocked;
            Globals.Server.MessageReceived += messageReceived;
            Globals.Server.Start();

            Console.WriteLine("SERVER STARTED: " + DateTime.Now);

            char read = Console.ReadKey(true).KeyChar;

            do
            {
                if (read == 'b')
                {
                    Globals.Server.BroadcastMessage(Console.ReadLine());
                }
            }
            while ((read = Console.ReadKey(true).KeyChar) != 'q');

            Globals.Server.Stop();
        }

        private static void clientConnected(Client client)
        {
            Console.WriteLine("CONNECTED: " + client);
            Globals.Server.SendMessage(client, Constants.Title);
            Globals.Server.SendMessage(client, Constants.END_LINE + "Login: ");
        }

        private static void clientDisconnected(Client client)
        {
            Console.WriteLine("DISCONNECTED: " + client);
        }

        private static void connectionBlocked(IPEndPoint ep)
        {
            Console.WriteLine(string.Format("BLOCKED: {0}:{1} at {2}", ep.Address, ep.Port, DateTime.Now));
        }

        private static void messageReceived(Client client, string message)
        {
            if (!client.IsLoggedIn)
            {
                handleLogin(client, message);
                return;
            }

            Console.WriteLine("MESSAGE: " + message);

            if (message == "kickmyass" || message == "logout" ||
                message == "exit")
            {
                Globals.Server.KickClient(client);
                Globals.Server.SendMessage(client, Constants.END_LINE + Constants.CURSOR);
            }

            else if (message == "clear")
            {
                Globals.Server.ClearClientScreen(client);
                Globals.Server.SendMessage(client, Constants.CURSOR);
            }

            else
                Globals.Server.SendMessage(client, Constants.END_LINE + Constants.CURSOR);
        }

        private static void handleLogin(Client client, string message)
        {
            switch (client.Status)
            {
                case ClientStatus.Guest:
                    if (message == "root")
                    {
                        Globals.Server.SendMessage(client, Constants.END_LINE + "Password: ");
                        client.Status = ClientStatus.Authenticating;
                    }

                    else
                        Globals.Server.KickClient(client);
                    break;
                case ClientStatus.Authenticating:
                    if (message == "r00t")
                    {
                        Globals.Server.ClearClientScreen(client);
                        Globals.Server.SendMessage(client, "Successfully authenticated." + Constants.END_LINE + Constants.CURSOR);
                        client.Status = ClientStatus.LoggedIn;
                    }

                    else
                        Globals.Server.KickClient(client);
                    break;
            }
        }
    }
}
