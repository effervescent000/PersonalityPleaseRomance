using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personality.Romance.HarmonyPatches;

public static class PPMain
{
    public static void Patch(Harmony harmony)
    {
        Type mainMindCardUtility = AccessTools.TypeByName("Personality.MindCardUtility");

        harmony.Patch(AccessTools.Method(mainMindCardUtility, "DrawRomance"), postfix: new HarmonyMethod(AccessTools.Method(typeof(PatchDrawRomance), nameof(PatchDrawRomance.Postfix))));
    }
}