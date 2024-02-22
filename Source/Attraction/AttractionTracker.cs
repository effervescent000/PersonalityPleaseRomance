using Personality.Core;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Personality.Romance;

public class AttractionTracker : IExposable
{
    public HashSet<Preference<string>> HairStylePreferences = new();
    public HashSet<Preference<string>> BodyPreferences = new();
    public HashSet<Preference<Color>> HairColorPreferences = new();
    public HashSet<Preference<string>> HeadTypePreferences = new();

    public AttractionTracker()
    { }

    public void Initialize(Pawn pawn)
    {
        int seed = pawn.GetSeed();
        System.Random random = new(seed);

        while (HairStylePreferences.Count < 4)
        {
            List<string> currentHairTags = (from tag in HairStylePreferences
                                            select tag.Pref).ToList();

            string newTag = AttractionHelper.HairStyleTags.RandomElement();
            if (!currentHairTags.Contains(newTag))
            {
                HairStylePreferences.Add(new Preference<string> { Pref = newTag, Value = GetUnmoderateValue(random) });
            }
        }
        Log.Message("Completed HairStlePrefs");

        List<BodyTypeDef> validBodyTypes = MakeBodyTypes(pawn);

        while (BodyPreferences.Count < Math.Floor(validBodyTypes.Count * 0.4f))
        {
            List<string> currentPrefs = (from body in BodyPreferences
                                         select body.Pref).ToList();
            string selection = validBodyTypes.RandomElement().defName;
            if (!currentPrefs.Contains(selection))
            {
                BodyPreferences.Add(new Preference<string> { Pref = selection, Value = GetUnmoderateValue(random) });
            }
        }
        Log.Message("Completed BodyTypePrefs");

        while (HairColorPreferences.Count < 3)
        {
            List<Color> currentPrefs = (from color in HairColorPreferences
                                        select color.Pref).ToList();
            Color selection = (Color)AttractionHelper.HairColors.RandomElement();
            if (!currentPrefs.Contains(selection))
            {
                HairColorPreferences.Add(new Preference<Color> { Pref = selection, Value = GetUnmoderateValue(random) });
            }
        }
        Log.Message("Completed HairColorPrefs");

        // give pawns one head type attraction for each gender they're attracted to
        if (pawn.IsAttractedToMen())
        {
            string selection = AttractionHelper.MaleHeads.RandomElement().defName;
            HeadTypePreferences.Add(new Preference<string> { Pref = selection, Value = GetUnmoderateValue(random) });
        }
        if (pawn.IsAttractedToWomen())
        {
            string selection = AttractionHelper.FemaleHeads.RandomElement().defName;
            HeadTypePreferences.Add(new Preference<string> { Pref = selection, Value = GetUnmoderateValue(random) });
        }
    }

    private float GetUnmoderateValue(System.Random rand)
    {
        while (true)
        {
            float newValue = rand.Next(-100, 100);
            if (newValue < 25)
            {
                if (newValue > -25)
                {
                    continue;
                }
            }
            return newValue / 100f;
        }
    }

    private List<BodyTypeDef> MakeBodyTypes(Pawn pawn)
    {
        List<BodyTypeDef> validBodyTypes = AttractionHelper.GenericBodyTypes.ListFullCopy();

        if (pawn.IsAttractedToMen())
        {
            validBodyTypes.Add(BodyTypeDefOf.Male);
        }
        if (pawn.IsAttractedToWomen())
        {
            validBodyTypes.Add(BodyTypeDefOf.Female);
        }

        return validBodyTypes;
    }

    public void ExposeData()
    {
        Scribe_Collections.Look(ref HairStylePreferences, "hairStylePrefs", LookMode.Deep);
        Scribe_Collections.Look(ref BodyPreferences, "bodyPreferences", LookMode.Deep);
        Scribe_Collections.Look(ref HairColorPreferences, "colorPreferences", LookMode.Deep);
    }
}