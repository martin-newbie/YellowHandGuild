using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSettingManager : MonoBehaviour
{

    private static DeckSettingManager instance = null;
    public static DeckSettingManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }


    public List<DeckGroundUnit> units = new List<DeckGroundUnit>();
    public int curSelectingUnit;

    private void Start()
    {
        InitUI();
    }

    public void InitUI()
    {
        for (int i = 0; i < units.Count; i++)
        {
            CharacterData data = null;
            if (TempData.Instance.charIndex[i] != -1)
            {
                data = UserData.Instance.characters[TempData.Instance.charIndex[i]];
            }
            units[i].UnitInit(data, i);
        }
    }

    public void OpenGroundUnitInfo(int groundIndex)
    {

    }
}
