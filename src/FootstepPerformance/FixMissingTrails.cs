using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FootprintPerformance
{
    [HarmonyPatch(typeof(LeaveTrailComponent), MethodType.Constructor, new[] { typeof(BaseCharacterComponent), typeof(string) })]
    class FixSpritelessTrail
    {
        static void Postfix(LeaveTrailComponent __instance)
        {
            foreach (var def in __instance._trail_definition.trails)
            {
                // If a trail type does not have any sprites, it should immediately "run out" so that the feet can be dirtied with a valid trail type.
                if (!def.HasAnyTrailSprites())
                {
                    def.custom_trail_decrease = true;
                    def.trail_decrease = 0f;
                }
            }
        }
    }
}
