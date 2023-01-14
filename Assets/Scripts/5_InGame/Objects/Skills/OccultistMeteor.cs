using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultistMeteor : SkillBase
{
    Collider2D atkCol;
    [SerializeField] ContactFilter2D filter;

    StatusData data;

    public void Init(StatusData _data)
    {
        atkCol = GetComponent<Collider2D>();
        data = _data;
    }

    void AttackFrame()
    {
        List<Collider2D> hostiles = new List<Collider2D>();
        Physics2D.OverlapCollider(atkCol, filter, hostiles);

        if (hostiles.Count <= 0) return;

        foreach (var item in hostiles)
        {
            item.GetComponent<HostileGameObject>().OnDamage(data.dmg, EAttackHitType.LONG_DISTANCE_ATK, data.hitRate, data.cri, data.criDmg, data.defBreak);
        }
    }

    void DestroyFrame()
    {
        Destroy(gameObject);
    }
}
