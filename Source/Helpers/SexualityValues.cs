using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personality.Romance;

public class SexualityValues
{
    public TraitDef TraitDef;
    public float chance;

    public SexualityValues(TraitDef traitDef, float chance)
    {
        TraitDef = traitDef;
        this.chance = chance;
        
    }

}
