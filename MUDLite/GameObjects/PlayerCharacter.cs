﻿using MattPruett.MUDLite.Data.DataModel.Models;
using MattPruett.MUDLite.GameObjects.Base;
using MattPruett.MUDLite.GameObjects.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattPruett.MUDLite.GameObjects
{
    internal class PlayerCharacter : Creature
    {

        public PlayerCharacter(Tbl_Creature creature) : base(creature)
        {
            LoadRoom();
        }

        private Room _room;

        public Room Room { get; set; }

        private void LoadRoom()
        {
            if (RM_Key != null || _room.Key != RM_Key)
            {
                _room = Room.GetRoom(RM_Key ?? 0);
            }
        }
    }
}
