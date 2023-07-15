using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FootprintPerformance
{
    public static class Extensions
    {
        public static bool HasAnyTrailSprites(this TrailTypeDefinition def)
        {
            var allSprites = new List<List<Sprite>>() { def.hor_b, def.hor_t, def.vert_l, def.vert_r, def.diag_lt, def.diag_rb };
            return allSprites.Any(x => x.Any());
        }
    }
}
