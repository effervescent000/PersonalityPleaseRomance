using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public class Preference<T> : IExposable
{
    public T Pref;
    public float Value;

    public Preference()
    {
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref Pref, "pref");
        Scribe_Values.Look(ref Value, "value");
    }
}