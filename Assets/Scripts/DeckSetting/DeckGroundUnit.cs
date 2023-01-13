using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckGroundUnit : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] Image characterImage;
    [SerializeField] Image posImage;

    CharacterData linkedData;
    public int unitIndex;

    public void UnitInit(CharacterData _data, int index)
    {
        linkedData = _data;
        unitIndex = index;

        activeUI(linkedData != null);
        if (linkedData == null)
        {
            return;
        }

        characterImage.sprite = SpriteManager.GetCharacterUnitSprite(linkedData.keyIndex);
        posImage.sprite = SpriteManager.GetJopSprite(linkedData.posType);

        void activeUI(bool active)
        {
            characterImage.gameObject.SetActive(active);
            posImage.gameObject.SetActive(active);
        }
    }

    public void OnUnitButton()
    {
        DeckSettingManager.Instance.OpenGroundUnitInfo(unitIndex);
    }
}
