using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour
{
    [Header("Info")]
    public Image infoBackground;
    public Image infoPreviewImage;

    [Header("Units")]
    public CharacterInfoUnit unitPrefab;
    public Transform unitParent;
    public List<CharacterInfoUnit> unitList = new List<CharacterInfoUnit>();

    private void Start()
    {
        InitButtonUnits();
    }

    void InitButtonUnits()
    {
        for (int i = 0; i < UserData.Instance.characters.Count; i++)
        {
            var unit = Instantiate(unitPrefab, unitParent);
            unit.InitInfoUnit(UserData.Instance.characters[i]);

            unitList.Add(unit);
        }
    }

    public void OpenInfoWindow()
    {
        gameObject.SetActive(true);
    }

    public void OnCloseButton()
    {

    }

    public void OnConfirmButton()
    {

    }

}
