using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personality.Romance;

[DefOf]
public static class RomanceSkillDefOf
{
    public static SkillDef Lovin;

    static RomanceSkillDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(RomanceSkillDefOf));
    }
}