using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TouchType
{
    NONE,
    SELECT,
    DRAG,
}

public class SkillButtonUnit : MonoBehaviour
{
    [HideInInspector] public CharacterGameObject targetObj;
    int curIndex;
    TouchType touchType;

    [Header("UI")]
    public Image skillIcon;

    [Header("Charging")]
    public Image chargingImg;
    public Text chargingTxt;

    public void InitButton(int idx, CharacterGameObject target = null)
    {
        targetObj = target;

        if (targetObj == null)
        {
            gameObject.SetActive(false);
            return;
        }

        curIndex = idx;
        skillIcon.sprite = SpriteManager.GetSkillSprites(targetObj.charIdx);
    }

    private void Update()
    {
        if (targetObj == null)
        {
            return;
        }

        chargingImg.gameObject.SetActive(!targetObj.TargetSkillAble());
        chargingTxt.gameObject.SetActive(!targetObj.TargetSkillAble());

        if (targetObj.TargetSkillAble())
        {
            chargingImg.fillAmount = targetObj.GetTargetSkillGauge();
            chargingTxt.text = string.Format("{0:0}", targetObj.thisAI.curTargetSkillCool);
        }
    }

    public void OnTouch()
    {
        UISkillPanel.Instance.OnButtonDragStart(curIndex);
        touchType = TouchType.SELECT;
    }

    public void OnDragStart()
    {
        UISkillPanel.Instance.OnButtonDragStart(curIndex);
        touchType = TouchType.DRAG;
    }

    public void OnDrag()
    {
        if (touchType != TouchType.DRAG)
            return;


    }

    public void OnDragEnd()
    {

    }
}
