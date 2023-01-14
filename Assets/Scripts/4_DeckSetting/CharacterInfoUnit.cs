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

    CharacterData linkedData;
    public int charIndex;

    public void InitInfoUnit(int index)
    {
        linkedData = UserData.Instance.characters[index];
        charIndex = index;

        characterImage.sprite = SpriteManager.GetCharacterUnitSprite(linkedData.keyIndex);
        characterName.text = StaticDataManager.GetCharacterStaticData(linkedData.keyIndex).name;
        DisableAllBorders();
    }

    public void OnChooseButton()
    {
        CharacterInfoWindow.Instance.OnPreviewButton(charIndex);
    }

    public void DisableAllBorders()
    {
        selectingBorder.SetActive(false);
        unchoosableBorder.SetActive(false);
    }

    public void OnSelectingBorder()
    {
        selectingBorder.SetActive(true);
        unchoosableBorder.SetActive(false);
    }

    public void OnUnchoosableBorder()
    {
        unchoosableBorder.SetActive(true);
        selectingBorder.SetActive(false);
    }
}
