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
    public static JobDef DoSelfLovin;
    public static JobDef LeadHookup;

    static RomanceJobDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RomanceJobDefOf));
    }
}