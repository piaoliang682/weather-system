using UnityEngine;

public class GameWinNode : MonoBehaviour
{
    // Tag of the object that triggers win (e.g. "Player")
    public string triggerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        // Check if the collider belongs to the expected object
        if (other.CompareTag(triggerTag))
        {
            // Assuming you have a GameManager instance
            SystemGameManager.Instance.GameWin();
        }
    }
}
