using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Personality.Romance;

public static class RomanceHelper
{
    private const float MINIMUM_ACCEPTANCE_VALUE = 0.5f;

    public static Pawn FindPartner(Pawn actor)
    {
        Map homeMap = actor.Map;
        List<Pawn> availablePawns =
            (
                from pawn in homeMap.mapPawns.AllPawnsSpawned
                where pawn.def.defName == "Human" && pawn.ageTracker.AgeBiologicalYears >= 16f
                select pawn
             ).ToList();

        // make a new list of potential partners iterate over `availablePawns` and add pawns that
        // pass basic checks to new list return a random pawn from new list

        List<Pawn> potentialPartners = new();

        foreach (Pawn pawn in availablePawns)
        {
            if (pawn.ThingID == actor.ThingID || !pawn.IsOk()) { continue; }

            // pawns will never initiate casual lovin' with someone who does not match their orientation
            if (!SexualityHelper.DoesOrientationMatch(actor, pawn, true)) { continue; }

            // if pawn is too far away, ignore
            if (!IsTargetInRange(actor, pawn)) { continue; }

            // TODO if pawn is in the Actor's reject list, ignore (unless conditions? maybe highly
            // chaotic and/or uncompassionate?)

            // no hooking up with blood relations
            if (actor.IsBloodRelatedTo(pawn)) { continue; }

            potentialPartners.Add(pawn);
        }
        if (potentialPartners.Count > 0)
        {
            return potentialPartners.RandomElement();
        }
        return null;
    }

    public static Building_Bed FindBed(Pawn actor, Pawn partner)
    {
        // find literally any bed
        List<Building_Bed> beds = actor.Map.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>().ToList();
        if (beds.Count > 0)
        {
            return beds[0];
        }
        return null;
    }

    public static bool IsTargetInRange(Pawn actor, Pawn target)
    {
        return actor.Position.InHorDistOf(target.Position, RomanceMod.settings.MaxInteractionDistance.Value);
    }

    public static bool DoesTargetAcceptHookup(Pawn actor, Pawn target)
    {
        float baseValue = Rand.Value;
        float adjustedValue = baseValue;

        // TODO add in relationship checks (existing lovers are much more likely to accept, etc)

        // TODO add precept checks: unmarried pawns in non-free-lovin ideos are unlikely to accept,
        // depending on strength of precept

        // target is much less likely to accept if they have an orientation mismatch
        if (!SexualityHelper.DoesOrientationMatch(actor, target, true))
        {
            adjustedValue *= .1f;
        }

        if (adjustedValue < MINIMUM_ACCEPTANCE_VALUE)
        {
            return false;
        }
        return true;
    }
}