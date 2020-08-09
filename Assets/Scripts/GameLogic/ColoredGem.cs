using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColoredGem : MonoBehaviour
{
    private List<Color> gemColors = new List<Color>{Color.red, Color.green, Color.blue, Color.yellow};
    public static float idle_z = -2f;
    public static bool IsDragged = false;
    
    [SerializeField]
    protected SpriteRenderer highlight;
    public void SetColor(int color)
    {
        var Gem = GetComponent<SpriteRenderer>();
        var _color = color;
        if (_color < 0 || _color > 3)
        {
            _color = 1;
            Debug.LogWarning("Incorrect color");
        }

        Gem.color = gemColors[_color];
    }

    private bool drag_block = false;
    private Vector3 shift_drag;
    private float mZCoord;

    



    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    
    
    void OnMouseDown()
    {
        var position = gameObject.transform.position;
        mZCoord = Camera.main.WorldToScreenPoint(
            position).z;
        IsDragged = true;
        drag_block = true;
        highlight.enabled = false;
        transform.localScale = new Vector3(1.1f, 1.1f, 1);
        shift_drag = position - GetMouseAsWorldPoint() + Vector3.back;
    }

    private void OnMouseEnter()
    {
        if (drag_block)
            return;
        highlight.enabled = true;
        highlight.color = IsDragged ? Color.magenta : Color.white;
    }
    
    
    private void OnMouseExit()
    {
        highlight.enabled = false;
    }


    void OnMouseDrag()
    {
        highlight.enabled = false;
        transform.position = GetMouseAsWorldPoint() + shift_drag;
    }

    private void OnMouseUp()
    {
        transform.localScale = new Vector3(1, 1, 1);
        drag_block = false;
        IsDragged = false;
    }
}