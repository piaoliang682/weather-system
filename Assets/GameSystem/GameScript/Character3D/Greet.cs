using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerBehaviour()
    {
        Debug.Log("debug");
        // Check nearby NPC
        Collider[] hits = Physics.OverlapSphere(transform.position, 5f);

        foreach (var hit in hits)
        {
            //NPCResponse npc = hit.GetComponent<NPCResponse>();
            //if (npc != null)
            //{
            //    npc.TriggerBehaviour();
            //    return;
            //}
        }
    }

}
