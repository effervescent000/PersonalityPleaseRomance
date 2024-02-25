using System.Collections.Generic;
using Verse;

namespace Personality.Romance.HarmonyPatches;

public static class PatchGetAttractionFactor
{
    public static Dictionary<string, float> Postfix(Dictionary<string, float> _, Pawn pawn, Pawn target)
    {
        RomanceComp comp = pawn.GetComp<RomanceComp>();
        AttractionEvaluation eval = comp.AttractionTracker.GetEvalFor(target);
        return new() { { "physical", eval.PhysicalScore }, { "personality", eval.PersonalityScore } };
    }
}