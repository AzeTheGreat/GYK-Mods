using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FootprintPerformance
{
    [HarmonyPatch(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent.LeaveTrail))]
    static class OverrideTrailUpdateThresh
    {
        public const float UNMODDED_THRESH = 0.1f;

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            return new CodeMatcher(codes)
                .SearchForward(i => i.LoadsConstant(UNMODDED_THRESH))
                .Set(OpCodes.Ldarg_0, null)
                .InsertAfter(Transpilers.EmitDelegate(GetNewThreshold))
                .InstructionEnumeration();

            float GetNewThreshold(LeaveTrailComponent cmp)
            {
                if (cmp._trail_type != cmp._ground_under)
                {
                    // Only override the threshold if the new ground type has its own trails.
                    var groundDef = cmp._trail_definition.GetByType(cmp._ground_under);
                    if (groundDef.HasAnyTrailSprites())
                        return AzePlugin.TrailUpdateThreshold.Value;
                }
                return UNMODDED_THRESH;
            }
        }
    }
}