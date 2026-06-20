using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPanel : InteractableBase
{

    [SerializeField] private string key;
    [SerializeField] private int valueOnConfirm;
    [SerializeField] private int valueOnReject;


    private void Start()
    {

    }

    protected override void OnConfirmed()
    {

        // Assume CharacterStats.Instance has a dictionary like: Dictionary<string, float> stats
        // e.g., stats["Coins"] stores current coin value

        if (!GameReference.TempGameStats.TryGetValue("Coins", out float currentCoins))
        {
            Debug.LogWarning("Coins not found in CharacterStats dictionary!");
            return;
        }

        float coffeeCost = 10f; // Set the cost of coffee

        if (currentCoins >= coffeeCost)
        {
            // Deduct coins
            GameReference.TempGameStats["Coins"] = currentCoins - coffeeCost;

            Debug.Log("Coffee purchased! Coins left: " + GameReference.TempGameStats["Coins"]);

            GameReference.TempGameStats[key] += valueOnReject;

            confirmFeed += $"\n <color=green>{key}  + {valueOnReject}<color=green>";

            // TODO: Add other effects, e.g., increase happiness, play sound, etc.
        }
        else
        {
            Debug.Log("Not enough coins to buy coffee.");
            // TODO: maybe show a popup saying "Not enough coins"
        }

        base.OnConfirmed();
    }


    protected override void OnReject()
    {

        GameReference.TempGameStats[key] += valueOnReject;
        rejectFeed+= $"\n <color=green>{key} {valueOnReject}<color=green>";
        base.OnReject();

    }
}
