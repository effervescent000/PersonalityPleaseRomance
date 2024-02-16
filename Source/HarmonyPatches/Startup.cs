using HarmonyLib;
using Verse;

namespace Personality.Romance.HarmonyPatches;

[StaticConstructorOnStartup]
public static class HarmonyStartup
{
    static HarmonyStartup()
    {
        Harmony harmonyInstance = new("effervescent.personalityplease.romance");
        harmonyInstance.PatchAll();
    }
}
