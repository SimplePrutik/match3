using UnityEngine;

public class ColoredGem : Gem
{

    public ColoredGem(int color)
    {
        var _color = color;
        if (_color < 1 || _color > 4)
        {
            _color = 1;
            Debug.LogWarning("Incorrect color");
        }

        switch (_color)
        {
            case 1:
                GemColor = Color.red;
                break;
            case 2:
                GemColor = Color.blue;
                break;
            case 3:
                GemColor = Color.green;
                break;
            case 4:
                GemColor = Color.yellow;
                break;
        }
    }
}