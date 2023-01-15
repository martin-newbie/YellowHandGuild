using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultistMeteor : SkillBase
{
    Collider2D atkCol;
    AI_Base subject;
    [SerializeField] ContactFilter2D filter;

    StatusData data;

    public void Init(StatusData _data, AI_Base _subject)
    {
        atkCol = GetComponent<Collider2D>();
        data = _data;
        subject = _subject;
    }

    void AttackFrame()
    {
        List<Collider2D> hostiles = new List<Collider2D>();
        Physics2D.OverlapCollider(atkCol, filter, hostiles);

        if (hostiles.Count <= 0) return;

        foreach (var item in hostiles)
        {
            item.GetComponent<HostileGameObject>().OnDamage(ERangeType.LONG_DISTANCE_ATK, data, subject);
        }
    }

    void DestroyFrame()
    {
        Destroy(gameObject);
    }
}
