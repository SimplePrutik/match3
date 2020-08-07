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

    private float game_width = 560f;


    
    [SerializeField]
    private List<Field> fields;

    void ButtonHandler(string button_name)
    {
        switch (button_name)
        {
            case "start_game":
                StartGame();
                break;
            case "new_game":
                GameConfig();
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
    /// Transition to config menu
    /// </summary>
    void GameConfig()
    {
        GetField("menu_field")?.SetActive(false);
        GetField("config_field")?.SetActive(true);
    }
    
    /// <summary>
    /// Start of the game after setting up config
    /// </summary>
    void StartGame()
    {
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.offsetMax = new Vector2(game_width, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.anchoredPosition = Vector2.zero;
        var cfg_field = GetField("config_field");
        var gm_field = GetField("game_field");
        if (cfg_field != null && gm_field != null)
        {
            cfg_field.SetActive(false);
            gm_field.SetActive(true);
            // cfg_field.GetComponent<>()
        }
        
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
