using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gem : MonoBehaviour
{
    private List<Color> gemColors = new List<Color>{Color.red, Color.green, Color.blue, Color.yellow};
    
    
    public static bool IsDragged = false;
    public static bool LockedInteraction = false;

    private int current_square;

    private float spawnY = 1.5f;
    
    [SerializeField]
    protected SpriteRenderer highlight;
    [SerializeField]
    protected SpriteRenderer special;
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

    private Vector3 shift_drag;
    private float mZCoord;

    /// <summary>
    /// Falling of gem and setting its position on field
    /// </summary>
    /// <param name="new_pos"></param>
    public void Move(Vector3 new_pos, int pos)
    {
        current_square = pos;
    }

    /// <summary>
    /// Winning explosion
    /// </summary>
    public void Explode()
    {
        
    }

    

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    
    
    void OnMouseDown()
    {
        var position = gameObject.transform.position;
        mZCoord = Camera.main.WorldToScreenPoint(position).z;
        IsDragged = true;
        GetComponent<BoxCollider>().enabled = false;
        highlight.enabled = false;
        transform.localScale = new Vector3(1.1f, 1.1f, 1);
        shift_drag = position - GetMouseAsWorldPoint() + Vector3.back;
    }

    private void OnMouseEnter()
    {
        highlight.enabled = true;
        if (IsDragged)
        {
            highlight.color = IsDragged ? Color.magenta : Color.white;
        }
    }
    
    
    private void OnMouseExit()
    {
        highlight.enabled = false;
    }


    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + shift_drag;
    }

    private void OnMouseUp()
    {
        transform.localScale = new Vector3(1, 1, 1);
        GetComponent<BoxCollider>().enabled = true;
        IsDragged = false;
    }
}