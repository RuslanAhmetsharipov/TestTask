using System.Collections.Generic;
using DG.Tweening;

public class Updater
{
    public List<IUpdatable> Updatables { get; } = new();

    private readonly float _timeToUpdate = 0f;
    private Tween _updateLoop;

    public Updater(int fps)
    {
        _timeToUpdate = 1f / fps;
    }

    public void Start()
    {
        _updateLoop = DOVirtual.DelayedCall(_timeToUpdate, Update).SetLoops(-1);
    }

    public void Update()
    {
        foreach (var updatable in Updatables)
        {
            updatable.Update();
        }
    }

    public void Stop()
    {
        _updateLoop.Kill();
    }

    public void AddUpdatable(IUpdatable updatable)
    {
        Updatables.Add(updatable);
    }
}