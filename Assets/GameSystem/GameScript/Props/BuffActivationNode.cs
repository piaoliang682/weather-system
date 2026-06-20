using System.Collections.Generic;
using UnityEngine;

public class BuffActivationNode : InteractableBase
{
    [Tooltip("Buff to apply")]
    public string buffName = "FearBuff";

    [Tooltip("Reference to the registrar component")]
    public CharacterRegistrationNode registrar;

    //private BuffSO buff;

    private void Start()
    {
        //buff = GameReference.BuffRegistry.GetBuffByName(buffName);
        if (registrar == null)
            Debug.LogWarning("BuffRegistrar not assigned!");
    }

    public override void HandleInteraction(GameObject interactor)
    {
        //GameReference.AmbienceAudioSource.clip = buff.soundEffect;
        GameReference.AmbienceAudioSource.Play();
        if (registrar == null) 
        { 
            Debug.Log("null character");
            return;
        }
        Debug.Log("interact character");

        //foreach (BuffController controller in registrar.registeredCharacters)
        //{
        //    Debug.Log("Buff applied to registered characters");
        //    foreach (var effect in buff.effects)
        //    {
        //        effect.SetCaster(interactor);
        //    }
        //    controller.ApplyBuff(buff);
        //}

        // Disable interactable after activation
        gameObject.SetActive(false);
    }
}
