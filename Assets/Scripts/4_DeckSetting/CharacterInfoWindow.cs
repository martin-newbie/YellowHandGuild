using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour
{
    private static CharacterInfoWindow instance = null;
    public static CharacterInfoWindow Instance => instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] DeckSceneManager deckManager;

    [Header("Info")]
    public Image infoBackground;
    public Image infoPreviewImage;
    int selectedGroundIndex = -1;
    int curCharIndex = -1;
    int tempIndex = -1;

    [Header("Units")]
    public CharacterInfoUnit unitPrefab;
    public Transform unitParent;
    public List<CharacterInfoUnit> unitList = new List<CharacterInfoUnit>();

    [Header("UI")]
    public Button confirmButton;


    private void Start()
    {
        InitButtonUnits();
    }

    void InitButtonUnits()
    {
        for (int i = 0; i < UserData.Instance.characters.Count; i++)
        {
            var unit = Instantiate(unitPrefab, unitParent);
            unitList.Add(unit);
        }
        unitPrefab.gameObject.SetActive(false);
        InitInfoUnits();
    }

    public void OpenInfoWindow(int groundIndex)
    {
        gameObject.SetActive(true);

        curCharIndex = TempData.Instance.charDeckIndex[groundIndex];
        selectedGroundIndex = groundIndex;

        tempIndex = curCharIndex;
        TempData.Instance.charDeckIndex[groundIndex] = -1;

        InitInfoUnits();
        InitUI();
    }

    public void OnCloseButton()
    {
        TempData.Instance.charDeckIndex[selectedGroundIndex] = tempIndex;
        Close();
    }
    public void OnConfirmButton()
    {
        if (!TempData.Instance.IsDeckAddable())
        {
            return;
        }

        TempData.Instance.charDeckIndex[selectedGroundIndex] = curCharIndex;
        GroundUnitManager.Instance.InitUI();
        Close();
    }
    void Close()
    {
        tempIndex = -1;
        deckManager.InitButton();
        gameObject.SetActive(false);
    }

    public void OnPreviewButton(int charIdx)
    {
        if (IsExistInUnits(charIdx) && charIdx != curCharIndex) return;

        if (curCharIndex != charIdx)
            curCharIndex = charIdx;
        else if (curCharIndex == charIdx)
            curCharIndex = -1;

        InitUI();
    }

    void InitInfoUnits()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            if (i < UserData.Instance.characters.Count)
            {
                unitList[i].InitInfoUnit(i);
            }
        }
    }
    void InitUI()
    {
        InitUnitButton();
        InitConfirmButton();
        if (curCharIndex == -1)
        {
            infoPreviewImage.gameObject.SetActive(false);
            return;
        }

        infoPreviewImage.gameObject.SetActive(true);
        infoPreviewImage.sprite = SpriteManager.GetCharacterUnitSprite(UserData.Instance.characters[curCharIndex].keyIndex);

    }
    void InitUnitButton()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            var unit = unitList[i];

            if (unit.charIndex == curCharIndex)
            {
                unit.OnSelectingBorder();
            }
            else if (IsExistInUnits(unit.charIndex))
            {
                unit.OnUnchoosableBorder();
            }
            else
            {
                unit.DisableAllBorders();
            }
        }
    }
    void InitConfirmButton()
    {
        SpriteManager.SetConfirmSprite(confirmButton, TempData.Instance.IsDeckAddable());
    }

    bool IsExistInUnits(int charIdx)
    {
        return TempData.Instance.charDeckIndex.Contains(charIdx);
    }
}
