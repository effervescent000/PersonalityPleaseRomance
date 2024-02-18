using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public class RejectionItem : IExposable
{
    private Pawn pawn;
    public int TicksSinceAsked;

    public RejectionItem(Pawn pawn)
    {
        this.pawn = pawn;
        TicksSinceAsked = 0;
    }

    public RejectionItem Get => this;

    public void ExposeData()
    {
        Scribe_References.Look(ref pawn, "pawn");
        Scribe_Values.Look(ref TicksSinceAsked, "ticks");
    }
}