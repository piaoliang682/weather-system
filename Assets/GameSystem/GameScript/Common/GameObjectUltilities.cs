using UnityEngine;

public class GameObjectUtilities : MonoBehaviour
{


    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// Destroys all children of this GameObject.
    /// </summary>
    public void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Toggles the active state of this GameObject.
    /// </summary>
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    /// <summary>
    /// Toggles the active state of all child GameObjects.
    /// </summary>
    public void ToggleChildrenActive()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }


    /// <summary>
    /// Sets the active state of all child GameObjects.
    /// </summary>
    /// <param name="isActive">True to activate all children, false to deactivate all children.</param>
    public void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
