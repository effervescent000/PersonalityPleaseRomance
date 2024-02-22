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
    }
}