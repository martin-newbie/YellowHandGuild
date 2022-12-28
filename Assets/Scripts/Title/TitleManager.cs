using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null;
        SceneManager.LoadScene("DeckSetting");
    }
}
