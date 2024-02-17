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
        var availablePawns = homeMap.mapPawns;
        return availablePawns.AllPawnsSpawned[0];

    }

    public static Building_Bed FindBed(Pawn actor, Pawn partner)
    {
        // find literally any bed
        List<Building_Bed> beds = actor.Map.listerBuildings.AllBuildingsColonistOfClass<Building_Bed>().ToList();
        if (beds.Count > 0 )
        {
            return beds[0];
        }
        return null;
    }

}
