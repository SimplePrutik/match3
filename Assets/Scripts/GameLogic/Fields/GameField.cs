using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameField : Field
{
    private List<int> field;
    private List<GameObject> gem_field;
    private int _rows = 7;
    private int _cols = 7;
    private int color_amount = 4;
    private int winning_line = 3;
    private int combo_line = 4;
    private bool gr_down = true;

    private float square_size = 0.703f;

    public GameObject gem_proto;
    
    private List<List<int>> presets = new List<List<int>>
    {
        new List<int>
        {
            2, 2, 1, 2, 3, 0, 3, 
            0, 2, 2, 0, 0, 2, 3, 
            1, 1, 0, 1, 1, 3, 1, 
            3, 1, 3, 0, 1, 3, 0, 
            3, 3, 0, 2, 0, 2, 2, 
            2, 1, 1, 0, 0, 1, 0, 
            3, 1, 1, 3, 3, 1, 1,
        },
        new List<int>
        {
            2, 3, 0, 3, 1, 0, 1, 
            0, 0, 1, 0, 3, 0, 0, 
            1, 2, 0, 2, 0, 1, 3, 
            0, 0, 2, 1, 2, 0, 3, 
            3, 3, 2, 2, 0, 1, 2, 
            1, 3, 1, 0, 1, 2, 1, 
            1, 1, 0, 3, 0, 0, 3,
        }
    };


    private void Awake()
    {
        CreateField();
    }

    private int draggedGem;
    private int draggedToGem;

    public void SetPositionOne(int pos) => draggedGem = pos;
    public void SetPositionTwo(int pos) => draggedToGem = pos;

    public IEnumerator MakeMove()
    {
        SwapGems();
        field = Swap(field, draggedGem, draggedToGem);
        var _field = new List<int>(field);
        if (!gr_down)
            _field.Reverse();
        var win_check = WinnableElems(_field);
        
        while (win_check.Any(x => x != -1))
        {
            for (int i = _field.Count-1; i >= 0; --i)
            {
                var new_index = NewPosition(i, win_check);
                var _gem = gem_field[i].GetComponent<Gem>();
                if (new_index == -1)
                {
                    _field[i] = -1;
                    _gem.StartCoroutine(_gem.Explode());
                    continue;
                }

                if (new_index != i)
                {
                    win_check[new_index] = win_check[i];
                    win_check[i] = 99;
                    _field[new_index] = _field[i];
                    _field[i] = -1;
                    gem_field[new_index] = gem_field[i];
                    gem_field[i] = null;
                    _gem.StartCoroutine(_gem.Move(GetPosition(new_index), new_index, false));
                }
                
            }

            for (int i = _field.Count - 1; i >= 0; --i)
            {
                if (_field[i] == -1)
                {
                    var new_gem = Random.Range(0, color_amount);
                    _field[i] = new_gem;
                    gem_field[i] = Instantiate(gem_proto, transform);
                    var _gem = gem_field[i].GetComponent<Gem>();
                    _gem.SetColor(new_gem);
                    _gem.Init(new_gem, this);
                    gem_field[i].transform.localPosition = GetPosition(i) + Vector3.up * (square_size * 10);
                    _gem.StartCoroutine(_gem.Move(GetPosition(i), i, false));
                }
            }

            var mes = "";
            var gem_mes = "";
            var t_mes = "";
            var t = WinnableElems(_field);
            for (int i = 0; i < 7; ++i)
            {
                mes += "\n";
                gem_mes += "\n";
                t_mes += "\n";
                for (int j = 0; j < 7; ++j)
                {
                    mes += _field[i * 7 + j] + " ";
                    gem_mes += gem_field[i * 7 + j].GetComponent<Gem>().curr_col + " ";
                    t_mes += t[i * 7 + j] + " ";
                }
            }

            Debug.Log(mes);
            Debug.Log(gem_mes);
            Debug.Log(t_mes);

            win_check = WinnableElems(_field);
            yield return new WaitForSeconds(2);
        }
        field = new List<int>(_field);
    }

    int NewPosition(int p, List<int> curField)
    {
        if (curField[p] >= 0 && curField[p] < 100)
            return -1;
        var newPos = p;
        var iter = newPos + _cols;
        while (iter < curField.Count && curField[iter] >= 0 && curField[iter] < 100)
        {
            newPos = iter;
            iter += _cols;
        }
        return newPos;
    }

    
    /// <summary>
    /// Initial spawn
    /// </summary>
    public void CreateField()
    {
        field = GenerateField();
        gem_field = new List<GameObject>();
        for (int i = 0; i < field.Count; ++i)
        {
            var gem = Instantiate(gem_proto, transform);
            gem.transform.localPosition = GetPosition(i);
            var _gem = gem.GetComponent<Gem>();
            _gem.Init(i, this);
            _gem.SetColor(field[i]);
            gem_field.Add(gem);
        }

        gem_proto.transform.localPosition += Vector3.down * 100;
    }

    /// <summary>
    /// Get coordinates for gem movement
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    Vector3 GetPosition(int i) => new Vector3(i % _cols * square_size, -i / _cols * square_size, -2);

    /// <summary>
    /// Initial field generation
    /// </summary>
    /// <returns></returns>
    List<int> GenerateField()
    {
        return presets[0];
        //choosing random preset
        var _arr = new List<int>(presets[Random.Range(0, presets.Count)]);
        //random color shift
        var _shift = Random.Range(0, color_amount);
        for (int i = 0; i < _arr.Count; ++i)
            _arr[i] = (_arr[i] + _shift) % color_amount;
        //reverse
        if (Random.value > 0.5f) _arr.Reverse();
        return _arr;
    }
    
    /// <summary>
    /// Returns array of booleans (true: part of winning lane) of current field 
    /// </summary>
    /// <returns></returns>
    List<int> WinnableElems(List<int> game_field)
    {
        //Row wins
        var rows = game_field.Select((x, i) => new {value = x, index = i})
            .GroupBy(x => x.index / (game_field.Count / this._rows))
            .Select(x => FindStreakLines(x.Select(z => z.value).ToList()))
            .ToList().SelectMany(x => x).ToList();
        
        //Column wins
        var cols = game_field.Select((x, i) => new {value = x, index = i})
            .GroupBy(x => x.index % _cols)
            .Select(x => FindStreakLines(x.Select(z => z.value).ToList()))
            .ToList();

        var transpCols = new List<List<int>>();
        for (int i = 0; i < this._rows; ++i)
            transpCols.Add(cols.Select(x => x[i]).ToList());
        
        return rows.Zip(transpCols.SelectMany(x => x).ToList(), Mathf.Max).ToList();
    }

    /// <summary>
    /// Checks if swapping squares leads to winning combination
    /// </summary>
    /// <param name="g1"></param>
    /// <param name="g2"></param>
    /// <returns></returns>
    public bool IsSwappable(int g2)
    {
        var _arr = Swap(field, draggedGem, g2);
        return IsAbleToSwap(draggedGem,g2) && WinnableElems(_arr).Any(x => x != -1);
    }

    /// <summary>
    /// Returns array with swapped elems
    /// </summary>
    /// <param name="arr"></param>
    /// <param name="i1"></param>
    /// <param name="i2"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    List<T> Swap<T>(List<T> arr, int i1, int i2)
    {
        if (i1 >= arr.Count || i2 >= arr.Count)
        {
            Debug.LogWarning("Index out of range");
            return arr;
        }

        var _arr = new List<T>(arr);
        var _t = _arr[i1];
        _arr[i1] = _arr[i2];
        _arr[i2] = _t;
        return _arr;

    }

    /// <summary>
    /// Swap color of gems
    /// </summary>
    /// <param name="p2"></param>
    void SwapGems()
    {
        var t = field[draggedGem];
        gem_field[draggedGem].GetComponent<Gem>().SetColor(field[draggedToGem]);
        gem_field[draggedToGem].GetComponent<Gem>().SetColor(t);
        Debug.Log("Swapped " + draggedGem + " and " + draggedToGem);
    }
    
    /// <summary>
    /// Checks if they are adjacent squares
    /// </summary>
    /// <param name="g1"></param>
    /// <param name="g2"></param>
    /// <returns></returns>
    bool IsAbleToSwap(int g1, int g2)
    {
        return AbsOne(g1 / _rows, g2 / _rows) && (g1 % _cols == g2 % _cols)
            || AbsOne(g1 % _cols, g2 % _cols) && (g1 / _rows == g2 / _rows);
    }

    /// <summary>
    /// Delta of abs of numbers equal 1
    /// </summary>
    /// <param name="i1"></param>
    /// <param name="i2"></param>
    /// <returns></returns>
    bool AbsOne(int i1, int i2) => Mathf.Abs(i1 - i2) == 1; 

    /// <summary>
    /// Checks line for winning streak of gems
    /// 0-4 => gem_line
    /// -1 => no gem_line
    /// 100-104 => supergem
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    List<int> FindStreakLines(List<int> line)
    {
        var _line = line;
        var streakLine = new List<int>();
        var streak = 1;
        var prevNumber = -1;
        for (int i = 0; i < _line.Count; ++i)
        {
            var streakContinues = prevNumber == _line[i];
            if (!streakContinues && streak >= combo_line)
                streakLine[Random.Range(streakLine.Count - streak, streakLine.Count - 1)] += 100;
            streak = prevNumber == _line[i] ? streak + 1 : 1;
            
            if (streak == winning_line)
            {
                streakLine[i - 2] = _line[i];
                streakLine[i - 1] = _line[i];
            }

            streakLine.Add(streak >= winning_line ? _line[i] : -1);

            prevNumber = line[i];
        }
        return streakLine;
    }
}