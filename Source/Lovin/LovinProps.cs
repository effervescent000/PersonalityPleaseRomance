using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public enum LovinContext
{
    SelfLovin,
    Casual,
    Intimate,
}

public class LovinProps
{
    private Pawn actor;
    private Pawn partner;
    private LovinContext context;

    public LovinProps(Pawn actor, Pawn partner, LovinContext context)
    {
        this.actor = actor;
        this.partner = partner;
        this.context = context;
    }
}