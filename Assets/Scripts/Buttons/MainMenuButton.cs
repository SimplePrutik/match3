
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MainMenuButton : MonoBehaviour, IButton
{
    public Image frame;
    public Color frame_color = Color.cyan;
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        frame.color = frame_color;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        frame.color = new Color(0,0,0, 0);
    }
}