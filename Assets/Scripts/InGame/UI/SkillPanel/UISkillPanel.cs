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
    [HideInInspector] public SkillButtonUnit curUnit;

    public void InitSkillIcons(List<CharacterGameObject> chars)
    {
        if (chars == null || chars.Count < 0)
        {
            return;
        }

        int btnIdx = 0;
        foreach (var item in chars)
        {
            var tempUnit = Instantiate(unitPrefab, contentsParent);
            tempUnit.InitButton(btnIdx, item.charIdx);
            btnIdx++;
        }
        unitPrefab.gameObject.SetActive(false);
    }

    Coroutine setSkillCor;
    public void OnButtonDragStart(int index, TouchType type)
    {
        CancelSkill();
        curUnit = unitList[index];
        touchType = type;

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
        touchType = TouchType.NONE;
        curUnit = null;
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
        // select nearest

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

            if(!inputImage.isPointerDown && Input.GetMouseButtonUp(0))
            {
                break;
            }
            yield return null;
        }

        EndSkill();
        yield break;
    }
    void SearchNearest()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void SelectNearest(HostileGameObject target)
    {
        touchType = TouchType.NONE;

        if (target == null)
        {
            // print message
            return;
        }

        InGameManager.GetCharacterObject(curUnit.curIndex).thisAI.SetTargetingSkillTarget(target);
    }
}
