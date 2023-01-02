using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TouchType
{
    NONE,
    SELECT,
    DRAG,
}

public class UISkillPanel : MonoBehaviour
{
    private static UISkillPanel instance = null;
    public static UISkillPanel Instance => instance;
    private void Awake()
    {
        instance = this;
    }

    public TouchType touchType;

    public SkillButtonUnit unitPrefab;
    public Transform contentsParent;
    public SkillInputImage inputImage;

    public List<SkillButtonUnit> unitList = new List<SkillButtonUnit>();
    [HideInInspector] public int curCharIndex = -1;

    bool isInit = false;

    public void InitSkillIcons(List<CharacterGameObject> chars)
    {
        if (chars == null || chars.Count <= 0)
        {
            return;
        }

        if (!isInit)
        {
            for (int i = 0; i < chars.Count; i++)
            {
                var tempUnit = Instantiate(unitPrefab, contentsParent);
                unitList.Add(tempUnit);
            }
            isInit = true;
        }

        for (int i = 0; i < chars.Count; i++)
        {
            unitList[i].InitButton(i, chars[i].charIdx);
        }


        unitPrefab.gameObject.SetActive(false);
    }

    Coroutine setSkillCor;
    public void OnButtonDragStart(int index, TouchType type)
    {
        CancelSkill();
        curCharIndex = index;
        touchType = type;

        InGameManager.Instance.TargetingCanvasObj.SetActive(true);
        switch (touchType)
        {
            case TouchType.SELECT:
                setSkillCor = StartCoroutine(SelectSkillCor());
                break;
            case TouchType.DRAG:
                setSkillCor = StartCoroutine(DragSkillCor());
                break;
        }
    }
    public void CancelSkill()
    {
        EndSkill();
        if (setSkillCor != null) StopCoroutine(setSkillCor);
    }
    void EndSkill()
    {
        InGameManager.Instance.OffTargeting();
        touchType = TouchType.NONE;
    }

    IEnumerator DragSkillCor()
    {
        yield return null;

        while (true)
        {
            SearchNearest();

            if (inputImage.isPointerFocus && Input.GetMouseButtonUp(0))
            {
                break;
            }
            else if (!inputImage.isPointerFocus && Input.GetMouseButtonUp(0))
            {
                CancelSkill();
                yield break;
            }
            yield return null;
        }
        SelectNearest();

        EndSkill();
        yield break;
    }
    IEnumerator SelectSkillCor()
    {
        yield return null;


        yield return new WaitUntil(() => inputImage.isPointerDown && Input.GetMouseButtonDown(0));

        while (true)
        {
            SearchNearest();

            if (!inputImage.isPointerFocus && Input.GetMouseButtonUp(0))
            {
                break;
            }
            if (inputImage.isPointerFocus && Input.GetMouseButtonUp(0))
            {
                SelectNearest();
                break;
            }
            yield return null;
        }

        EndSkill();
        yield break;
    }
    void SearchNearest()
    {
        InGameManager.Instance.SearchNearestTargetCharacter(curCharIndex);
    }
    void SelectNearest()
    {
        touchType = TouchType.NONE;
        InGameManager.Instance.SelectNearestTargetCharacter(curCharIndex);
    }
}
