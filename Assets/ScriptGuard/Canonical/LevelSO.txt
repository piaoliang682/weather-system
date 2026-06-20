using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level List", fileName = "LevelList")]
public class LevelSO : ScriptableObject
{
    public List<LevelItemSO> levelItems = new List<LevelItemSO>();
}
