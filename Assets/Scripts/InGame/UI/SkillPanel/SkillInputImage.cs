using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInputImage : MonoBehaviour
{
    public bool isPointerDown;
    public bool isPointerFocus;

    public void OnPointerDown()
    {
        isPointerDown = true;
    }

    public void OnPointerUp()
    {
        isPointerDown = false;
    }

    public void OnPointerEnter()
    {
        isPointerFocus = true;
    }

    public void OnPointerExit()
    {
        isPointerFocus = false;
    }
}
