using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCrossbowArrow : SkillBase
{
    public float moveSpeed = 10f;
    bool isInit = false;
    StatusData data;
    Vector2 dir;

    public void ArrowShoot(Vector2 _dir, StatusData _data)
    {
        isInit = true;
        dir = _dir;
        data = _data;
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
            collision.GetComponent<PlayableObject>()?.OnDamage(data.dmg, EAttackHitType.LONG_DISTANCE_ATK, data.hitRate, data.cri, data.criDmg, data.defBreak);
            Destroy(gameObject);
        }
    }
}
