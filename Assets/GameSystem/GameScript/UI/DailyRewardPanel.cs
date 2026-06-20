using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DailyRewardPanel : InteractivePanelBase
{
    [Header("Reward Data Source")]
    [Tooltip("Reference to the ShopSO that contains daily reward items.")]
    public ShopSO rewardDataSO;

    [SerializeField] private Transform rewardContainer;          // parent transform for instantiated UI slots

    [Header("UI References (TMP)")]
    public TMP_Text streakText;
    public TMP_Text timerText;
    private List<ShopItemInstance> dailyRewardInstances = new List<ShopItemInstance>();
    private int currentDayIndex;
    private DateTime lastClaimTime;
    private bool rewardClaimedToday;

    private int daysPassed;
    private const string LAST_CLAIM_KEY = "DailyReward_LastClaim";
    private const string STREAK_KEY = "DailyReward_Streak";
    private const string INSTANCE_SAVE_KEY = "DailyReward_ItemStates";


    private List<GameObject> instantiatedSlots = new List<GameObject>();
    private List<ShopItemUI> instantiatedItems = new List<ShopItemUI>();
    protected override void Awake()
    {
        LoadRewardsFromShop();
        LoadRewardData();

        base.Awake();

    }

    private void Start()
    {

        GenerateUI();
        CanClaimReward();
        UpdateCurrentDayIndex();

    }

    /// <summary>
    /// Load rewards from the linked ShopSO asset.
    /// </summary>

    protected override void ShowPanel()
    {
        UpdateCurrentDayIndex();
    }



    public void CanClaimReward()
    {
        DateTime now = DateTime.Now;
        DateTime lastClaimDate = lastClaimTime.Date;
        DateTime today = now.Date;

        Debug.Log($"[DailyReward] Now: {now}, LastClaim: {lastClaimTime}, " +
                  $"CurrentDayIndex: {currentDayIndex}, ClaimedToday: {rewardClaimedToday}");

        if (lastClaimTime == DateTime.MinValue)
        {
            Debug.Log("✅ First claim ever → Allowed");

        }

        // Same calendar day → cannot claim again
        if (today == lastClaimDate && rewardClaimedToday)
        {
            Debug.Log("⛔ Already claimed today → Wait until after midnight.");

        }

        // After midnight → can claim
        if (today > lastClaimDate)
        {
            Debug.Log("✅ New day → Claim allowed.");

        }
    }




    private void ClaimReward(ShopItemSO rewardSO)
    {
        var reward = dailyRewardInstances.Find(r => r.itemData == rewardSO);
        if (reward == null || reward.IsClaimed())
        {
            //feedbackText.text = "Reward already claimed or unavailable.";
            return;
        }

        reward.Claim();
        GiveRewardToPlayer(reward.itemData);

        rewardClaimedToday = true;
        lastClaimTime = DateTime.Now;
        SaveRewardData();
        UpdateUI();
    }


    private void GiveRewardToPlayer(ShopItemSO reward)
    {
        Debug.Log($"[DailyReward] Player received: {reward.id}");
        reward.OnPurchased();

        // Optionally: connect to your inventory system
        // InventoryManager.Instance.AddItem(reward);
    }

    private void GenerateUI()
    {
        // Clear old slots
        foreach (var slot in instantiatedSlots)
            Destroy(slot);
        instantiatedSlots.Clear();
        instantiatedItems.Clear();

        int rewardIndex = 0;

        foreach (var group in rewardDataSO.itemGroup)
        {
            GameObject groupGO = Instantiate(group.GroupPrefab, rewardContainer.transform);
            ShopItemGroupUI groupUI = groupGO.GetComponentInChildren<ShopItemGroupUI>();
            groupUI.groupName.text = group.groupName;

            foreach (ShopItemInstance instance in dailyRewardInstances)
            {
                GameObject itemGO = Instantiate(instance.itemData.itemPrefab, groupUI.itemHolder);
                var itemUI = itemGO.GetComponent<ShopItemUI>();

                // basic setup without claim logic
                itemUI.Initialize(instance.itemData, null);

                instantiatedItems.Add(itemUI);
                rewardIndex++;
            }

            instantiatedSlots.Add(groupGO);
        }

        UpdateUI(); // now handles interactivity & visual state
    }


    private void UpdateUI()
    {
        for (int i = 0; i < dailyRewardInstances.Count; i++)
        {
            var instance = dailyRewardInstances[i];
            var itemUI = instantiatedItems[i];

            bool isClaimed = instance.IsClaimed();
            bool isToday = i == currentDayIndex;
            bool isPast = i < currentDayIndex;
            bool isFuture = i > currentDayIndex;

            // allow claim only for today and before (if unclaimed)
            bool canClaim = (isToday || isPast) && !isClaimed;

            // refresh display
            itemUI.SetClear(isClaimed);
            itemUI.SetFocus(isToday);
            itemUI.SetSpecial(i == currentDayIndex + 1);

            // update interactivity
            if (canClaim)
                itemUI.SetClaimButton(() => ClaimReward(instance.itemData));
            else
                itemUI.SetClaimButton(null);
        }
    }

    public void SetDate()
    {
        lastClaimTime= DateTime.Now;
        Debug.Log($"{(lastClaimTime)} time passed");
        lastClaimTime = lastClaimTime.Date.AddDays(-1);

        float timeInDays = (DateTime.Now - lastClaimTime).Minutes;
        Debug.Log($"{(lastClaimTime)} time passed" );
        UpdateCurrentDayIndex();
    }
    public void UpdateCurrentDayIndex()
    {
        if (dailyRewardInstances == null || dailyRewardInstances.Count == 0)
        {
            Debug.LogWarning("[DailyReward] ⚠️ No rewards loaded when updating day index.");
            return;
        }

        DateTime now = DateTime.Now;
        DateTime lastRewardDate = lastClaimTime.Date;
        DateTime today = now.Date;

        if (lastClaimTime == DateTime.MinValue)
        {
            currentDayIndex = 0;
            lastClaimTime = now;
        }

        // Advance by how many days have passed
        daysPassed = (today - lastRewardDate).Days;


        if (daysPassed > 0)
        {
            lastClaimTime = now;
            currentDayIndex += daysPassed;
            rewardClaimedToday = false;

            // Reached or exceeded last reward → full reset
            if (currentDayIndex >= dailyRewardInstances.Count)
            {
                Debug.Log($"🔁 All rewards completed ({currentDayIndex}/{dailyRewardInstances.Count}). Resetting...");
                ResetAllRewards();
                return;
            }

            Debug.Log($"📅 Day advanced by {daysPassed} → new index: {currentDayIndex}");
            UpdateUI();
            SaveRewardData();
        }
    }


    public void ResetAllRewards()
    {
        Debug.Log("🧹 [DailyRewardPanel] Resetting all rewards...");

        // 🗑️ 1. Clear saved PlayerPrefs data
        PlayerPrefs.DeleteKey(LAST_CLAIM_KEY);
        PlayerPrefs.DeleteKey(STREAK_KEY);
        PlayerPrefs.DeleteKey(INSTANCE_SAVE_KEY);
        PlayerPrefs.Save();

        // 🔄 2. Reset all in-memory reward instances
        foreach (ShopItemInstance instance in dailyRewardInstances)
        {
            if (instance.itemData.isLimitedStock)
                instance.currentStock = 1;
            else
                instance.currentStock = instance.itemData.maxStock; // optional, restore default

            instance.SetIsClaimed(false);
        }

        // 📅 3. Reset state variables
        currentDayIndex = 0;
        rewardClaimedToday = false;
        lastClaimTime = DateTime.MinValue;

        // 💾 4. Save and refresh UI
        SaveRewardData();
        GenerateUI();

        Debug.Log("✅ [DailyRewardPanel] All rewards reset. Day index = 0, claim state cleared.");
    }



    private void SaveRewardData()
    {
        PlayerPrefs.SetString(LAST_CLAIM_KEY, lastClaimTime.ToString());
        PlayerPrefs.SetInt(STREAK_KEY, currentDayIndex);

        // Serialize instance data
        List<SerializableShopItemInstance> serializedList = new List<SerializableShopItemInstance>();
        foreach (var instance in dailyRewardInstances)
        {
            serializedList.Add(instance.ToSerializable());
        }

        string json = JsonUtility.ToJson(new Wrapper<SerializableShopItemInstance>(serializedList));

        PlayerPrefs.SetString(INSTANCE_SAVE_KEY, json);
        PlayerPrefs.Save();

        // 🧩 Debug output
        Debug.Log($"✅ Saved reward data!\n" +
                  $"LAST_CLAIM_KEY: {lastClaimTime}\n" +
                  $"STREAK_KEY: {currentDayIndex}\n" +
                  $"INSTANCE_SAVE_KEY JSON:\n{json}");
    }



    private void LoadRewardData()
    {
        Debug.Log("🔍 [DailyRewardPanel] Loading reward data...");

        if (PlayerPrefs.HasKey(LAST_CLAIM_KEY))
        {
            string savedTime = PlayerPrefs.GetString(LAST_CLAIM_KEY);
            DateTime.TryParse(savedTime, out lastClaimTime);
            currentDayIndex = PlayerPrefs.GetInt(STREAK_KEY, 0);

            TimeSpan timeSince = DateTime.Now - lastClaimTime;
            rewardClaimedToday = timeSince.TotalHours < 24;

            Debug.Log($"✅ Loaded PlayerPrefs:\n" +
                      $"LAST_CLAIM_KEY: {savedTime}\n" +
                      $"STREAK_KEY: {currentDayIndex}\n" +
                      $"Hours since last claim: {timeSince.TotalHours:F2}\n" +
                      $"Reward claimed today: {rewardClaimedToday}");
        }
        else
        {
            lastClaimTime = DateTime.MinValue;
            currentDayIndex = 0;
            rewardClaimedToday = false;

            Debug.LogWarning("⚠ No previous daily reward data found. Using default values.");
        }

        if (currentDayIndex > dailyRewardInstances.Count - 1)
        {
            Debug.LogWarning($"⚠ currentDayIndex ({currentDayIndex}) is out of range. Resetting all rewards...");
            ResetAllRewards();
        }
        else
        {
            Debug.Log($"📅 Current day index: {currentDayIndex}, Rewards count: {dailyRewardInstances.Count}");
        }
    }
    private void LoadRewardsFromShop()
    {
        Debug.Log("🟦 LoadRewardsFromShop() called.");

        dailyRewardInstances.Clear();

        // Load previously saved state
        Dictionary<string, SerializableShopItemInstance> savedMap = new Dictionary<string, SerializableShopItemInstance>();

        if (PlayerPrefs.HasKey(INSTANCE_SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(INSTANCE_SAVE_KEY);
            Debug.Log($"📂 Loaded JSON from PlayerPrefs ({INSTANCE_SAVE_KEY}): {json}");

            Wrapper<SerializableShopItemInstance> wrapper = JsonUtility.FromJson<Wrapper<SerializableShopItemInstance>>(json);

            if (wrapper != null && wrapper.Items != null)
            {
                foreach (var data in wrapper.Items)
                {
                    savedMap[data.id] = data;
                    Debug.Log($"✅ data: {data.id}, Claimed={data.claimed}, Stock={data.currentStock}");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Wrapper or wrapper.Items is null when loading saved reward data.");
            }
        }
        else
        {
            Debug.Log("ℹ️ No saved PlayerPrefs data found for rewards.");
        }

        // Rebuild list and apply saved states
        foreach (var group in rewardDataSO.itemGroup)
        {
            if (group == null || group.items == null)
            {
                Debug.LogWarning("⚠️ Skipped null group or empty item list in rewardDataSO.");
                continue;
            }

            foreach (var item in group.items)
            {
                if (item == null)
                {
                    Debug.LogWarning("⚠️ Skipped null ShopItemSO entry.");
                    continue;
                }

                var instance = new ShopItemInstance(item);

                if (savedMap.TryGetValue(item.id, out var saved))
                {
                    instance.isClaimed = saved.claimed;
                    instance.currentStock = saved.currentStock;
                    Debug.Log($"🔁 Restored {item.id} from saved data → Claimed={saved.claimed}, Stock={saved.currentStock}");
                }
                else
                {
                    Debug.Log($"🆕 Created new instance for {item.id} (no saved data).");
                }

                dailyRewardInstances.Add(instance);
            }
        }

        Debug.Log($"✅ LoadRewardsFromShop completed. Total items loaded: {dailyRewardInstances.Count}");

        if (dailyRewardInstances.Count == 0)
            Debug.LogWarning("[DailyRewardPanel] ⚠️ No reward items found in ShopSO!");
    }

}

[System.Serializable]
public class Wrapper<T>
{
    public List<T> Items;
    public Wrapper(List<T> items) { Items = items; }
}
