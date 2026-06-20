using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SceneRegistry", menuName = "Game/SceneRegistry", order = 1)]
public class GameSceneRegistry : ScriptableObject
{
    // Start is called before the first frame update
    public string startSceneName;
    public string lobbySceneName;
}
