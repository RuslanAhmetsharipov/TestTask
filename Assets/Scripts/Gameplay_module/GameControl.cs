using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameControl : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameSettings settings;
    [SerializeField] private Transform obstaclesContainer;
    [SerializeField] private GameObject squareObstacle;
    [SerializeField] private GameObject circleObstacle;

    private List<GameObject> SpawnedObstacles { get; } = new();
    public List<BaseObstacle> ActiveObstacles { get; } = new();

    private ClicksReceiver _clicksReceiver;
    private Updater _updater;
    private IMovable _currentPlayerObject;
    private GameUIController _uiController;
    private Tween _mainSpawningLoop;

    private void Start()
    {
        StartGame();
    }

    public void Intercept(BaseObstacle objectToDestroy, float distance)
    {
        ScoreSaver.ChangeData(distance, settings.rewardForSquare);
        DestroyObstacle(objectToDestroy);
    }

    private void StartGame()
    {
        var timeToStart = settings.timeToStart;
        InitUpdater();
        InitPlayer();
        InitObstacles();
        InitMovementController();
        InitGameUI();
        DOVirtual.DelayedCall(timeToStart, SetupMainSpawnLoop);
        DOVirtual.DelayedCall(timeToStart, _currentPlayerObject.DeriveControl);
    }

    private void StopGame()
    {
        _updater.Stop();
        foreach (var obstacle in SpawnedObstacles)
        {
            Destroy(obstacle);
        }

        ScoreSaver.Save();
        _mainSpawningLoop?.Kill();
    }

    private void SetupMainSpawnLoop()
    {
        _mainSpawningLoop = DOVirtual.DelayedCall(settings.timeBetweenSpawns, () =>
        {
            if (CheckCanSpawn())
            {
                Activate(new SquareObstacle());
            }
        }).SetLoops(-1);
    }

    private void InitPlayer()
    {
        if (_currentPlayerObject == null)
        {
            var playerObject = new CircleObstacle();
            _currentPlayerObject = Spawn(playerObject) as IMovable;
            _currentPlayerObject?.SetSpeed(settings.playerSpeed);
            _updater.AddUpdatable(_currentPlayerObject as IUpdatable);
        }
    }

    private void InitObstacles()
    {
        while (SpawnedObstacles.Count < settings.maxCountObstacles)
        {
            Spawn(new SquareObstacle());
        }
    }

    private void InitMovementController()
    {
        if (_clicksReceiver == null)
        {
            _clicksReceiver = new ClicksReceiver(_currentPlayerObject, mainCamera);
            _updater.AddUpdatable(_clicksReceiver);
        }
    }

    private void InitUpdater()
    {
        if (_updater == null)
        {
            _updater = new Updater(settings.fps);
            _updater.Start();
        }
    }

    private void InitGameUI()
    {
        _uiController = FindObjectOfType<GameUIController>();
        _uiController.SetData(ScoreSaver.Load());
        _uiController.StartCounter(settings.timeToStart);
    }

    private bool CheckCanSpawn()
    {
        return SpawnedObstacles.Count(x => x.activeInHierarchy) < settings.maxCountObstacles;
    }

    private BaseObstacle Spawn(BaseObstacle item)
    {
        GameObject go = null;
        switch (item)
        {
            case CircleObstacle:
                go = Instantiate(circleObstacle, Vector2.zero, Quaternion.identity);
                break;
            case SquareObstacle:

                go = Instantiate(squareObstacle, GetRandomPositionOnScreen(), Quaternion.identity, obstaclesContainer);
                SpawnedObstacles.Add(go);
                go.SetActive(false);
                break;
            default:
                Debug.LogError("Type of creation for object has not defined");
                break;
        }

        item.Spawn(this, go, 1f);
        return item;
    }

    private Vector2 GetRandomPositionOnScreen()
    {
        var spawnY = Random.Range
        (mainCamera.ScreenToWorldPoint(new Vector2(0, 0)).y,
            mainCamera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        var spawnX = Random.Range
        (mainCamera.ScreenToWorldPoint(new Vector2(0, 0)).x,
            mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        return new Vector2(spawnX, spawnY);
    }

    private void Activate(BaseObstacle item)
    {
        var firstFree = SpawnedObstacles.First(x => !x.activeInHierarchy);
        item.Spawn(this, firstFree, Random.Range(0.5f, 2.0f));
        firstFree.transform.position = GetRandomPositionOnScreen();
        firstFree.SetActive(true);
        ActiveObstacles.Add(item);
    }

    private void DestroyObstacle(BaseObstacle item)
    {
        ActiveObstacles.Remove(item);
        item.ApplyDestroy(settings.timeToDestroyObstacle);
    }

    private void OnDestroy()
    {
        StopGame();
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    private void OnApplicationQuit()
    {
        ScoreSaver.Save();
    }
}