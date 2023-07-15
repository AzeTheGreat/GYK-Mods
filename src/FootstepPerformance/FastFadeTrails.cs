using HarmonyLib;
using System;
using System.Collections.Generic;

namespace FootprintPerformance
{
    [HarmonyPatch(typeof(TrailObject), nameof(TrailObject.Update))]
    static class FastFadeTrails
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            return codes
                .ReplaceConstant(TrailObject.DEGRADE_SPEED_OUTSIDE, () => AzePlugin.DegSpeedOutside.Value)
                .ReplaceConstant(TrailObject.DEGRADE_SPEED_INSIDE, () => AzePlugin.DegSpeedInside.Value)
                .ReplaceConstant(TrailObject.DEGRADE_SPEED_OUTSIDE_RAIN, () => AzePlugin.DegSpeedOutsideRain.Value);
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
