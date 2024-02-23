﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Personality.Romance;

public static class MindCardUtility
{
    private static readonly string LikesText = "PawnLikes".Translate();
    private static readonly string DislikesText = "PawnDislikes".Translate();

    private static float CardHeight => 15f;

    private static readonly float LabelWidth = Math.Max(Text.CalcSize(LikesText).x, Text.CalcSize(DislikesText).x);

    public static void DrawMindCardRomanceStuff(Rect rect, Pawn pawn)
    {
        RomanceComp comp = pawn.GetComp<RomanceComp>();
        AttractionTracker attractionTracker = comp?.AttractionTracker;

        float yStart = rect.y;

        //List<string> likes = (from c in attractionTracker.HairColorPreferences
        //                      where c.Value >= 0.25f
        //                      select c.Pref.ToString()).ToList();

        //List<string> dislikes = (from c in attractionTracker.HairColorPreferences
        //                         where c.Value <= 0.25f
        //                         select c.Pref.ToString()).ToList();

        //Rect subRect = new(rect.x, rect.y, rect.width, CalcSizeOfSection(likes));
        //DrawLikesAndDislikes(pawn, likes, dislikes, subRect);
    }

    //private static void DrawLikesAndDislikes(Pawn pawn, List<string> likes, List<string> dislikes, Rect rect)
    //{
    //    float yStart = rect.y;
    //    if (likes.Count > 0)
    //    {
    //        Text.Font = GameFont.Small;
    //        Rect labelRect = new(rect.x, rect.y, LabelWidth, Text.CalcHeight(LikesText, LabelWidth));
    //        Widgets.Label(labelRect, LikesText);
    //        foreach (var like in likes)
    //        {
    //            Text.Font = GameFont.Tiny;
    //            Vector2 cardSize = Text.CalcSize(like);
    //            Rect cardRect = new(rect.x + LabelWidth + 5f, yStart, cardSize.x, cardSize.y);
    //            Widgets.Label(cardRect, like);
    //            yStart += cardSize.y;
    //        }
    //        if (yStart < labelRect.height - rect.y)
    //        {
    //            yStart = labelRect.yMax;
    //        }
    //    }

    //    if (dislikes.Count > 0)
    //    {
    //        Text.Font = GameFont.Small;
    //        Rect labelRect = new(rect.x, yStart, LabelWidth, Text.CalcHeight(DislikesText, LabelWidth));
    //        Widgets.Label(labelRect, DislikesText);
    //        foreach (var dislike in dislikes)
    //        {
    //            Text.Font = GameFont.Tiny;
    //            Vector2 cardSize = Text.CalcSize(dislike);
    //            Rect cardRect = new(rect.x + LabelWidth + 5f, yStart, cardSize.x, cardSize.y);
    //            Widgets.Label(cardRect, dislike);
    //            yStart += cardSize.y;
    //        }
    //        if (yStart < labelRect.height - rect.y)
    //        {
    //            yStart = labelRect.yMax;
    //        }
    //    }
    //}

    private static float CalcSizeOfSection(List<string> itemsToRender)
    {
        if (itemsToRender.Count == 0) return 0f;

        return itemsToRender.Count * CardHeight;
    }
}