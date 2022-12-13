using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ObjectsController
{
    private Camera _mainCamera;
    
    public List<BaseObstacle> ActiveObstacles { get; } = new();
    
    private List<GameObject> SpawnedObstacles { get; } = new();
    
    private GameSettings _settings;
    private GameControl _gameControl;
    private IFactory _factory; 
    
    private Tween _mainSpawningLoop;

    public void Init(GameControl gameControl, GameSettings settings)
    {
        _settings = settings;
        _gameControl = gameControl;
        _mainCamera = Camera.main;
        _gameControl.OnInterception += DestroyObstacle;
        _factory = new CreationFactory(gameControl, _mainCamera);
        
        InitObstacles();
        
        DOVirtual.DelayedCall(_settings.TimeToStart, SetupMainSpawnLoop);
    }

    public void StopGame()
    {
        _mainSpawningLoop?.Kill();
        
        foreach (var obstacle in SpawnedObstacles)
        {
            GameObject.Destroy(obstacle);
        }
    }
    
    public BaseObstacle Spawn(BaseObstacle item)
    {
        GameObject go = null;
        switch (item)
        {
            case CircleObstacle:
                go = _factory.Spawn(item);
                break;
            case SquareObstacle:
                go = _factory.Spawn(item);
                SpawnedObstacles.Add(go);
                break;
            default:
                Debug.LogError("Type of creation for object has not defined");
                break;
        }

        item.Spawn(_gameControl, go, 1f);
        return item;
    }
    
    private void InitObstacles()
    {
        while (SpawnedObstacles.Count < _settings.MaxCountObstacles)
        {
            Spawn(new SquareObstacle());
        }
    }
    
    private void SetupMainSpawnLoop()
    {
        _mainSpawningLoop = DOVirtual.DelayedCall(_settings.TimeBetweenSpawns, () =>
        {
            if (CheckCanSpawn())
            {
                Activate(new SquareObstacle());
            }
        }).SetLoops(-1);
    }
        
    private bool CheckCanSpawn()
    {
        return SpawnedObstacles.Count(x => x.activeInHierarchy) < _settings.MaxCountObstacles;
    }

    private void Activate(BaseObstacle item)
    {
        var firstFree = SpawnedObstacles.First(x => !x.activeInHierarchy);
        item.Spawn(_gameControl, firstFree, Random.Range(0.5f, 2.0f));
        firstFree.transform.position = GetRandomPositionOnScreen();
        firstFree.SetActive(true);
        ActiveObstacles.Add(item);
    }

    private Vector2 GetRandomPositionOnScreen()
    {
        var spawnY = Random.Range
        (_mainCamera.ScreenToWorldPoint(new Vector2(0, 0)).y,
            _mainCamera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
        var spawnX = Random.Range
        (_mainCamera.ScreenToWorldPoint(new Vector2(0, 0)).x,
            _mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

        return new Vector2(spawnX, spawnY);
    }
    
    
    private void DestroyObstacle(BaseObstacle item)
    {
        ActiveObstacles.Remove(item);
        item.ApplyDestroy(_settings.TimeToDestroyObstacle);
    }
    
}