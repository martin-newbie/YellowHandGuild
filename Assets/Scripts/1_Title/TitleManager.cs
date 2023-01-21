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

            loadingText.text = $"게임 데이터 받아오는 중 ({cur.ToString() + "/" + total.ToString()})";
            loadingImg.fillAmount = (float)cur / (float)total;

            return total <= cur;
        });


        UserData.Instance.characters.Add(new CharacterData(14));
        UserData.Instance.characters.Add(new CharacterData(15));
        UserData.Instance.characters.Add(new CharacterData(16));
        UserData.Instance.characters.Add(new CharacterData(17));
        UserData.Instance.characters.Add(new CharacterData(13));
        UserData.Instance.characters.Add(new CharacterData(20));
        SceneManager.LoadScene("StageSelect");
    }
}
