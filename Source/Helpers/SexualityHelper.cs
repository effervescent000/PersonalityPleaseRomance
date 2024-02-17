
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Personality.Romance;

public static class SexualityHelper
{
    // to be replaced with stuff from settings
    private const float asexualityChanceBase = 0.05f;
    private const float homoChanceBase = 0.225f;
    private const float biChanceBase = 0.50f;
    private const float heteroChanceBase = 0.225f;

    private static SexualityValues straightValues = new(RomanceTraitDefOf.Straight, heteroChanceBase);
    private static SexualityValues biValues = new(TraitDefOf.Bisexual, biChanceBase);
    private static SexualityValues gayValues = new(TraitDefOf.Gay, homoChanceBase);

    private static SexualityValues aceValues = new(TraitDefOf.Asexual, asexualityChanceBase);
    private static SexualityValues aceBiValues = new(RomanceTraitDefOf.AceBi, biChanceBase);
    private static SexualityValues aceHeteroValues = new(RomanceTraitDefOf.AceHetero, heteroChanceBase);
    private static SexualityValues aceHomoValues = new(RomanceTraitDefOf.AceHomo, homoChanceBase);
    private static SexualityValues aroAceValues = new(RomanceTraitDefOf.AroAce, asexualityChanceBase);

    private static List<string> asexualTraitDefNames = new()
        {   aceBiValues.TraitDef.defName,
            aceHeteroValues.TraitDef.defName,
            aceHomoValues.TraitDef.defName,
            aroAceValues.TraitDef.defName
        };

    private static List<string> biTraitDefNames = new() { TraitDefOf.Bisexual.defName, RomanceTraitDefOf.AceBi.defName };
    private static List<string> heteroTraitDefNames = new() { RomanceTraitDefOf.Straight.defName, RomanceTraitDefOf.AceHetero.defName };
    private static List<string> homoTraitDefNames = new() { TraitDefOf.Gay.defName, RomanceTraitDefOf.AceHomo.defName };

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
            if (found.TraitDef.defName == TraitDefOf.Asexual.defName)
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
            if (found.TraitDef.defName == TraitDefOf.Asexual.defName)
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
            if (found.TraitDef.defName == TraitDefOf.Asexual.defName)
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
        if (found.TraitDef.defName == TraitDefOf.Asexual.defName)
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

    public static bool IsAsexual(this Pawn pawn)
    {
        if (pawn.story != null && pawn.story.traits != null)
        {
            foreach (Trait trait in pawn.story.traits.allTraits)
            {
                if (asexualTraitDefNames.Contains(trait.def.defName))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsBisexual(this Pawn pawn)
    {
        if (pawn.story != null && pawn.story.traits != null)
        {
            foreach (Trait trait in pawn.story.traits.allTraits)
            {
                if (biTraitDefNames.Contains(trait.def.defName))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsGay(this Pawn pawn)
    {
        if (pawn.story != null && pawn.story.traits != null)
        {
            foreach (Trait trait in pawn.story.traits.allTraits)
            {
                if (homoTraitDefNames.Contains(trait.def.defName))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsStraight(this Pawn pawn)
    {
        if (pawn.story != null && pawn.story.traits != null)
        {
            foreach (Trait trait in pawn.story.traits.allTraits)
            {
                if (heteroTraitDefNames.Contains(trait.def.defName))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool DoesOrientationMatch (Pawn actor, Pawn target, bool asexualityBlocks = false)
    {
        if (asexualityBlocks && actor.IsAsexual()) { return false; }

        if (actor.IsStraight() && actor.gender == target.gender) { return false; }

        if (actor.IsGay()  && actor.gender != target.gender) { return false; }

        // if none of the ifs fail, then it's a match
        return true;
    }
}
