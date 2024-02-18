using RimWorld;
using UnityEngine;
using Verse;

namespace Personality.Romance;

public class RomanceMod : Mod
{
    public static Settings settings;

    public RomanceMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<Settings>();
    }

    public override string SettingsCategory()
    {
        return "PPR.Romance".Translate();
    }

    public override void DoSettingsWindowContents(Rect canvas)
    {
        Listing_Standard mainList = new()
        {
            ColumnWidth = (canvas.width / 2)
        };
        mainList.Begin(canvas);

        Listing_Standard section = BeginNewSection(mainList);

        settings.MaxInteractionDistance.Value = DrawLabeledSlider(section, settings.MaxInteractionDistance.Value, 50, 500, settings.MaxInteractionDistance.CurrentLabel, settings.MaxInteractionDistance.Description);

        EndSection(mainList, section);
        mainList.End();
    }

    private int DrawLabeledSlider(Listing_Standard list, int defaultValue, int min, int max, string label, string description)
    {
        list.Label(label, tooltip: description);
        list.maxOneColumn = true;
        int newValue = (int)list.Slider(defaultValue, min, max);
        return newValue;
    }

    private Listing_Standard BeginNewSection(Listing_Standard outerList, float height = 50f)
    {
        // TODO eventually add section headers
        Listing_Standard subList = outerList.BeginSection(height);
        return subList;
    }

    private void EndSection(Listing_Standard outerList, Listing_Standard sectionList)
    {
        // TODO add local reset buttons to reset values per section? otherwise might not need this func
        outerList.EndSection(sectionList);
    }
}