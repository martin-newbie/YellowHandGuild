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
            chargingImg.gameObject.SetActive(!GetTarget().TargetSkillAble());
            chargingTxt.gameObject.SetActive(!GetTarget().TargetSkillAble());

            if (!GetTarget().TargetSkillAble())
            {
                chargingImg.fillAmount = GetTarget().GetTargetSkillGauge();
                chargingTxt.text = string.Format("{0:0}", GetTarget().thisAI.targetSkillCool - GetTarget().thisAI.curTargetSkillCool);
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
        if (!GetTarget().TargetSkillAble()) return;
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

            if (UISkillPanel.Instance.curCharIndex == curIndex)
            {
                UISkillPanel.Instance.CancelSkill();
            }
            else UISkillPanel.Instance.OnButtonDragStart(curIndex, TouchType.SELECT);
        }
    }

    CharacterGameObject GetTarget() => InGameManager.GetCharacterObject(curIndex);
}
