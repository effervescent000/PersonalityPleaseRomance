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

    public static ThoughtDef PP_TurnedMeDownForIntimacy;
    public static ThoughtDef PP_TurnedMeDownForIntimacy_Mood;
    public static ThoughtDef PPR_HadToRejectSomeoneForIntimacy;

    static RomanceThoughtDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RomanceThoughtDefOf));
    }
}