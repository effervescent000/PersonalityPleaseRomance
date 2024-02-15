using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance.HarmonyPatches;

[HarmonyPatch(typeof(PawnGenerator), nameof(PawnGenerator.GenerateTraitsFor))]
public class PawnGenerator_Patches
{
    
    private static readonly List<string> vanillaSexualityTraits = new() { "Gay", "Bisexual" };

    [HarmonyPostfix]
    public static void CleanAndApplySexualityTraits(ref Pawn pawn)
    {
        bool sexualityTraitFound = false;
        foreach (var trait in pawn.story.traits.allTraits)
        {
            if (trait.def.defName == "Gay")
            {
                sexualityTraitFound = true;
                pawn.story.traits.RemoveTrait(trait);
            }
            else if (trait.def.defName == "Bisexual")
            {
                sexualityTraitFound = true;
                pawn.story.traits.RemoveTrait(trait);
            }
        }
        // if a sexuality trait is found, reroll a different trait
        if (sexualityTraitFound)
        {
            Trait newTrait = null;
            while (newTrait is null)
            {
                List<Trait> rolledTrait = PawnGenerator.GenerateTraitsFor(pawn, 1);
                if (vanillaSexualityTraits.Contains(rolledTrait[0].def.defName))
                {
                    newTrait = rolledTrait[0];
                }
            }
            pawn.story.traits.GainTrait(newTrait);
        }
        // now assign a sexuality trait

    }
}
