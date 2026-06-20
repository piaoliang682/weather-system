using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScoreItem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private string playerTag = "Player";

    private void Reset()
    {
        // Ensure collider is trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        Destroy(gameObject);
    }
}
