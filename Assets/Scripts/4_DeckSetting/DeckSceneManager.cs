using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckSceneManager : MonoBehaviour
{
    [Header("UI")]
    public Button gameStartButton;

    private void Start()
    {
        InitButton();
    }

    public void InitButton()
    {
        SpriteManager.SetConfirmSprite(gameStartButton, TempData.Instance.IsGameStartAble());
    }

    public void StartGame()
    {
        if (!TempData.Instance.IsGameStartAble())
        {
            return;
        }

        LoadingManager.LoadScene("InGame");
    }
}
