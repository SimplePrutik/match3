using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameField : Field
{
    private List<int> field;
    private int _rows;
    private int _cols;

    void Awake()
    {
        
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

    public void CreateField(int rows, int cols)
    {
        _rows = rows;
        _cols = cols;
        
    }

    /// <summary>
    /// Checks if there is winning row or column on the field
    /// </summary>
    /// <returns></returns>
    List<bool> isWinnable()
    {
        //Row wins
        var rows = field.Select((x, i) => new {value = x, index = i})
            .GroupBy(x => x.index / (field.Count / this._rows))
            .Select(x => IsWinningLine(x.Select(z => z.value).ToList()))
            .ToList().SelectMany(x => x).ToList();
        
        //Column wins
        var cols = field.Select((x, i) => new {value = x, index = i})
            .GroupBy(x => x.index % _cols)
            .Select(x => IsWinningLine(x.Select(z => z.value).ToList()))
            .ToList();

        var transp_cols = new List<List<bool>>();
        for (int i = 0; i < this._rows; ++i)
            transp_cols.Add(cols.Select(x => x[i]).ToList());
        
        return rows.Zip(transp_cols.SelectMany(x => x).ToList(), (f,s) => f || s).ToList();
    }

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
            
            if (streak == 3)
            {
                bool_line[i - 2] = true;
                bool_line[i - 1] = true;
            }

            bool_line.Add(streak >= 3);

            prev_number = line[i];
        }
        return bool_line;
    }
}