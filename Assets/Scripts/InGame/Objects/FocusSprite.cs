using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSprite : MonoBehaviour
{
    SpriteRenderer renderer;
    PlayableObject subject;

    bool isFocus;

    public void StartInit(PlayableObject _subject)
    {
        renderer = GetComponent<SpriteRenderer>();

        subject = _subject;
        SetFocus(false);
    }

    void Update()
    {
        if (!isFocus) return;

        renderer.sprite = subject.model.sprite;
        renderer.flipX = subject.model.flipX;
    }

    public void SetFocus(bool focus)
    {
        gameObject.SetActive(focus);
        isFocus = focus;
    }
}
