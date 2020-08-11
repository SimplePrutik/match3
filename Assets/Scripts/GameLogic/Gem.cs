using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gem : MonoBehaviour
{
    private List<Color> gemColors = new List<Color>{Color.red, Color.green, Color.blue, Color.yellow};

    public int curr_col;
    
    public static bool IsDragged = false;
    public static bool LockedInteraction = false;
    public static bool IsRightMove = false;

    private int current_square;
    private Vector3 current_pos;
    private bool special_gem => special.enabled;

    protected GameField game_manager;

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

        curr_col = _color;
        Gem.color = gemColors[_color];
    }

    private Vector3 shift_drag;
    private float mZCoord;
    

    /// <summary>
    /// Initialising position and game manager for checking game logic 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="gf"></param>
    public void Init(int pos, GameField gf)
    {
        game_manager = gf;
        current_square = pos;
        
        current_pos = transform.localPosition;
    }

    private float speed = 2f; 
    
    /// <summary>
    /// Falling of gem and setting its position on field
    /// </summary>
    /// <param name="new_pos"></param>
    public IEnumerator Move(Vector3 new_pos, int pos, bool instantly = true)
    {
        // if (instantly)
        //     transform.localPosition = new_pos;
        current_square = pos;
        current_pos = new_pos;

        var direction = new_pos - transform.localPosition;

        yield return new WaitForSeconds(1f);

        while (Vector3.Distance(transform.localPosition, new_pos) > 0.1f)
        {
            transform.localPosition += direction * (Time.deltaTime * speed);
            yield return null;
        }

        transform.localPosition = new_pos;
    }

    /// <summary>
    /// Winning explosion
    /// </summary>
    public IEnumerator Explode()
    {
        var sc = 1f;
        while (sc > 0f)
        {
            transform.localScale = new Vector3(sc, sc,1);
            sc -= Time.deltaTime * 1.5f;
            yield return null;
        }
        Destroy(gameObject);
    }
    
    //MOUSE INTERACTION

    

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    
    
    private void OnMouseDown()
    {
        var position = gameObject.transform.position;
        mZCoord = Camera.main.WorldToScreenPoint(position).z;
        IsDragged = true;
        GetComponent<BoxCollider>().enabled = false;
        highlight.enabled = false;
        transform.localScale = new Vector3(1.1f, 1.1f, 1);
        shift_drag = position - GetMouseAsWorldPoint() + Vector3.back;
        game_manager.SetPositionOne(current_square);
    }

    private void OnMouseEnter()
    {
        IsRightMove = false;
        highlight.enabled = true;
        highlight.color = Color.white;
        if (IsDragged)
        {
            if (game_manager.IsSwappable(current_square))
            {
                IsRightMove = true;
                game_manager.SetPositionTwo(current_square);
                highlight.color = Color.magenta;
                return;
            }
            highlight.enabled = false;
        }
    }
    
    
    private void OnMouseExit()
    {
        highlight.enabled = false;
    }


    private void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + shift_drag;
    }

    private void OnMouseUp()
    {
        transform.localPosition = current_pos;
        transform.localScale = new Vector3(1, 1, 1);
        GetComponent<BoxCollider>().enabled = true;
        IsDragged = false;
        if (IsRightMove)
            game_manager.StartCoroutine(game_manager.MakeMove());

    }
}