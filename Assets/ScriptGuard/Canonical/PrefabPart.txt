using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPart : MonoBehaviour
{
    public PrefabNode socketNode;
    public PrefabNode connectNode;
    // Start is called before the first frame update
    void Start()
    {
        socketNode.AssignParent(this.transform);
        connectNode.AssignParent(this.transform);
    }

    public void PlugTo(GameObject gb)
    {
        connectNode.Connect(gb.GetComponent<PrefabPart>().socketNode);
    }

}
