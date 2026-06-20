using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class CharacterStats : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth = 50f;
    [Header("Stats")]
    public int baseScoreValue = 10;
    public float baseAttack = 10f;
    public float baseDefense = 5f;
    public float baseMoveSpeed = 3f;
    public float baseHPRegen = 1f;
    public bool isDeadAnim;
    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onHealthChanged;

    [Header("UI Elements (Optional)")]
    public Image healthFillImage;     // ¡û Replaces Slider
    public Text healthText;
    public TMP_Text healthTMP_Text;
    [Header("Animation")]
    public Animator animator;
    public string deathAnimationStateName = "Death";
    public float destroyDelay = 3f;

    [Header("Death Effects")]
    public GameObject deathEffectPrefab;
    public Vector3 effectOffset = Vector3.zero;

    public float debugValue;

    //private Dictionary<StatModifierBuff.StatType, float> additiveModifiers =
    //    new Dictionary<StatModifierBuff.StatType, float>();

    //private Dictionary<StatModifierBuff.StatType, float> multiplierModifiers =
    //    new Dictionary<StatModifierBuff.StatType, float>();


    private bool isDead = false;

    [Header("Stat Definitions (Assign All Here)")]
    public StatRegistry statRegistry;

    private Dictionary<StatType, StatValue> runtimeStats = new Dictionary<StatType, StatValue>();
    private Dictionary<StatType, StatDefinition> statLookup = new Dictionary<StatType, StatDefinition>();

    private void Awake()
    {

        if (gameObject.tag == "Enemy") GameReference.EnemyRegistry.Add(gameObject);
        GameReference.RegisterPauseObject(gameObject);
    }
    private void Start()
    {
        UpdateHealthUI();

        if (animator == null)
            animator = GetComponent<Animator>();
    }
    private void Update()
    {
        //debugValue = GetModifiedStat(StatModifierBuff.StatType.Attack);
    }

    public void InitializeStats()
    {
        runtimeStats.Clear();
        statLookup.Clear();

        if (statRegistry == null)
        {
            Debug.LogWarning("StatRegistry not assigned to CharacterStats!");
            return;
        }

        var lookup = statRegistry.GetLookup();
        foreach (var def in lookup.Values)
        {
            if (def == null) continue;

            statLookup[def.statType] = def;
            runtimeStats[def.statType] = new StatValue
            {
                BaseValue = Mathf.Clamp(def.defaultValue, def.minValue, def.maxValue)
            };
        }

        Debug.Log($"Initialized {runtimeStats.Count} stats from registry.");
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }

        UpdateHealthUI();
        onHealthChanged.Invoke();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} dead");
        onDeath.Invoke();

        if (animator != null)
        {
            DisableCharacterControls();
            animator.SetTrigger("Die");
            StartCoroutine(DieCoroutine());
        }
        else
        {
            Debug.Log(gameObject.name+"dead");


            gameObject.SetActive(false);
        }

            if (gameObject.tag == "Enemy") GameReference.EnemyRegistry.Remove(gameObject);
        if (ScoreManager.Instance != null)
        {
            //ScoreManager.Instance.AddScore(((int)GetModifiedStat(StatModifierBuff.StatType.Score)));
            //Debug.Log($"{((int)GetModifiedStat(StatModifierBuff.StatType.Score))} added");
        }

            if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position + effectOffset, Quaternion.identity);
        }
    }

    private void DisableCharacterControls()
    {
        var controllers = GetComponents<MonoBehaviour>();
        foreach (var ctrl in controllers)
        {
            if (ctrl != this)
                ctrl.enabled = false;
        }
    }
//modify stat, not permanent value changed
    //public float GetModifiedStat(StatModifierBuff.StatType type)
    //{
    //    float baseValue = type switch
    //    {
    //        StatModifierBuff.StatType.Attack => baseAttack,
    //        StatModifierBuff.StatType.Defense => baseDefense,
    //        StatModifierBuff.StatType.MoveSpeed => baseMoveSpeed,
    //        StatModifierBuff.StatType.HPRegen => baseHPRegen,
    //        StatModifierBuff.StatType.Score => baseScoreValue,
    //        _ => 0f
    //    };

    //    float add = additiveModifiers.ContainsKey(type) ? additiveModifiers[type] : 0;
    //    float mult = multiplierModifiers.ContainsKey(type) ? multiplierModifiers[type] : 1;
    //    return (baseValue + add) * mult;
    //}

    //public void ApplyModifier(StatModifierBuff.StatType type, float mult, float add)
    //{
    //    // Additive
    //    if (!additiveModifiers.TryGetValue(type, out float currentAdd))
    //    {
    //        currentAdd = 0f;
    //    }
    //    additiveModifiers[type] = currentAdd + add;

    //    // Multiplier
    //    if (!multiplierModifiers.TryGetValue(type, out float currentMult))
    //    {
    //        currentMult = 1f;
    //    }
    //    multiplierModifiers[type] = currentMult * mult;

    //}

    //public void RemoveModifier(StatModifierBuff.StatType type, float mult, float add)
    //{
    //    additiveModifiers[type] -= add;
    //    multiplierModifiers[type] /= mult;
    //}

    private IEnumerator DieCoroutine()
    {
        if (isDeadAnim)
        {
            // Wait until the ¡°Die¡± state has started playing
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

            // Wait until the animation finishes (normalizedTime > 1 means complete)
            yield return new WaitUntil(() =>
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
        }
        gameObject.SetActive(false);
        Debug.Log("die");
    }

    private void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
        if (healthTMP_Text != null) {
            healthTMP_Text.text = $"{currentHealth}/{maxHealth}";
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}