using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personality.Romance.HarmonyPatches;

public static class PPL
{
    public static void Patch(Harmony harmony)
    {
        Type PPLLovinHelper = AccessTools.TypeByName("Personality.Lovin.LovinHelper");

        harmony.Patch(AccessTools.Method(PPLLovinHelper, "GetAttractionFactorFor"), postfix: new HarmonyMethod(AccessTools.Method(typeof(PatchGetAttractionFactor), nameof(PatchGetAttractionFactor.Postfix))));
    }
}