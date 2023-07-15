using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace FootprintPerformance
{
    [HarmonyPatch(typeof(TrailObject), nameof(TrailObject.Update))]
    class FixTrailAlpha
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var getColor_getter = AccessTools.PropertyGetter(typeof(SpriteRenderer), nameof(SpriteRenderer.color));
            var setAlpha_method = AccessTools.Method(typeof(ExtentionTools), nameof(ExtentionTools.SetAlpha));

            return new CodeMatcher(codes)
                // Remove the getColor call, leaving just the SpriteRenderer on the stack.
                .SearchForward(i => i.Calls(getColor_getter))
                .RemoveInstruction()
                // Replace the setAlpha call.
                .SearchForward(i => i.Calls(setAlpha_method))
                .SetInstruction(Transpilers.EmitDelegate(SetAlphaFixed))
                .InstructionEnumeration();

            // The game's implementation of SetAlpha just gets the color property and sets the alpha, which means it is never applied to the sprite.
            static void SetAlphaFixed(SpriteRenderer spr, float realAlpha)
            {
                var color = spr.color;
                color.a = realAlpha;
                spr.color = color;
            }
        }
    }
}
