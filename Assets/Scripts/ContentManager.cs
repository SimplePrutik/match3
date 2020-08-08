using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    private RectTransform rect
    {
        get { return GetComponent<RectTransform>(); }
    }

    private float game_width = 760f;


    
    [SerializeField]
    private List<Field> fields;

    void ButtonHandler(string button_name)
    {
        switch (button_name)
        {
            case "new_game":
                StartGame();
                break;
            case "exit":
                Application.Quit();
                break;
        }
    }

    GameObject GetField(string field)
    {
        return fields.Find(x => x.field_name == field)?.gameObject;
    }

    /// <summary>
    /// Transition to game scene
    /// </summary>
    void StartGame()
    {
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.offsetMax = new Vector2(game_width, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.anchoredPosition = Vector2.zero;
        GetField("menu_field").SetActive(false);
        GetField("game_field").SetActive(true);
        GetField("game_field").GetComponent<GameField>().CreateField();
    }
    
    
    private void OnEnable()
    {
        MainMenuButton.OnPressed += ButtonHandler;
    }

    private void OnDisable()
    {
        MainMenuButton.OnPressed -= ButtonHandler;
    }
}
