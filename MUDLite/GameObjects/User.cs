using MattPruett.MUDLite.Data.DataModel.Models;
using System.Collections.Generic;
using System.Linq;
using MattPruett.MUDLite.System;
using MattPruett.MUDLite.GameObjects.World;

namespace MattPruett.MUDLite.GameObjects
{
    internal class User
    {
        internal User(Tbl_User user)
        {
            Id = user.Id;
            Name = user.Name;
            _user = user;
            Characters = _user.Characters.Select(character => new PlayerCharacter(character)).ToList();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        private Tbl_User _user { get; set; }
        private Client _client;

        public bool HasCharacters
        {
            get
            {
                return (Characters?.Count ?? 0) > 0;
            }
        }

        public void SetClient(Client client)
        {
            _client = client;
        }

        public PlayerCharacter Character { get; set; }

        public List<PlayerCharacter> Characters { get; set; }

        public PlayerCharacter CharacterByName(string name)
        {
            return (
                from chars in Characters
                where chars.Name == name
                select chars
            ).FirstOrDefault();
        }

        public void CreateCharacter(string name)
        {
            _user.CreateCharacter(name);
        }

        public void ShowRoom()
        {
            ShowRoom(Character.Room);
        }

        public void ShowRoom(Room room)
        {
            if (room != null)
            {
                _client.Send(room.ToString());
            }
        }
    }
}
