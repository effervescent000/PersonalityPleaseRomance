using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace Personality.Romance;

public class JoyGiver_CasualLovin : JoyGiver
{

    public override Job TryGiveJob(Pawn pawn)
    {
        if (pawn.IsAsexual())
        {
            return null;
        }
        if (pawn.ageTracker.AgeBiologicalYearsFloat < 18f)
        {
            return null;
        }

        Log.Message($"Looking for partner for {pawn.Name}");

        Pawn partner = RomanceHelper.FindPartner(pawn);
        if (partner.ThingID == pawn.ThingID)
        {
            return null;
        }
        Log.Message($"Found pair for hookup: {pawn.Name} & {partner.Name}");

        Building_Bed bed = RomanceHelper.FindBed(pawn, partner);
        if (bed == null)
        {
            return null;
        }
        Log.Message("Found bed, initiating hookup");

        return JobMaker.MakeJob(def.jobDef, partner, bed);
    }

}
