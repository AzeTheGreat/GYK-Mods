using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace FootprintPerformance
{
    [HarmonyPatch(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent.LeaveTrail))]
    class CapTrails
    {
        static void Postfix()
        {
            if (AzePlugin.MaxTrails.Value == -1)
                return;

            var allTrails = LeaveTrailComponent._all_trails;
            if (allTrails.Count > AzePlugin.MaxTrails.Value)
            {
                var oldestTrail = allTrails.First();
                LeaveTrailComponent.OnTrailObjectDestroyed(oldestTrail);
                oldestTrail.gameObject.AddComponent<DestroyTrailWhenInvisible>();
            }
        }

        class DestroyTrailWhenInvisible : MonoBehaviour
        {
            void Start()
            {
                if (!GetComponent<SpriteRenderer>().isVisible)
                    Destroy(gameObject);
            }

            void OnBecameInvisible() => Destroy(gameObject);
        }
    }
}
