using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrder : MonoBehaviour
{
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        int order = (int)(-transform.position.y * 100f);
        sprite.sortingOrder = order;
    }
}
