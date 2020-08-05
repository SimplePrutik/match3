
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitGameButton : MainMenuButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }

}