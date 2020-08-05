
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IButton
{
    public Image frame;
    public Color frame_color = Color.cyan;
    public static event Action<string> OnPressed = delegate(string s) {  };
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnPressed(name);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        frame.color = frame_color;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        frame.color = new Color(0,0,0,0);
    }
}