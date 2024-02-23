using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Personality.Romance.HarmonyPatches;

public static class PatchDrawRomance
{
    public static void Postfix(Rect rect, Pawn pawn)
    {
        MindCardUtility.DrawMindCardRomanceStuff(rect, pawn);
    }
}