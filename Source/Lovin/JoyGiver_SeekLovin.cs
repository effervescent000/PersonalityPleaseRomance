using Personality.Core;
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
        if (Settings.LovinModuleActive)
        {
            if (Rand.Value <= .25f)
            {
                return LovinHelper.TryDoSelfLovin(pawn);
            }
        }
        return LovinHelper.TryDoHookup(pawn);
    }
}