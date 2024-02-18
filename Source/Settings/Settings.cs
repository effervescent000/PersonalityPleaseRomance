using Verse;

namespace Personality.Romance;

public class Settings : ModSettings
{
    //public int maxInteractionDistance = 100;
    public SettingValues<int> MaxInteractionDistance = new(100, "PPR.MaxInteractDistance.Label", "PPR.MaxInteractDistance.Desc", 50, 500);

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref MaxInteractionDistance.Value, "maxInteractionDistance", 100);
    }
}