using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FootprintPerformance
{
    [HarmonyPatch(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent.LeaveTrail))]
    static class OverrideTrailUpdateThresh
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var unmoddedThresh = 0.1f;

            return new CodeMatcher(codes)
                .SearchForward(i => i.LoadsConstant(unmoddedThresh))
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
                        return 0.5f;
                }
                return unmoddedThresh;
            }
        }
    }
}