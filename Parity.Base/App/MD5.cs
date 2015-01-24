using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Parity.Base.App
{
    public class MD5
    {

        /// <summary>
        /// Gibt einen MD5 Hash als String zurück
        /// </summary>
        /// <param name="TextToHash">string der Gehasht werden soll.</param>
        /// <returns>Hash als string.</returns>
        public static string Hash(string TextToHash)
        {
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(TextToHash))).Replace("-", "").ToLower();
        }
        public static byte[] HashBytes(string TextToHash)
        {
            return new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(TextToHash));
        }

    }
}
