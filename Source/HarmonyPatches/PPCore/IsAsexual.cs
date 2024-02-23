using HarmonyLib;
using Personality.Core;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance.HarmonyPatches;

[HarmonyPatch(typeof(PawnExtensions), nameof(PawnExtensions.IsAsexual))]
public class IsAsexual
{
    [HarmonyPostfix]
    public static bool PatchIsAsexual(bool _, Pawn pawn)
    {
        if (pawn.story != null && pawn.story.traits != null)
        {
            foreach (Trait trait in pawn.story.traits.allTraits)
            {
                if (SexualityHelper.asexualTraitDefNames.Contains(trait.def.defName))
                {
                    return true;
                }
            }
        }
        return false;
    }
}