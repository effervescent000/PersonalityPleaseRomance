using HarmonyLib;
using Personality.Romance.HarmonyPatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

[StaticConstructorOnStartup]
public static class OnStartup
{
    static OnStartup()
    {
        if (ModsConfig.IsActive("effervescent.personalityplease.lovin"))
        {
            Settings.LovinModuleActive = true;
        }
        if (ModsConfig.IsActive("effervescent.personalityplease"))
        {
            Settings.MainModuleActive = true;
        }

        Harmony harmony = new("effervescent.personalityplease.romance");
        harmony.PatchAll();

        if (Settings.MainModuleActive)
        {
            PPMain.Patch(harmony);
        }
        if (Settings.LovinModuleActive)
        {
            PPL.Patch(harmony);
        }
    }
}