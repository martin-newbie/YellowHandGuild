using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckSceneManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("InGame");
    }
}
