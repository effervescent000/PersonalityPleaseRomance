﻿using Verse;

namespace Personality.Romance;

public class RomanceComp : ThingComp
{
    public RomanceTracker RomanceTracker = new();
    public AttractionTracker AttractionTracker = new();

    public override void PostExposeData()
    {
        Scribe_Deep.Look(ref RomanceTracker, "romance");
        Scribe_Deep.Look(ref AttractionTracker, "attraction");
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
        if (!respawningAfterLoad)
        {
            AttractionTracker.Initialize(parent as Pawn);
        }
    }

    public override void CompTick()
    {
        RomanceTracker?.Tick();
        AttractionTracker.Tick();
    }
}