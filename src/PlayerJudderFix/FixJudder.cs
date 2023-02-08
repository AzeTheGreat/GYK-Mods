using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Rendering;

namespace PlayerJudderFix
{
    [HarmonyPatch(typeof(PlayerComponent), nameof(PlayerComponent.InitLocalPlayer))]
    class EnableInterpolation
    {
        static void Postfix(PlayerComponent __instance)
        {
            __instance.wgo.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    [HarmonyPatch(typeof(RoundAndSortComponent), nameof(RoundAndSortComponent.DoUpdateStuff))]
    class FixPlayerMovement
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var position_setter = AccessTools.PropertySetter(typeof(Transform), nameof(Transform.position));

            return codes.Manipulator(
                i => i.Calls(position_setter),
                i => new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    CodeInstruction.Call(typeof(FixPlayerMovement), nameof(Wrapper))
                });
        }

        private static void Wrapper(Transform transform, Vector3 position, RoundAndSortComponent roundSort)
        {
            if (roundSort._world_part?.parent?.is_player ?? false)
            {
                var tf = transform.GetComponentInChildren<SortingGroup>().transform;
                tf.position = new(tf.position.x, tf.position.y, position.z);
            }
            else
                transform.position = position;
        }
    }
}
