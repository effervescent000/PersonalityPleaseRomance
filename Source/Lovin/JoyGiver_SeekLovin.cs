using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;
using Verse;
using Verse.AI;

namespace Personality.Romance;

public class JoyGiver_SeekLovin : JoyGiver
{
    public override Job TryGiveJob(Pawn pawn)
    {
        if (pawn.IsAsexual())
        {
            return null;
        }
        if (pawn.ageTracker.AgeBiologicalYearsFloat < 16f)
        {
            return null;
        }

        // before we look for a partner, decide what kind of lovin' we're looking for. if we roll
        // above a .25, we're looking for a partner. if we're below that, the pawn just does self lovin'
        if (Rand.Value <= .25f)
        {
            return TrySelfLovin(pawn);
        }
        else
        {
            return TryDoHookup(pawn);
        }
    }

    public Job TryDoHookup(Pawn pawn)
    {
        Pawn partner = RomanceHelper.FindPartner(pawn);
        // if we can't actually find a partner, then the pawn either gives up and does something
        // else or does self lovin'
        if (partner == null)
        {
            if (Rand.Value <= .75f)
            {
                return null;
            }
            else
            {
                return TrySelfLovin(pawn);
            }
        }

        Building_Bed bed = RomanceHelper.FindBed(pawn, partner);
        if (bed == null)
        {
            return null;
        }

        return JobMaker.MakeJob(RomanceJobDefOf.LeadHookup, partner, bed);
    }

    public Job TrySelfLovin(Pawn pawn)
    {
        Building_Bed bed = RomanceHelper.FindBed(pawn);

        return JobMaker.MakeJob(RomanceJobDefOf.DoSelfLovin, bed, bed.GetSleepingSlotPos(0));
    }
}