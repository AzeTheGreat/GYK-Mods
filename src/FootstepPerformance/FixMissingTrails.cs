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

    [HarmonyPatch(typeof(LeaveTrailComponent), (nameof(LeaveTrailComponent.LeaveTrail)))]
    class FixTrailSwitchSkip
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var getByType_method = AccessTools.Method(typeof(TrailDefinition), nameof(TrailDefinition.GetByType));
            var trailType_field = AccessTools.Field(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent._trail_type));

            // Find the byType local store.
            var storeByTypeLoc_code = new CodeMatcher(codes)
                .SearchForward(i => i.Calls(getByType_method))
                .SearchForward(i => i.IsStloc())
                .Instruction;

            return new CodeMatcher(codes)
                // Update the byType local when the trailType is updated if the dirtyAmount is too low.
                .SearchForward(i => i.StoresField(trailType_field))
                .InsertAfter(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    Transpilers.EmitDelegate(GetUpdatedTrailDef),
                    storeByTypeLoc_code.Clone())
                .SearchForward(i => i.opcode == OpCodes.Ret)
                // Remove the return from this block since the updated trailType allows leaving a trail.
                .Set(OpCodes.Nop, null)
                .InstructionEnumeration();

            TrailTypeDefinition GetUpdatedTrailDef(LeaveTrailComponent cmp) => cmp._trail_definition.GetByType(cmp._trail_type);
        }
    }
}
