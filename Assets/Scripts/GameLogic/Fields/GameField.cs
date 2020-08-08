using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField : Field
{
    private List<int> field;
    private int _rows;
    private int _cols;
    private int color_amount = 4;
    private int winning_line = 3;


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
        _rows = GameConfiguration.Config().GetConfig<int>("height_value");
        _cols = GameConfiguration.Config().GetConfig<int>("width_value");
        field = GenerateField();

    }

    /// <summary>
    /// Initial field generation
    /// </summary>
    /// <returns></returns>
    List<int> GenerateField()
    {
        var _arr = new List<int>();
        var _win = false;
        var field_size = _rows * _cols;
        while (!_win && Time.realtimeSinceStartup < 60)
        {
            _arr = new List<int>();
            for (int i = 0; i < field_size; ++i)
                _arr.Add(Random.Range(0, color_amount));
            for (int i = 0; i < field_size; ++i)
                for (int j = 0; j < field_size; ++j)
                    if (WinnableElems(Swap(_arr, i, j)).Any(x => x)
                    && !WinnableElems(_arr).Any(x => x))
                        _win = true;
        }

        string message = "";
        for (int i = 0; i < _rows; ++i)
        {
            for (int j = 0; j < _cols; ++j)
                message += _arr[i * _rows + j] + " ";
            message += "\n";
        }
        Debug.Log(message);
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