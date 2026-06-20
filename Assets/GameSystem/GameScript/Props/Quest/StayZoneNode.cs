using UnityEngine;

public class StayZoneNode : MonoBehaviour
{
    public string triggerTag = "Player";
    public float requiredStayTime = 3f;

    private float stayTimer = 0f;
    private bool playerInside = false;
    private bool isComplete = false;
    public ProgressBar progressBar;

    private void Start()
    {
        progressBar.SetImageProgress(0);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            playerInside = true;
            stayTimer = 0f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            playerInside = false;
            stayTimer = 0f;
        }
    }

    void Update()
    {

        if (playerInside)
        {
            stayTimer += Time.deltaTime;
            progressBar.SetImageProgress(stayTimer/requiredStayTime);
            if (stayTimer >= requiredStayTime&&!isComplete)
            {
                isComplete = true;
                 QuestManager.Instance.CompleteObjective("StayZone");
                // Player has stayed long enough ¡ª fire event, then reset / disable
                //GlobalEvent.OnPlayerInZone?.Invoke();
                // optionally disable to avoid repeated invokes
                playerInside = false;
            }
        }
    }
}
