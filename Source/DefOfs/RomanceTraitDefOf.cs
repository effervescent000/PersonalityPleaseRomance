using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personality.Romance;

[DefOf]
public static class RomanceTraitDefOf
{
    public static TraitDef Straight;
    public static TraitDef AroAce;
    public static TraitDef AceHetero;
    public static TraitDef AceBi;
    public static TraitDef AceHomo;

    // asexual should never actually show up. if it does you broke it!!!!!!
    public static TraitDef Asexual;

    static RomanceTraitDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RomanceTraitDefOf));
    }

}
