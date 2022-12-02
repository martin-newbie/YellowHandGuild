using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillPanel : MonoBehaviour
{
    private static UISkillPanel instance = null;
    public static UISkillPanel Instance => instance;
    private void Awake()
    {
        instance = this;
    }

    public SkillButtonUnit unitPrefab;
    public Transform contentsParent;

    public List<SkillButtonUnit> unitList = new List<SkillButtonUnit>();
    SkillButtonUnit curUnit;

    public void InitSkillIcons(List<CharacterGameObject> chars)
    {
        if (chars == null || chars.Count < 0)
        {
            return;
        }

        int idx = 0;
        foreach (var item in chars)
        {
            var tempUnit = Instantiate(unitPrefab, contentsParent);
            tempUnit.InitButton(idx, item);
            idx++;
        }
    }

    public void OnButtonDragStart(int index)
    {
        curUnit = unitList[index];
    }
}
