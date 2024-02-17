using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public static class PawnHelper
{
    public static bool IsOk(this Pawn pawn)
    {
        if (pawn == null || !pawn.health.capacities.CanBeAwake || pawn.health.Downed || pawn.health.Dead) return false;

        return true;
    }
}
