
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewGameButton : MainMenuButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

}