using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCrossbowArrow : SkillBase
{
    public float moveSpeed = 10f;
    bool isInit = false;
    StatusData data;
    Vector2 dir;
    AI_Base subject;

    public void ArrowShoot(Vector2 _dir, StatusData _data, AI_Base _subject)
    {
        isInit = true;
        dir = _dir;
        data = _data;
        subject = _subject;
    }

    void Update()
    {
        if (!isInit) return;

        transform.Translate(dir * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayableObject>()?.OnDamage(ERangeType.LONG_DISTANCE_ATK, data, subject);
            Destroy(gameObject);
        }
    }
}
