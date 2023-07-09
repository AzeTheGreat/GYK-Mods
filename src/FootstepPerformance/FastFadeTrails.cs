using HarmonyLib;
using System;
using System.Collections.Generic;

namespace PerformanceFixes
{
    [HarmonyPatch(typeof(TrailObject), nameof(TrailObject.Update))]
    static class FastFadeTrails
    {
        public static float DegSpeedOutside => 1f;
        public static float DegSpeedInside => 25f;
        public static float DegSpeedOutsideRain => 100f;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            return codes
                .ReplaceConstant(TrailObject.DEGRADE_SPEED_OUTSIDE, () => DegSpeedOutside)
                .ReplaceConstant(TrailObject.DEGRADE_SPEED_INSIDE, () => DegSpeedInside)
                .ReplaceConstant(TrailObject.DEGRADE_SPEED_OUTSIDE_RAIN, () => DegSpeedOutsideRain);
        }

        private static IEnumerable<CodeInstruction> ReplaceConstant(this IEnumerable<CodeInstruction> codes, float oldVal, Func<float> func)
        {
            var call = Transpilers.EmitDelegate(func);
            return new CodeMatcher(codes)
                .SearchForward(i => i.LoadsConstant(oldVal))
                .Set(call.opcode, call.operand)
                .InstructionEnumeration();
        }
    }
}
