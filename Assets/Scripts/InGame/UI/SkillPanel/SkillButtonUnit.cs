using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUnit : MonoBehaviour
{
    int curIndex = -1;

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
        if (curIndex == -1) return;

        try
        {
            chargingImg.gameObject.SetActive(!GetTarget().TargetSkillAble());
            chargingTxt.gameObject.SetActive(!GetTarget().TargetSkillAble());

            if (!GetTarget().TargetSkillAble())
            {
                chargingImg.fillAmount = GetTarget().GetTargetSkillGauge();
                chargingTxt.text = string.Format("{0:0}", GetTarget().ai.skillData.targetSkillCool - GetTarget().ai.curTargetSkillCool);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
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

    CharacterGameObject GetTarget() => InGameManager.Instance.GetCharacterObject(curIndex);
}
