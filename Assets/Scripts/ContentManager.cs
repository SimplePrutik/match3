using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    private RectTransform rect
    {
        get { return GetComponent<RectTransform>(); }
    }

    private float game_width = 560f;

    [SerializeField]
    private GameObject game_field;
    [SerializeField]
    private GameObject button_field;

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

    void StartGame()
    {
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.offsetMax = new Vector2(game_width, 0);
        rect.offsetMin = new Vector2(0, 0);
        rect.anchoredPosition = Vector2.zero;
        button_field.SetActive(false);
        game_field.SetActive(true);
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
