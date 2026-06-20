using UnityEngine;

[CreateAssetMenu(
    fileName = "PopUpDefinition",
    menuName = "Game/PopUpDefinition",
    order = 0)]
public class PopUpDefinition : ScriptableObject
{
    [Header("Interaction Settings")]
    [SerializeField, Tooltip("Message shown to player when near this object.")]
    private string promptTitle = "Stole this item?";

    [SerializeField, TextArea, Tooltip("Detailed message shown when interacting.")]
    private string promptText = "Stealing is a crime, but I am poor.";

    [SerializeField]
    private Sprite icon;

    [Header("Feedback Messages")]
    [SerializeField, TextArea, Tooltip("Message shown when confirmed.")]
    private string confirmFeed = "You took the item.";

    [SerializeField, TextArea, Tooltip("Message shown when rejected.")]
    private string rejectFeed = "You decided not to steal.";

    // --- Public Getters ---
    public string PromptTitle => promptTitle;
    public string PromptText => promptText;
    public Sprite Icon => icon;
    public string ConfirmFeed => confirmFeed;
    public string RejectFeed => rejectFeed;
}
