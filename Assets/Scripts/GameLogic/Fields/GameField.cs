using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameField : Field
{
    private List<int> field;
    private int _rows = 7;
    private int _cols = 7;
    private int color_amount = 4;
    private int winning_line = 3;

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

    /// <summary>
    /// Changes color of gem
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="color"></param>
    public void Change(int pos, int color)
    {
        if (field.Count <= pos || pos < 0)
        {
            Debug.LogWarning("Incorrect change field values");
            return;
        }

        field[pos] = color;
    }

    /// <summary>
    /// Initial spawn
    /// </summary>
    public void CreateField()
    {
        field = GenerateField();
        for (int i = 0; i < field.Count; ++i)
        {
            var gem = Instantiate(gem_proto, transform);
            gem.transform.localPosition = new Vector3(i % _cols * square_size, -i / _cols * square_size, ColoredGem.idle_z);
            gem.GetComponent<ColoredGem>().SetColor(field[i]);
        }
        Destroy(gem_proto);
    }

    /// <summary>
    /// Initial field generation
    /// </summary>
    /// <returns></returns>
    List<int> GenerateField()
    {
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
    List<bool> WinnableElems(List<int> game_field)
    {
        //Row wins
        var rows = game_field.Select((x, i) => new {value = x, index = i})
            .GroupBy(x => x.index / (game_field.Count / this._rows))
            .Select(x => IsWinningLine(x.Select(z => z.value).ToList()))
            .ToList().SelectMany(x => x).ToList();
        
        //Column wins
        var cols = game_field.Select((x, i) => new {value = x, index = i})
            .GroupBy(x => x.index % _cols)
            .Select(x => IsWinningLine(x.Select(z => z.value).ToList()))
            .ToList();

        var transp_cols = new List<List<bool>>();
        for (int i = 0; i < this._rows; ++i)
            transp_cols.Add(cols.Select(x => x[i]).ToList());
        
        return rows.Zip(transp_cols.SelectMany(x => x).ToList(), (f,s) => f || s).ToList();
    }

    /// <summary>
    /// Checks if swapping squares leads to winning combination
    /// </summary>
    /// <param name="g1"></param>
    /// <param name="g2"></param>
    /// <returns></returns>
    bool IsSwappable(int g1, int g2)
    {
        var _arr = Swap(field, g1, g2);
        return IsAbleToSwap(g1,g2) && WinnableElems(_arr).Any(x => x);
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
    /// Checks if there is winning line in a row or column
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    List<bool> IsWinningLine(List<int> line)
    {
        var _line = line;
        var bool_line = new List<bool>();
        var streak = 1;
        var prev_number = -1;
        for (int i = 0; i < _line.Count; ++i)
        {
            streak = prev_number == _line[i] ? streak + 1 : 1;
            
            if (streak == winning_line)
            {
                bool_line[i - 2] = true;
                bool_line[i - 1] = true;
            }

            bool_line.Add(streak >= winning_line);

            prev_number = line[i];
        }
        return bool_line;
    }
}