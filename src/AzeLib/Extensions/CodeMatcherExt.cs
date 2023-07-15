using HarmonyLib;
using System.Collections.Generic;

namespace AzeLib.Extensions
{
    public static class CodeMatcherExt
    {
        /// <summary>Inserts some instructions after the current instruction</summary>
        /// <param name="instructions">The instructions</param>
        /// <returns>The same code matcher</returns>
        public static CodeMatcher InsertAfter(this CodeMatcher cm, params CodeInstruction[] instructions) => 
            cm.Advance(1).Insert(instructions);

        /// <summary>Inserts an enumeration of instructions after the current instruction</summary>
        /// <param name="instructions">The instructions</param>
        /// <returns>The same code matcher</returns>
        public static CodeMatcher InsertAfter(this CodeMatcher cm, IEnumerable<CodeInstruction> instructions) =>
            cm.Advance(1).Insert(instructions);
    }
}
