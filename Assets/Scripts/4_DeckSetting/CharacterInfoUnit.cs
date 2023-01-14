using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUnit : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] Image characterImage;
    [SerializeField] Text characterName;

    [Header("Borders")]
    [SerializeField] GameObject selectingBorder;
    [SerializeField] GameObject unchoosableBorder;

    [HideInInspector] public CharacterData linkedData;
    [HideInInspector] public int userCharIndex;
    [HideInInspector] public int unitIndex;

    public void InitInfoUnit(CharacterData data, int index, int _unitIndex)
    {
        linkedData = data;
        userCharIndex = index;
        unitIndex = _unitIndex;

        characterImage.sprite = SpriteManager.GetCharacterUnitSprite(linkedData.keyIndex);
        characterName.text = StaticDataManager.GetCharacterStaticData(linkedData.keyIndex).name;
        DisableAllBorders();
    }

    public void OnChooseButton()
    {
        CharacterInfoWindow.Instance.OnPreviewButton(userCharIndex);
    }

    public void DisableAllBorders()
    {
        selectingBorder.SetActive(false);
        unchoosableBorder.SetActive(false);
    }

    public void OnSelectingBorder()
    {
        selectingBorder.SetActive(true);
    }

    public void OnUnchoosableBorder()
    {
        unchoosableBorder.SetActive(true);
    }
}
