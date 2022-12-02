using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUnit : MonoBehaviour
{
    [HideInInspector] public CharacterGameObject targetObj;
    int curIndex;

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
