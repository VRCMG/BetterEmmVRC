using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace BetterEmmVRC.Helpers_And_Extensions
{
    internal class Helpers
    {
        internal static void Log(string text)
        {
            MelonLogger.Log(ConsoleColor.Magenta, text);
        }
    }
}
