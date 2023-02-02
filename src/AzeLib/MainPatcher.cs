using HarmonyLib;
using System.Reflection;

namespace AzeLib
{
    public class MainPatcher
    {
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var harmony = new Harmony("Aze." + assembly.FullName);
            harmony.PatchAll(assembly);
        }
    }
}
