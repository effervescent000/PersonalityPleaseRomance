using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personality.Romance;

[DefOf]
public static class RomanceThoughtDefOf
{
    public static ThoughtDef PPR_TurnedMeDownForHookup;
    public static ThoughtDef PPR_TurnedMeDownForHookup_Mood;
    public static ThoughtDef PPR_HadToRejectSomeoneForHookup;

    static RomanceThoughtDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RomanceThoughtDefOf));
    }
}