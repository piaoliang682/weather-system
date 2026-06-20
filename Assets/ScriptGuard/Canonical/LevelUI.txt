using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
public class LevelUI : MonoBehaviour
{
    [Header("UI References")]
    public Image previewImage;
    public Button selectButton; 
    public Button enterButton;
    public TMP_Text levelNameText;
    public TMP_Text requirementText;
    public GameObject lockedOverlay;
    public List<GameObject> selectedHighlight = new List<GameObject>();

    private LevelItemSO data;
    private LevelSelectionPanel panel;
    private bool isLocked;

    public void Setup(LevelItemSO levelData, LevelSelectionPanel parent, bool isLocked)
    {
        data = levelData;
        panel = parent;
        this.isLocked = isLocked;
        if (levelNameText) levelNameText.text = data.levelName;
        if (requirementText) requirementText.text = data.requirementText;
        if (previewImage) previewImage.sprite = data.previewImage;

        if (lockedOverlay) lockedOverlay.SetActive(isLocked);

        SetSelected(false);
    }

    public Button GetSelectButton()
    {
        return selectButton;
    }

    public Button GetEnterButton()
    {
        return enterButton;
    }
    public void SetSelected(bool selected)
    {
        if (selectedHighlight!=null)
        {
            foreach(var gb in selectedHighlight)
            {
            gb.SetActive(selected);
            }
        }

    }
    public void OnClick()
    {
        if (isLocked)
        {
            Debug.Log($"Level {data.levelName} is locked.");
            return;
        }

        panel?.OnLevelSelected(this);
    }


    public LevelItemSO GetData() => data;
}
