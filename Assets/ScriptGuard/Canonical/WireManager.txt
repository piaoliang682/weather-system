using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireManager : MonoBehaviour
{
    public static WireManager Instance;

    [Header("Wire Prefab")]
    [SerializeField] private Wire wirePrefab;

    private Wire activeWire;

    // Keep track of all connected wires
    private List<Wire> allWires = new List<Wire>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Returns the currently active wire being dragged.
    /// </summary>
    public Wire GetActiveWire()
    {
        return activeWire;
    }

    /// <summary>
    /// Spawns a new wire prefab and sets it as active.
    /// </summary>
    public Wire SpawnWire()
    {
        if (wirePrefab == null)
        {
            Debug.LogError("Wire prefab not assigned in WireManager!");
            return null;
        }

        GameObject wireObj = Instantiate(wirePrefab.gameObject);
        activeWire = wireObj.GetComponent<Wire>();

        if (activeWire == null)
        {
            Debug.LogError("Wire prefab does not contain a Wire component!");
            return null;
        }
        RegisterWire(activeWire);
        return activeWire;
    }


    public void DestroyAndUnregisterWire(Wire wire)
    {
        Debug.Log(wire.gameObject.name);
        Destroy(wire.gameObject);

        UnregisterWire(wire);
    }
    /// <summary>
    /// Clears the current active wire when finished connecting.
    /// </summary>
    public void ClearActiveWire()
    {
        activeWire = null;
    }

    /// <summary>
    /// Register a wire when it is fully connected to sockets
    /// </summary>
    public void RegisterWire(Wire wire)
    {
        if (wire == null)
            return;

        if (!allWires.Contains(wire))
            allWires.Add(wire);

        // The wire itself should also register itself to its start/end sockets
        //wire.RegisterToSockets();
    }

    /// <summary>
    /// Optional: remove a wire from the list (for destruction)
    /// </summary>
    public void UnregisterWire(Wire wire)
    {
        if (wire == null)
            return;

        if (allWires.Contains(wire))
            allWires.Remove(wire);
    }
}
