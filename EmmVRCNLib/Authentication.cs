using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterEmmVRC.EmmVRCNLib
{
    internal class Authentication
    {
        internal static bool Exists(string userID)
        {
            return File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + Authentication.extension));
        }

        internal static string ReadTokenFile(string userID)
        {
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + Authentication.extension)))
            {
                return "";
            }

            return File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + Authentication.extension));
        }

        internal static void DeleteTokenFile(string userID)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + Authentication.extension)))
            {
                File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + Authentication.extension));
            }
        }

        internal static void CreateTokenFile(string userID, string data)
        {
            //Emm, You Don't Need This. WriteAllBytes Also Overwrites.
            //if (File.Exists(Path.Combine(Authentication.path, path)))
            //{
            //    File.Delete(Path.Combine(Authentication.path, path));
            //}

            File.WriteAllBytes(Path.Combine(Authentication.path, userID + Authentication.extension), Encoding.UTF8.GetBytes(data));
        }

        private static string path = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/");

        private static string extension = ".ema";
    }
}
