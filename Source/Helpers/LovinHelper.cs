using Personality.Core;
using RimWorld;
using Verse;
using Verse.AI;

namespace Personality.Romance;

public static class LovinHelper
{
    public static Job TryDoHookup(Pawn pawn)
    {
        Pawn partner = RomanceHelper.FindPartner(pawn);
        // if we can't actually find a partner, then the pawn either gives up and does something
        // else or does self lovin'
        if (partner == null)
        {
            if (!Settings.LovinModuleActive || Rand.Value <= .75f)
            {
                return null;
            }
            else
            {
                return TryDoSelfLovin(pawn);
            }
        }

        Building_Bed bed = CoreLovinHelper.FindBed(pawn, partner);
        if (bed == null)
        {
            return null;
        }

        return JobMaker.MakeJob(RomanceJobDefOf.LeadHookup, partner, bed);
    }

    public static Job TryDoSelfLovin(Pawn pawn)
    {
        return null;
    }

    public static Toil EvaluateLovin(LovinProps props)
    {
        return null;
    }
}