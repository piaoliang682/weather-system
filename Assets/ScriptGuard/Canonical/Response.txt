using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NPCResponse : MonoBehaviour
{
    public float interactionDistance = 5f;
    private Animator anim;
    private Transform player;

    private void Start()
    {
        anim = GetComponent<AIController>().anim;
        player = GameReference.Player.transform;
    }

    public void TriggerBehaviour()
    {
        Debug.Log("emoji trigger");
        // Stun NPC for 3 seconds
        GetComponent<AIController>().SetStunt(3);

        // Calculate distance
        float dist = Vector3.Distance(transform.position, player.position);

        // Face player (only Y rotate)
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
        {
            Debug.Log("rotate");
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = lookRot;
        }

        // Play emoji animation if close enough
        if (dist <= interactionDistance)
        {
            anim.SetTrigger("Emoji");
        }

        // Delay quest completion
        StartCoroutine(CompleteObjectiveDelayed(3f));
    }

    private IEnumerator CompleteObjectiveDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        QuestManager.Instance.CompleteObjective("MakeFriends");
    }


}
