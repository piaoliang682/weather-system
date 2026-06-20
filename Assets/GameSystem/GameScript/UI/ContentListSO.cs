using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewContentsList", menuName = "Game/ContentsList")]
public class ContentListSO : ScriptableObject
{
    public string topic;
    public List<ContentSO> contentListSO = new List<ContentSO>();
}
