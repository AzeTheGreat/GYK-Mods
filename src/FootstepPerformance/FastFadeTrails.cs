using HarmonyLib;
using System;
using System.Collections.Generic;

namespace FootprintPerformance
{
    [HarmonyPatch(typeof(TrailObject), nameof(TrailObject.Update))]
    static class FastFadeTrails
    {
        private static float DegSpeedOutside => AzePlugin.DegSpeedOutside.Value;
        private static float DegSpeedInside => AzePlugin.DegSpeedInside.Value;
        private static float DegSpeedOutsideRain => AzePlugin.DegSpeedOutsideRain.Value;

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
