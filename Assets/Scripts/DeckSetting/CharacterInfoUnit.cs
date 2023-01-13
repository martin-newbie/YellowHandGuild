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

    public void InitInfoUnit(CharacterData data)
    {
        linkedData = data;
    }
}
