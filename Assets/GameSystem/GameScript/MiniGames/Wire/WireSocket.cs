using System.Collections.Generic;
using UnityEngine;
public class WireSocket : MonoBehaviour
{
    // Keep track of wires connected to this socket
    private List<Wire> connectedWires = new List<Wire>();
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("socket clicked");
            Wire activeWire = WireManager.Instance.GetActiveWire();
            if (activeWire == null)
            {
                activeWire=WireManager.Instance.SpawnWire();
            }
        

            activeWire.ConnectToSocket(this);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Socket right-clicked: destroying connected wires");
            DestroyAllConnectedWires();
        }



    }
    public void RegisterWire(Wire wire)
    {
        if (!connectedWires.Contains(wire))
            connectedWires.Add(wire);
    }

    public void DestroySocketWire(Wire wire)
    {

            WireManager.Instance.DestroyAndUnregisterWire(wire);

        if (connectedWires.Contains(wire))
            connectedWires.Remove(wire);
    }

    /// <summary>
    /// Destroy all wires connected to this socket
    /// </summary>
    private void DestroyAllConnectedWires()
    {
        foreach (Wire wire in connectedWires)
        {
            WireManager.Instance.DestroyAndUnregisterWire(wire);
        }
        connectedWires.Clear();
    }
}
