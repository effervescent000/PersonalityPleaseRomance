using Personality.Core;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Personality.Romance;

public static class RomanceHelper
{
    private const float MINIMUM_HOOKUP_ACCEPTANCE_VALUE = 0.5f;

    private static readonly List<string> romanticRelationDefs = new() { PawnRelationDefOf.Lover.defName, PawnRelationDefOf.Fiance.defName, PawnRelationDefOf.Spouse.defName };

    private static readonly SimpleCurve chanceToIgnoreRejectionByLawfulness = new()
    {
        new CurvePoint(-1f, 0.5f),
        new CurvePoint(1f, -0.25f)
    };

    private static readonly SimpleCurve chanceToIgnoreRejectionByCompassion = new()
    {
        new CurvePoint(-1f, 0.5f),
        new CurvePoint(1f, -1f)
    };

    public static Pawn FindPartnerForIntimacy(Pawn actor)
    {
        List<DirectPawnRelation> relations = actor.relations.DirectRelations;
        List<Pawn> potentialPartners = new();

        if (relations.Count == 0) return null;

        float actorLawfulness = CorePersonalityHelper.GetPersonalityNodeRating(CorePersonalityHelper.LAWFULNESS, actor);
        float actorCompassion = CorePersonalityHelper.GetPersonalityNodeRating(CorePersonalityHelper.COMPASSION, actor);

        foreach (DirectPawnRelation rel in relations)
        {
            Pawn target = rel.otherPawn;
            if (!target.Spawned || target.Map.uniqueID != actor.Map.uniqueID) continue;
            if (!romanticRelationDefs.Contains(rel.def.defName)) continue;

            RomanceComp comp = actor.GetComp<RomanceComp>();
            if (comp.RomanceTracker.IsInRejectionList(target))
            {
                if (Rand.Value >= chanceToIgnoreRejectionByCompassion.Evaluate(actorCompassion) + chanceToIgnoreRejectionByLawfulness.Evaluate(actorLawfulness)) continue;
            }

            if (!target.IsOk()) continue;

            potentialPartners.Add(target);
        }

        if (potentialPartners.Count > 0)
        {
            if (potentialPartners.Count == 1) { return potentialPartners[0]; }

            List<Pair<Pawn, int>> partnersByAttraction = new();
            foreach (Pawn pawn in potentialPartners)
            {
                partnersByAttraction.Add(new(pawn, actor.relations.OpinionOf(pawn)));
            }
            List<Pair<Pawn, int>> sorted = partnersByAttraction.OrderByDescending(pair => pair.Second).ToList();

            // TODO instead of just choosing the first one, choose weighted random

            //return sorted[0].First;
            return sorted.RandomElementByWeight(pair => pair.Second).First;
        }

        return null;
    }

    public static Pawn FindPartnerForHookup(Pawn actor)
    {
        List<Pawn> availablePawns =
            (
                from pawn in actor.Map.mapPawns.AllPawnsSpawned
                where pawn.def.defName == "Human" && pawn.ageTracker.AgeBiologicalYears >= 16
                select pawn
             ).ToList();

        List<Pawn> potentialPartners = new();

        if (availablePawns.Count == 0) return null;

        float actorLawfulness = CorePersonalityHelper.GetPersonalityNodeRating(CorePersonalityHelper.LAWFULNESS, actor);
        float actorCompassion = CorePersonalityHelper.GetPersonalityNodeRating(CorePersonalityHelper.COMPASSION, actor);

        foreach (Pawn pawn in availablePawns)
        {
            if (pawn.ThingID == actor.ThingID || !pawn.IsOk()) continue;
            if (!CoreLovinHelper.DoesOrientationMatch(actor, pawn, true)) continue;
            if (!CoreGeneralHelper.IsTargetInRange(actor, pawn)) continue;

            RomanceComp comp = actor.GetComp<RomanceComp>();
            if (comp.RomanceTracker.IsInRejectionList(pawn))
            {
                if (Rand.Value >= chanceToIgnoreRejectionByCompassion.Evaluate(actorCompassion) + chanceToIgnoreRejectionByLawfulness.Evaluate(actorLawfulness)) continue;
            }

            if (actor.IsBloodRelatedTo(pawn)) continue;

            potentialPartners.Add(pawn);
        }
        if (potentialPartners.Count > 0)
        {
            List<Pair<Pawn, AttractionEvaluation>> partnersByAttraction = new();
            RomanceComp comp = actor.GetComp<RomanceComp>();
            foreach (Pawn partner in potentialPartners)
            {
                partnersByAttraction.Add(new(partner, comp.AttractionTracker.GetEvalFor(partner)));
            }
            List<Pair<Pawn, AttractionEvaluation>> sorted = partnersByAttraction.OrderByDescending(pair => pair.Second.PhysicalScore).ToList();
            Log.Message($"returning partner {sorted[0].First.LabelShort} with an attraction of {sorted[0].Second.PhysicalScore}");

            // TODO make personality a non-zero factor in hookups, altho i'm not sure how important
            // to make it

            return sorted.RandomElementByWeight(pair => pair.Second.PhysicalScore).First;
        }
        return null;
    }

    public static bool DoesTargetAcceptHookup(Pawn actor, Pawn target)
    {
        float rolledValue = Rand.Value;

        // TODO add in relationship checks (existing lovers are much more likely to accept, etc)

        // TODO add precept checks: unmarried pawns in non-free-lovin ideos are unlikely to accept,
        // depending on strength of precept

        // target is much less likely to accept if they have an orientation mismatch
        if (!CoreLovinHelper.DoesOrientationMatch(actor, target, true))
        {
            rolledValue *= .1f;
        }

        if (rolledValue < MINIMUM_HOOKUP_ACCEPTANCE_VALUE)
        {
            return false;
        }
        return true;
    }
}