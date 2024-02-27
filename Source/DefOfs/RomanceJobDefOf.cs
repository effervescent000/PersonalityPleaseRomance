using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

[DefOf]
public static class RomanceJobDefOf
{
    public static JobDef DoCasualLovin;
    public static JobDef LeadHookup;

    public static JobDef PP_InitiateIntimateLovin;
    public static JobDef PP_DoIntimateLovin;

    static RomanceJobDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RomanceJobDefOf));
    }
}