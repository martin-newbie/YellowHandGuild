using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerBullet : SkillBase
{
    float speed = 45f;

    SpriteRenderer sprite;
    Vector2 dir;
    StatusData data;
    CharacterAI subject;

    bool isEnd;

    public void BulletInit(Vector2 _dir, StatusData _data, CharacterAI _subject)
    {
        sprite = GetComponent<SpriteRenderer>();

        dir = _dir;
        data = _data;
        subject = _subject;
    }

    private void Update()
    {
        if (isEnd) return;

        transform.Translate(dir * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hostile"))
        {
            collision.gameObject.GetComponent<PlayableObject>().OnDamage(ERangeType.LONG_DISTANCE_ATK, data, subject);
            StartCoroutine(DestroyAction());
        }
    }

    IEnumerator DestroyAction()
    {
        isEnd = true;
        sprite.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
