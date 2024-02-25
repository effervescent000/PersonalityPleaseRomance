using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public class AttractionEvaluation
{
    public Pawn Target;
    public float PhysicalScore = 1f;
    public float PersonalityScore = 1f;
    public int TicksSinceCache = 0;

    public AttractionEvaluation(Pawn target)
    {
        Target = target;
    }

    public void MakeEval(AttractionTracker attraction)
    {
        TicksSinceCache = 0;
        foreach (Preference pref in attraction.AllPrefs)
        {
            PhysicalScore += pref.CalcAttractionEffect(Target);
        }
        PersonalityScore = attraction.Pawn.relations.CompatibilityWith(Target);
    }

    public void Tick()
    {
        TicksSinceCache++;
    }
}