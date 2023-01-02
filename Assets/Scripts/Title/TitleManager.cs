using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public Text loadingText;
    public Image loadingImg;

    IEnumerator Start()
    {
        StartCoroutine(StaticDataManager.Instance.LoadAllStatics());
        yield return new WaitUntil(() =>
        {
            int total = StaticDataManager.Instance.totalDatasCount;
            int cur = StaticDataManager.Instance.currentLoadDataCount;

            loadingText.text = $"���� ������ �޾ƿ��� �� ({cur.ToString() + "/" + total.ToString()})";
            loadingImg.fillAmount = (float)cur / (float)total;

            return total <= cur;
        });

        SceneManager.LoadScene("InGame");
    }
}
