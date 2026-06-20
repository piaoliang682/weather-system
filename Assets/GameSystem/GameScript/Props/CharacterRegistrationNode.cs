using System.Collections.Generic;
using UnityEngine;

public class CharacterRegistrationNode : MonoBehaviour
{
    [Tooltip("Tag of characters to register")]
    public string targetTag = "Enemy";

    //[HideInInspector]
    //public List<BuffController> registeredCharacters = new List<BuffController>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag))
        {
            Debug.Log(other.name);
            return;
        }


        //    BuffController controller = other.GetComponent<BuffController>();
        //if (!registeredCharacters.Contains(controller))
        //{
        //    registeredCharacters.Add(controller);
        //    Debug.Log($"Registered {other.name} for buff");
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //BuffController controller = other.GetComponent<BuffController>();
        //if (controller != null && registeredCharacters.Contains(controller))
        //{
        //    registeredCharacters.Remove(controller);
        //    Debug.Log($"Removed {other.name} from buff list");
        //}
    }
}
