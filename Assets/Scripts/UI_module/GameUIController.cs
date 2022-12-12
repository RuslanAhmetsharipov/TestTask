using System.Collections;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text distance;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text timeCounter;
    
    private ScoreData _data;

    public void SetData(ScoreData data)
    {
        _data = data;
        _data.OnDataChanged += UpdateView;
        UpdateView();
    }

    public void StartCounter(float time)
    {
        StartCoroutine(CounterRoutine(time));
    }

    public void UpdateView()
    {
        distance.text = _data.Distance.ToString("F1");
        score.text = _data.Score.ToString();
    }

    private IEnumerator CounterRoutine(float time)
    {
        var passedTime = 0f;
        while (passedTime < time)
        {
            timeCounter.text = $"{(int)(time-passedTime)}";
            yield return new WaitForEndOfFrame();
            passedTime += Time.deltaTime;
        }
        timeCounter.gameObject.SetActive(false);
    }
}