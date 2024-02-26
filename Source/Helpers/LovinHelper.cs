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

    public static void FinishLovin(LovinProps props)
    {
        Thought_Memory thought_memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.GotSomeLovin);
        props.Actor.needs.mood?.thoughts.memories.TryGainMemory(thought_memory, props.Partner);

        CoreLovinHelper.TryPregnancy(props);
    }

    // noops for patching below here --------------------------------

    public static Job TryDoSelfLovin(Pawn pawn)
    {
        return null;
    }

    public static Toil EvaluateLovin(LovinProps props)
    {
        return null;
    }

    public static float GetLovinBaseChance(Pawn pawn)
    {
        return 0f;
    }
}