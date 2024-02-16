#nullable enable

using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Personality.Romance;

public static class SexualityHelpers
{
    // to be replaced with stuff from settings
    private const float asexualityChanceBase = 0.05f;
    private const float homoChanceBase = 0.225f;
    private const float biChanceBase = 0.50f;
    private const float heteroChanceBase = 0.225f;

    private const string ASEXUAL = "Asexual";

    private static SexualityValues straightValues = new(RomanceTraitDefOf.Straight, heteroChanceBase);
    private static SexualityValues biValues = new(TraitDefOf.Bisexual, biChanceBase);
    private static SexualityValues gayValues = new(TraitDefOf.Gay, homoChanceBase);

    private static SexualityValues aceValues = new(TraitDefOf.Asexual, asexualityChanceBase);
    private static SexualityValues aceBiValues = new(RomanceTraitDefOf.AceBi, biChanceBase);
    private static SexualityValues aceHeteroValues = new(RomanceTraitDefOf.AceHetero, heteroChanceBase);
    private static SexualityValues aceHomoValues = new(RomanceTraitDefOf.AceHomo, homoChanceBase);
    private static SexualityValues aroAceValues = new(RomanceTraitDefOf.AroAce, asexualityChanceBase);


    public static void RollSexualityTraitFor(Pawn pawn)
    {
        bool maybeGay = false;
        bool maybeStraight = false;

        List<SexualityValues> rollingForSexuality;

        if (LovePartnerRelationUtility.HasAnyLovePartnerOfTheSameGender(pawn) || LovePartnerRelationUtility.HasAnyExLovePartnerOfTheSameGender(pawn))
        {
            maybeGay = true;
        }
        if (LovePartnerRelationUtility.HasAnyLovePartnerOfTheOppositeGender(pawn) || LovePartnerRelationUtility.HasAnyExLovePartnerOfTheOppositeGender(pawn))
        {
            maybeStraight = true;
        }

        SexualityValues found;

        if (maybeGay && maybeStraight)
        {
            rollingForSexuality = new()
            {
                biValues, aceValues
            };
            found = FindOrientation(rollingForSexuality);
            if (found.TraitDef.defName == ASEXUAL)
            {
                pawn.story.traits.GainTrait(new Trait(RomanceTraitDefOf.AceBi));
                return;
            }
            pawn.story.traits.GainTrait(new Trait(TraitDefOf.Bisexual));
            return;
        }
        if (maybeGay)
        {
            rollingForSexuality = new() { biValues, gayValues, aceValues };
            found = FindOrientation(rollingForSexuality);
            if (found.TraitDef.defName == ASEXUAL)
            {
                List<SexualityValues> rollingForAce = new() { aceBiValues, aceHomoValues };
                pawn.story.traits.GainTrait(new Trait(FindOrientation(rollingForAce).TraitDef));
                return;
            }
            pawn.story.traits.GainTrait(new Trait(found.TraitDef));
            return;
        }
        if (maybeStraight)
        {
            rollingForSexuality = new() { biValues, straightValues, aceValues };
            found = FindOrientation(rollingForSexuality);
            if (found.TraitDef.defName == ASEXUAL)
            {
                List<SexualityValues> rollingForAce = new() { aceBiValues, aceHeteroValues };
                pawn.story.traits.GainTrait(new Trait(FindOrientation(rollingForAce).TraitDef));
                return;
            }
            pawn.story.traits.GainTrait(new Trait(found.TraitDef));
            return;
        }

        // now, roll general sexuality (no restrictions based on relationships)
        rollingForSexuality = new() { biValues, straightValues, gayValues, aceValues };
        found = FindOrientation(rollingForSexuality);
        if (found.TraitDef.defName == ASEXUAL)
        {
            List<SexualityValues> rollingForAce = new() { aceBiValues, aceHeteroValues, aceHomoValues, aroAceValues };
            pawn.story.traits.GainTrait(new Trait(FindOrientation(rollingForAce).TraitDef));
            return;
        }
        pawn.story.traits.GainTrait(new Trait(found.TraitDef));
    }

    private static SexualityValues FindOrientation(List<SexualityValues> values)
    {
        float sumValue = 0;
        values.ForEach(value => sumValue += value.chance);

        float orientationCheckValue = 0f;
        if (sumValue < 1f)
        {
            values.ForEach(value => orientationCheckValue += value.chance / sumValue);
        }
        else
        {
            orientationCheckValue = 1f;
        }

        float orientationValue = Rand.Value;

        foreach (SexualityValues value in values)
        {
            sumValue -= value.chance;
            if (sumValue <= orientationValue)
            {
                return value;

            }

        }
        throw new Exception("No sexuality match found, somehow");
    }
}
