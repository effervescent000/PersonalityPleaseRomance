using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public class RomanceComp : ThingComp
{
    private RomanceTracker romanceTracker = new();
    private AttractionTracker attractionTracker = new();

    public RomanceTracker RomanceTracker => romanceTracker;

    public override void PostExposeData()
    {
        Scribe_Deep.Look(ref romanceTracker, "romance");
        Scribe_Deep.Look(ref attractionTracker, "attraction");
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if (!respawningAfterLoad)
        {
            attractionTracker.Initialize(parent as Pawn);
        }
    }

    public override void CompTick()
    {
        romanceTracker?.Tick();
    }
}