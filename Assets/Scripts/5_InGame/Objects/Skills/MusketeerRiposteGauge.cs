using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusketeerRiposteGauge : SkillBase
{
    public GameObject[] gaugeObjs;
    Transform target;

    bool isInit = false;

    public void InitGauge(Transform _target)
    {
        target = _target;
        isInit = true;
    }

    public void SetGaugeCount(int count)
    {
        for (int i = 0; i < gaugeObjs.Length; i++)
        {
            if (i < count) gaugeObjs[i].SetActive(true);
            else gaugeObjs[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!isInit) return;

        transform.position = target.position + new Vector3(0, 2.3f);
    }
}
