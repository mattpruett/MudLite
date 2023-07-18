using MattPruett.MUDLite.Libraries;
using MattPruett.MUDLite.System;

namespace MattPruett.MUDLite.GameObjects.Commands
{
    internal class Look : ICommand
    {
        public Look()
        {
            Name = "look";
            Role = Role.Player;
        }

        public string Name { get; set; }
        public Role Role { get; set; }

        public void Execute(Client client, object _)
        {
            client.User.ShowRoom();
        }
    }
}