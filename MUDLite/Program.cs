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
            Globals.Server.ClientConnected += ClientConnected;
            Globals.Server.ClientDisconnected += ClientDisconnected;
            Globals.Server.ConnectionBlocked += ConnectionBlocked;
            Globals.Server.MessageReceived += MessageReceived;
            Globals.Server.Start();

            Console.WriteLine($"SERVER STARTED at {DateTime.Now} on port {Globals.Server.Port}");

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

        #region Events
        private static void ClientConnected(Client client)
        {
            Console.WriteLine("CONNECTED: " + client);
            Globals.Server.SendMessage(client, Constants.Title, Constants.END_LINE);
            UserManagement.SendWelcomeMessage(client);
        }

        private static void ClientDisconnected(Client client)
        {
            Console.WriteLine("DISCONNECTED: " + client);
        }

        private static void ConnectionBlocked(IPEndPoint ep)
        {
            Console.WriteLine(string.Format("BLOCKED: {0}:{1} at {2}", ep.Address, ep.Port, DateTime.Now));
        }

        private static void MessageReceived(Client client, string message)
        {
            if (!client.IsLoggedIn)
            {
                UserManagement.HandleLogin(client, message);
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
            {
                Globals.Server.SendMessage(client, Constants.END_LINE + Constants.CURSOR);
            }
        }
        #endregion
    }
}
