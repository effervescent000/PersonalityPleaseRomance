using Verse;

namespace Personality.Romance;

public class Settings : ModSettings
{
    public SettingValues<int> MaxInteractionDistance = new(100, "PPR.MaxInteractDistance.Label", "PPR.MaxInteractDistance.Desc", 50, 500);

    // permanently enabled rn for testing
    public bool LovinEnabled = true;

    //set automatically
    public static bool LovinModuleActive = false;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref MaxInteractionDistance.Value, "maxInteractionDistance", 100);
    }
}