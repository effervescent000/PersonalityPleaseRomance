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
        if (pawn == null || pawn.health.Downed || pawn.health.Dead) return false;

        return true;
    }

    public static bool IsBloodRelatedTo(this Pawn pawn, Pawn target)
    {
        var familyMembers = (from member in pawn.relations.FamilyByBlood
                             where member.ThingID == target.ThingID
                             select member).ToList();

        if (familyMembers.Count > 0) { return true; }
        return false;
    }

    public static void ThrowHeart(this Pawn pawn)
    {
        FleckMaker.ThrowMetaIcon(pawn.Position, pawn.Map, FleckDefOf.Heart);
    }
}