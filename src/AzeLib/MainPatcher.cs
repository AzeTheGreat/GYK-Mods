using HarmonyLib;
using System.Reflection;

namespace AzeLib
{
    public class MainPatcher
    {
        public static void Patch(string guid)
        {
            var harmony = new Harmony(guid);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
