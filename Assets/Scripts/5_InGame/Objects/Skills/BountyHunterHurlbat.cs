using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyHunterHurlbat : SkillBase
{
    public Transform modelTR;
    public float rotateSpeed;
    public bool isArrive;

    bool isInit;

    Vector2 dir;
    float moveSpeed;
    StatusData data;

    public void Init(Vector2 _dir, float _speed, StatusData _data)
    {
        isInit = true;
        dir = _dir;
        moveSpeed = _speed;
        data = _data;
    }

    private void Update()
    {
        if (!isInit) return;

        if (!isArrive)
        {
            modelTR.Rotate(Vector3.back * Time.deltaTime * rotateSpeed);
            transform.Translate(dir * Time.deltaTime * -moveSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hostile"))
        {
            collision.GetComponent<HostileGameObject>().OnDamage(data.dmg, EAttackHitType.LONG_DISTANCE_ATK, data.hitRate, data.cri, data.criDmg, data.defBreak);
            InGameManager.Instance.PlayEffect(0, collision.transform.position + new Vector3(0, 1));
            isArrive = true;
            Destroy(gameObject);
        }
    }
}
