using MattPruett.MUDLite.Libraries;
using MattPruett.MUDLite.System;

namespace MattPruett.MUDLite.GameObjects
{
    internal interface ICommand
    {
        void Execute(Client client, object commandState);

        string Name { get; set; }

        Role Role { get; set; }
    }
}