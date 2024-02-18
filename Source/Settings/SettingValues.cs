using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public class SettingValues<T>
{
    public T Value;
    private string label;
    private string description;
    public T MinValue;
    public T MaxValue;

    public SettingValues(T value, string label, string description, T minValue, T maxValue)
    {
        Value = value;
        this.label = label;
        this.description = description;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public string CurrentLabel => $"{Label}: {Value}";

    public string Label => label.Translate();

    public string Description => description.Translate();
}