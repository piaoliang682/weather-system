using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class InventoryItem : MonoBehaviour
{
    [Header("Item Info")]
    public InventoryItemSO item;
    public int quantity;
    public Transform effectParent;
    [Header("UI Prefab")]
    public GameObject popUpPrefab;

    [Header("Sound Effect")]
    public float soundVolume = 1f;

    private PlayerInventory inventoryManager;
    private AudioSource audioSource;
    public AudioClip clip;

    private void Awake()
    {
    }

    public void Initiate(InventoryItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;

        if (inventoryManager == null)
            inventoryManager = FindObjectOfType<PlayerInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectObject();
        }
    }

    private void CollectObject()
    {
        Debug.Log("Item collected!");
        PlayerInventory.Instance.Add(item, quantity);


        float destroyDelay = 0f;
        PopUp popUpScript = null;

        // ✅ Spawn text
        if (popUpPrefab != null && effectParent != null)
        {
            GameObject collectTextObj = Instantiate(popUpPrefab, effectParent.position, Quaternion.identity);
            audioSource=collectTextObj.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            popUpScript = collectTextObj.GetComponent<PopUp>();
            if (popUpScript != null)
            {
                popUpScript.SetMessage($"得到了 {item.id} x{quantity}!");
            }
        }
        else
        {
            Debug.LogWarning("PopUp prefab or effectParent not assigned!");
        }
        // ✅ Play sound effect
        if (audioSource != null &&clip!=null)
            audioSource.PlayOneShot(clip, soundVolume);

        // ✅ Destroy the item after the longer of sound or pop-up
        float soundLength = (audioSource != null && audioSource.clip != null)
            ? audioSource.clip.length
            : 0f;

        destroyDelay = Mathf.Max(soundLength, destroyDelay);

        Destroy(gameObject, destroyDelay);
    }

}
