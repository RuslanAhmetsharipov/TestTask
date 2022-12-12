using System;

public class ScoreData
{
    public float Distance
    {
        get => _distance;
        set
        {
            _distance = value;
            OnDataChanged?.Invoke();
        }
    }

    private float _distance;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnDataChanged?.Invoke();
        }
    }

    private int _score;

    public Action OnDataChanged;

}