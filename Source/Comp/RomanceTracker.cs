using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Personality.Romance;

public class RomanceTracker : IExposable
{
    private HashSet<RejectionItem> rejectionList = new();

    public RomanceTracker()
    {
        
    }

    public HashSet<RejectionItem> RejectionList => rejectionList;

    public void Tick()
    {
        List<RejectionItem> itemsToRemove = new();

        foreach (var item in rejectionList)
        {
            item.TicksSinceAsked++;
            if (item.TicksSinceAsked > GameHelper.TICKS_PER_DAY * 2)
            {
                itemsToRemove.Add(item);
            }
        }
        foreach (var item in itemsToRemove)
        {
            rejectionList.Remove(item);
        }
    }


    public void ExposeData()
    {
        Scribe_Collections.Look(ref rejectionList, "rejections", LookMode.Deep);
    }
}
