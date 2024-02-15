using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance.HarmonyPatches;

[StaticConstructorOnStartup]
public static class HarmonyStartup
{
    static HarmonyStartup()
    {
        HarmonyLib.Harmony harmonyInstance = new("effervescent.personalityplease.romance");
        harmonyInstance.PatchAll();
    }
}
