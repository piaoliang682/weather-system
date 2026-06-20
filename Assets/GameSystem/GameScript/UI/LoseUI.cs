using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LoseUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button lobbyButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button reviveButton;
    [SerializeField] private Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Button GetLobbyButton()
    {
        return lobbyButton;
    }
    public Button GetReviveButton()
    {
        return reviveButton;
    }
    public Button GetExitButton()
    {
        return exitButton;
    }
    public Button GetRetryButton()
    {
        return retryButton;
    }

}
