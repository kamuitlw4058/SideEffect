using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlotManager
{

    public static List<TargetSlot> targetSlots = new List<TargetSlot>();

    public static void RegisterTargetSlot(TargetSlot slot)
    {
        if (!targetSlots.Contains(slot))
        {
            targetSlots.Add(slot);
        }
    }

}
