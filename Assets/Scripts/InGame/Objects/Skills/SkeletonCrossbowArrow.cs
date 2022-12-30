using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCrossbowArrow : SkillBase
{
    public float moveSpeed = 10f;
    bool isInit = false;
    Vector2 dir;
    int damage;
    bool isCritical;

    public void ArrowShoot(Vector2 _dir, int _damage, bool _isCritical = false)
    {
        isInit = true;
        dir = _dir;
        damage = _damage;
        isCritical = _isCritical;
    }

    void Update()
    {
        if (!isInit) return;

        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayableObject>()?.OnDamage(damage, EAttackHitType.LONG_DISTANCE_ATK, isCritical);
            Destroy(gameObject);
        }
    }
}
