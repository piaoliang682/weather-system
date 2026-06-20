using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabNode
{
    public enum NodeType
    {
        Socket, // Can accept connections
        Plug,   // Can connect to sockets
        Both    // Can do both
    }

    [Header("Node Info")]
    public string nodeName;

    [Tooltip("The root transform of the prefab this node belongs to")]
    [HideInInspector]
    public Transform nodeParent;

    [Tooltip("Optional transform used to define node position and rotation")]
    public Transform nodeTransform;

    [Tooltip("Type of this node")]
    public NodeType nodeType = NodeType.Both;

    [Tooltip("Currently connected node")]
    public PrefabNode connectedNode;

    /// <summary>
    /// Can this node accept incoming connections?
    /// </summary>
    public bool CanAcceptConnection => nodeType == NodeType.Socket || nodeType == NodeType.Both;

    /// <summary>
    /// Can this node initiate a connection?
    /// </summary>
    public bool CanPlug => nodeType == NodeType.Plug || nodeType == NodeType.Both;

    /// <summary>
    /// Local position relative to parent (read from nodeTransform if assigned)
    /// </summary>
    public Vector3 LocalPosition => nodeTransform != null ? nodeTransform.localPosition : Vector3.zero;

    /// <summary>
    /// Local rotation relative to parent (read from nodeTransform if assigned)
    /// </summary>
    public Quaternion LocalRotation => nodeTransform != null ? nodeTransform.localRotation : Quaternion.identity;

    /// <summary>
    /// Connect this node to another node
    /// </summary>
    public void Connect(PrefabNode targetNode)
    {
        if (targetNode == null) return;
        if (!CanPlug || !targetNode.CanAcceptConnection) return;
        if (connectedNode == targetNode) return;

        AlignToTargetNode(targetNode);
        connectedNode = targetNode;

        // Dual-side connection
        if (nodeType == NodeType.Both && targetNode.nodeType == NodeType.Both)
        {
            targetNode.connectedNode = this;
        }
    }

    /// <summary>
    /// Disconnect this node
    /// </summary>
    public void Disconnect()
    {
        if (connectedNode != null)
        {
            if (nodeType == NodeType.Both && connectedNode.nodeType == NodeType.Both)
                connectedNode.connectedNode = null;

            connectedNode = null;
        }
    }

    /// <summary>
    /// Moves the nodeParent prefab so this node aligns with the target node
    /// </summary>
    private void AlignToTargetNode(PrefabNode targetNode)
    {
        if (nodeParent == null || targetNode.nodeParent == null) return;

        // World position & rotation of target node
        Vector3 targetWorldPos = targetNode.nodeParent.TransformPoint(targetNode.LocalPosition);
        Quaternion targetWorldRot = targetNode.nodeParent.rotation * targetNode.LocalRotation;

        // Offset of this node relative to its parent
        Vector3 nodeOffset = nodeParent.TransformPoint(LocalPosition) - nodeParent.position;

        // Move parent so this node aligns with the target node
        nodeParent.position = targetWorldPos - nodeOffset;
        nodeParent.rotation = targetWorldRot * Quaternion.Inverse(LocalRotation);
    }

    public void AssignParent(Transform parent)
    {
        nodeParent = parent;
    }
}
