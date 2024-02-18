using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public class RomanceComp : ThingComp
{
    private RomanceTracker romanceTracker;

    public RomanceTracker RomanceTracker => romanceTracker;

    public override void PostExposeData()
    {
        Scribe_Deep.Look(ref romanceTracker, "romance");
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        romanceTracker = new RomanceTracker();
    }

    public override void CompTick()
    {
        romanceTracker?.Tick();
    }
}