using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour
{

    public SpriteRenderer model;
    SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        renderer.sprite = model.sprite;
    }
}
