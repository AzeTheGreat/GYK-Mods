using HarmonyLib;
using UnityEngine;

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
}
