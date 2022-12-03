using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUnit : MonoBehaviour
{
    public int curIndex;

    [Header("UI")]
    public Image skillIcon;

    [Header("Charging")]
    public Image chargingImg;
    public Text chargingTxt;

    public void InitButton(int btnIdx, int charIdx)
    {
        curIndex = btnIdx;
        skillIcon.sprite = SpriteManager.GetSkillSprites(charIdx);
    }

    private void Update()
    {
        try
        {
            var targetObj = InGameManager.GetCharacterObject(curIndex);

            chargingImg.gameObject.SetActive(!targetObj.TargetSkillAble());
            chargingTxt.gameObject.SetActive(!targetObj.TargetSkillAble());

            if (!targetObj.TargetSkillAble())
            {
                chargingImg.fillAmount = targetObj.GetTargetSkillGauge();
                chargingTxt.text = string.Format("{0:0}", targetObj.thisAI.targetSkillCool - targetObj.thisAI.curTargetSkillCool);
            }
        }
        catch (System.Exception)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    bool isTouch;
    public void OnTouch()
    {
        if (isTouch) return;

        isTouch = true;
    }
    public void OnDrag()
    {
        if (isTouch)
        {
            isTouch = false;
            UISkillPanel.Instance.OnButtonDragStart(curIndex, TouchType.DRAG);
        }
    }
    public void OnPointerEnd()
    {
        if (isTouch)
        {
            isTouch = false;

            var unit = UISkillPanel.Instance.curUnit;
            if (unit != null && unit.curIndex == curIndex)
            {
                UISkillPanel.Instance.CancelSkill();
            }
            else UISkillPanel.Instance.OnButtonDragStart(curIndex, TouchType.SELECT);
        }
    }
}
