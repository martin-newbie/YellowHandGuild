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

    public void InitInfoUnit(CharacterData data, int index)
    {
        linkedData = data;
        userCharIndex = index;
    }

    public void DisableAllBorders()
    {

    }

    public void OnSelectingBorder()
    {

    }

    public void OnUnchoosableBorder()
    {

    }
}
