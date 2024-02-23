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
    public float PhysicalScore = 0f;
    public float PersonalityScore = 0f;
    public int TicksSinceCache = 0;

    public AttractionEvaluation(Pawn target)
    {
        Target = target;
    }

    public void MakeEval(AttractionTracker attraction)
    {
        TicksSinceCache = 0;
        foreach (var pref in attraction.AllPrefs)
        {
            PhysicalScore += pref.CalcAttractionEffect(Target);
        }
    }

    public void Tick()
    {
        TicksSinceCache++;
    }
}