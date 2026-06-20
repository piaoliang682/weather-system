using UnityEngine;

[System.Serializable]
public class StatChange
{
    public float happiness = 0;
    public float stress = 0;
    public float guilt = 0;
    public float coins = 0;
}

public class StealInteractable : InteractableBase
{
    [Header("Stat Changes")]
    [Tooltip("Stats if player is observed")]
    public StatChange observedStats = new StatChange { happiness = -10, stress = 20, guilt = 0 };

    [Tooltip("Stats if player is NOT observed")]
    public StatChange unobservedStats = new StatChange { happiness = 10, stress = 5, guilt = 5, coins =0};

    [Tooltip("Stats if player resists interaction")]
    public StatChange resistStats = new StatChange { happiness = -5, stress = -5, guilt = -3 };

    private bool hasInteracted = false;

    protected override void OnConfirmed()
    {
        if (hasInteracted) return;
        hasInteracted = true;

        bool observed = IsPlayerObserved();

        if (observed)
        {
            AdjustStats(observedStats);
            confirmFeed += ($"You were caught stealing! Happiness {FormatStat(observedStats.happiness)}, Stress {FormatStat(observedStats.stress)}, Guilt {FormatStat(observedStats.guilt)}.");
        }
        else
        {
            AdjustStats(unobservedStats);
           confirmFeed+=($"You stole the item! Happiness {FormatStat(unobservedStats.happiness)}, Stress {FormatStat(unobservedStats.stress)}, Guilt {FormatStat(unobservedStats.guilt)}, Coins {FormatStat(unobservedStats.coins)}.");
        }

        base.OnConfirmed();
    }

    private bool IsPlayerObserved()
    {
        NPCObserver[] observers = FindObjectsOfType<NPCObserver>();
        foreach (var npc in observers)
        {
            if (npc.CanSeePlayer(GameReference.Player.transform))
                return true;
        }
        return false;
    }


    protected override void OnReject()
    {
        hasInteracted = true;
        AdjustStats(resistStats);
        rejectFeed+=$"You resisted temptation. Happiness {FormatStat(resistStats.happiness)}, Stress {FormatStat(resistStats.stress)}, Guilt {FormatStat(resistStats.guilt)}.";
        base.OnReject();
    }

    private void AdjustStats(StatChange changes)
    {
        if (GameReference.TempGameStats == null) return;

        GameReference.TempGameStats["Happiness"] += changes.happiness;
        GameReference.TempGameStats["Stress"] += changes.stress;
        GameReference.TempGameStats["Guilt"] += changes.guilt;
        GameReference.TempGameStats["Coins"] += changes.coins;
    }


    private string FormatStat(float value)
    {
        return value >= 0 ? $"+{value}" : $"{value}";
    }
}
