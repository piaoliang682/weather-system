using UnityEngine;
[System.Serializable]
public class StatValue
{
    public float BaseValue;
    public float AdditiveBonus;
    public float MultiplicativeBonus = 1f;

    public float FinalValue => (BaseValue + AdditiveBonus) * MultiplicativeBonus;
}
