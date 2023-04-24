using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MattPruett.MUDLite.Libraries
{
    internal class StringHasher
    {
        internal string Hash(string value)
        {
            // Create a SHA256   
            using (var hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                // Convert byte array to a string   
                var sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // two uppercase hexadecimal characters
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
