using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MattPruett.MUDLite.Data.DataModel;

namespace MattPruett.MUDLite.Data
{
    internal static class DataHelpers
    {
        internal static bool CharacterNameExists(string name, int? besidesThisKey = null)
        {
            using(var db = new MUDLiteDataContext())
            {
                var chars = (from creature in db.Creatures
                             where creature.Name == name
                                && (besidesThisKey == null || creature.Key != besidesThisKey)
                             select true).Any();
                return chars;
            }
        }
    }
}
