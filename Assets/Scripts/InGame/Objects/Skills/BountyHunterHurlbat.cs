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
    int damage;

    public void Init(Vector2 dir, float speed, int damage)
    {
        isInit = true;
        this.dir = dir;
        this.damage = damage;
        
        moveSpeed = speed;

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
            collision.GetComponent<HostileGameObject>().OnDamage(damage);
/*
            modelTR.rotation = Quaternion.Euler(Vector3.zero);*/

            transform.SetParent(collision.transform);
            isArrive = true;
        }
    }
}
