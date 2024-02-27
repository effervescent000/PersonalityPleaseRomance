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

    public static Pawn FindPartnerForIntimacy(Pawn actor)
    {
        List<DirectPawnRelation> relations = actor.relations.DirectRelations;
        List<Pawn> potentialPartners = new();
        foreach (DirectPawnRelation rel in relations)
        {
            Pawn target = rel.otherPawn;
            if (!target.Spawned || target.Map.uniqueID != actor.Map.uniqueID) continue;
            if (!romanticRelationDefs.Contains(rel.def.defName)) continue;
            if (!target.IsOk()) continue;
            // TODO rejection list check here

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

            return sorted[0].First;
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

        // make a new list of potential partners iterate over `availablePawns` and add pawns that
        // pass basic checks to new list return a random pawn from new list

        List<Pawn> potentialPartners = new();

        foreach (Pawn pawn in availablePawns)
        {
            if (pawn.ThingID == actor.ThingID || !pawn.IsOk()) continue;
            if (!CoreLovinHelper.DoesOrientationMatch(actor, pawn, true)) continue;
            if (!CoreGeneralHelper.IsTargetInRange(actor, pawn)) continue;

            // TODO if pawn is in the Actor's reject list, ignore (unless conditions? maybe highly
            // chaotic and/or uncompassionate?)

            if (actor.IsBloodRelatedTo(pawn)) continue;

            potentialPartners.Add(pawn);
        }
        if (potentialPartners.Count > 0)
        {
            //return potentialPartners.RandomElement();

            List<Pair<Pawn, AttractionEvaluation>> partnersByAttraction = new();
            RomanceComp comp = actor.GetComp<RomanceComp>();
            foreach (Pawn partner in potentialPartners)
            {
                partnersByAttraction.Add(new(partner, comp.AttractionTracker.GetEvalFor(partner)));
            }
            List<Pair<Pawn, AttractionEvaluation>> sorted = partnersByAttraction.OrderByDescending(pair => pair.Second.PhysicalScore).ToList();
            Log.Message($"returning partner {sorted[0].First.LabelShort} with an attraction of {sorted[0].Second.PhysicalScore}");

            // TODO instead of just choosing the first one, choose weighted random

            return sorted[0].First;
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