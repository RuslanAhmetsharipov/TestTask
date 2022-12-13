using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

internal class CircleObstacle : BaseObstacle, IMovable, IUpdatable
{
    private bool _isControllReceived = false;
    private float _speed = 1f;
    private bool _isMoving;
    private Vector2 _destination = Vector2.zero;
    private Vector2 _lastIntersectPosition = Vector2.zero;
    private float _minMoveDistance = 1f;

    private List<Vector2> _path = new();
    private Tween _movingTween;

    public void DeriveControl()
    {
        _isControllReceived = true;
    }

    public void Move(Vector2 pos)
    {
        if (_isControllReceived && Vector2.Distance(pos, _destination) >= _minMoveDistance)
        {
            AddPointToPath(pos);
        }
    }

    private void AddPointToPath(Vector2 pos)
    {
        _path.Add(pos);
        _destination = pos;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    public override void ApplyDestroy(float time)
    {
        _isControllReceived = false;
    }

    public float GetCoveredWay()
    {
        return Vector2.Distance(AssociatedObject.transform.position, _lastIntersectPosition);
    }

    private void CheckInterception()
    {
        foreach (var obstacle in Controller.GetObjectsController().ActiveObstacles)
        {
            if (CheckInterceptionWithObject(obstacle))
            {
                Controller.Intercept(obstacle);
                _lastIntersectPosition = AssociatedObject.transform.position;
                return;
            }
        }
    }

    private bool CheckInterceptionWithObject(BaseObstacle obstacle)
    {
        if (Vector2.Distance(AssociatedObject.transform.position,
                obstacle.AssociatedObject.transform.position) >=
            Size + obstacle.Size)
            return false;
        var associatedTransformPosition = AssociatedObject.transform.position;
        var obstaclePosition = obstacle.AssociatedObject.transform.position;
        var obstacleSize = obstacle.Size;

        var x = Math.Max(obstaclePosition.x - obstacleSize / 2f,
            Math.Min(associatedTransformPosition.x, obstaclePosition.x + obstacleSize / 2f));
        var y = Math.Max(obstaclePosition.y - obstacleSize / 2f,
            Math.Min(associatedTransformPosition.y, obstaclePosition.y + obstacleSize / 2f));

        var distance = Math.Sqrt((x - associatedTransformPosition.x) * (x - associatedTransformPosition.x) +
                                 (y - associatedTransformPosition.y) * (y - associatedTransformPosition.y));

        return distance < Size;
    }

    private void SetupNextPoint()
    {
        if (_path.Count > 0)
        {
            var nextPoint = _path[0];
            var time = Vector2.Distance(AssociatedObject.transform.position, nextPoint) / _speed;
            Ease choosenEase;
            if (_isMoving)
            {
                choosenEase = Ease.Linear;
            }
            else
            {
                choosenEase = Ease.InSine;
            }
            AssociatedObject.transform.DOMove(nextPoint, time)
                .OnComplete(() =>
                {
                    _path.Remove(nextPoint);
                    SetupNextPoint();
                })
                .SetEase(choosenEase);
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }

    public void Update()
    {
        CheckInterception();
        if (!IsMoving())
        {
            SetupNextPoint();
        }
    }
}