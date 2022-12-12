using System;
using UnityEngine;

public class ScoreSaver
{
    private const string DISTANCE_KEY = "distance";
    private const string SCORE_KEY = "key";

    private static ScoreData _data;
    
    public static void Save()
    {
        PlayerPrefs.SetFloat(DISTANCE_KEY, _data.Distance);
        PlayerPrefs.SetInt(SCORE_KEY, _data.Score);
        PlayerPrefs.Save();
    }
    
    public static ScoreData Load()
    {
        if(_data == null)
        {
            _data = new ScoreData()
            {
                Distance = PlayerPrefs.GetFloat(DISTANCE_KEY, 0f),
                Score = PlayerPrefs.GetInt(SCORE_KEY, 0)
            };
        }
        else
        {
            _data.Distance = PlayerPrefs.GetFloat(DISTANCE_KEY, 0f);
            _data.Score = PlayerPrefs.GetInt(SCORE_KEY, 0);
        }
        return _data;
    }

    public static bool DataExist()
    {
        return PlayerPrefs.HasKey(DISTANCE_KEY);
    }

    public static void ChangeData(float dist,int score)
    {
        _data.Distance += dist;
        _data.Score += score;
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}
