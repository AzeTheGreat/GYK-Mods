using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace FootprintPerformance
{
    [HarmonyPatch(typeof(LeaveTrailComponent), nameof(LeaveTrailComponent.LeaveTrail))]
    class CapTrails
    {
        public static int MaxTrails => 100;

        static void Postfix()
        {
            if (MaxTrails == -1)
                return;

            var allTrails = LeaveTrailComponent._all_trails;
            if (allTrails.Count > MaxTrails)
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
