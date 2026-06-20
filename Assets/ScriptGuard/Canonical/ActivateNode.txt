using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ActivateNode : InteractableBase
{
    public UnityEvent onActivate;
    public bool isUseTag;
    public string targetTag;
    public enum ActivationAction
    {
        Enable,
        Disable,
        Instantiate,
        Destroy
    }
    [Header("Action Settings")]
    [SerializeField] private ActivationAction action;

    [Header("Objects")]
    [SerializeField] private List<GameObject> targets;

    protected override void OnConfirmed()
    {
        base.OnConfirmed();

        foreach (GameObject obj in targets)
        {
            if (obj == null) continue;

            switch (action)
            {
                case ActivationAction.Enable:
                    obj.SetActive(true);
                    break;

                case ActivationAction.Disable:
                    obj.SetActive(false);
                    break;

                case ActivationAction.Instantiate:
                    Instantiate(obj, obj.transform.position, obj.transform.rotation);
                    break;

                case ActivationAction.Destroy:
                    Destroy(obj);
                    break;
            }
        }
        onActivate?.Invoke();
    }

    protected override void OnReject()
    {
        base.OnReject();
        // Optional: cancel logic
    }


    protected override void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
       if (isUseTag)
            {
                if (other.CompareTag(targetTag))
                {


                //targets.Add(other.gameObject);
                base.OnTriggerEnter(other);

                }
       }

        else
        {
            base.OnTriggerEnter(other);
        }




    }
}
