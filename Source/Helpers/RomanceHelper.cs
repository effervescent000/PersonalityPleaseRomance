using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Personality.Romance;

public static class RomanceHelper
{

    public static Pawn FindPartner(Pawn actor)
    {
        Map homeMap = actor.Map;
        List<Pawn> availablePawns = homeMap.mapPawns.FreeAdultColonistsSpawned;

        // make a new list of potential partners
        // iterate over `availablePawns` and add pawns that pass basic checks to new list
        // return a random pawn from new list

        List<Pawn> potentialPartners = new();

        foreach (Pawn pawn in availablePawns)
        {
            if (pawn.ThingID == actor.ThingID || !pawn.IsOk()) { continue; }

            // pawns will never initiate casual lovin' with someone who does not match their orientation
            if (!SexualityHelper.DoesOrientationMatch(actor, pawn, true)) { continue; }

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

}
