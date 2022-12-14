using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccultistMeteor : SkillBase
{
    Collider2D atkCol;
    [SerializeField] ContactFilter2D filter;
    Occultist occultist;

    public void Init(Occultist occultist)
    {
        atkCol = GetComponent<Collider2D>();
        this.occultist = occultist;
    }

    void AttackFrame()
    {
        List<Collider2D> hostiles = new List<Collider2D>();
        Physics2D.OverlapCollider(atkCol, filter, hostiles);

        if (hostiles.Count <= 0) return;

        foreach (var item in hostiles)
        {
            item.GetComponent<HostileGameObject>().OnDamage(occultist.GetDamage(), AttackHitType.LONG_DISTANCE_ATK, occultist.IsCritical());
        }
    }

    void DestroyFrame()
    {
        Destroy(gameObject);
    }
}
