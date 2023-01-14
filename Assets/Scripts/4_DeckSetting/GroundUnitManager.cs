using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundUnitManager : MonoBehaviour
{

    private static GroundUnitManager instance = null;
    public static GroundUnitManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }


    public List<DeckGroundUnit> units = new List<DeckGroundUnit>();
    public int curSelectingUnit;

    [Header("Info")]
    public CharacterInfoWindow infoWindow;

    private void Start()
    {
        InitUI();
    }

    public void InitUI()
    {
        for (int i = 0; i < units.Count; i++)
        {
            CharacterData data = null;
            if (TempData.Instance.charDeckIndex[i] != -1)
            {
                data = UserData.Instance.characters[TempData.Instance.charDeckIndex[i]];
            }
            units[i].UnitInit(data, i);
        }
    }

    public void OpenGroundUnitInfo(int groundIndex)
    {
        infoWindow.OpenInfoWindow(groundIndex);
    }
}
