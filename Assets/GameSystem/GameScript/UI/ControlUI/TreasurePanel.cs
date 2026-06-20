using UnityEngine;

public class TreasurePanel : InteractivePanelBase
{



    protected override string GetFailureMessage()
    {
        return "This chest is empty.";
    }
}
