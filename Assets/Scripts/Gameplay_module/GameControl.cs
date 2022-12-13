using System;
using DG.Tweening;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    //Cheat, in real project I prefer using addressable or load from resources or create default 2D plane and get texture by id and adjust size
    public GameObject SquareObstacle => squareObstacle;
    public GameObject CircleObstacle => circleObstacle;
    public Transform ObstaclesContainer => obstaclesContainer;
    
    [SerializeField] private GameSettings settings;
    [SerializeField] private GameObject squareObstacle;
    [SerializeField] private GameObject circleObstacle;
    [SerializeField] private Transform obstaclesContainer;

    public Action<BaseObstacle> OnInterception;

    private ClicksReceiver _clicksReceiver;
    private Updater _updater;
    private IMovable _currentPlayerObject;
    private GameUIController _uiController;
    private ObjectsController _objectsController;
    private Camera _mainCamera;

    private void Start()
    {
        StartGame();
    }

    public ObjectsController GetObjectsController()
    {
        return _objectsController;
    }
    

    public void Intercept(BaseObstacle objectToDestroy)
    {
        OnInterception?.Invoke(objectToDestroy);
    }

    private void StartGame()
    {
        var timeToStart = settings.TimeToStart;
        _mainCamera = Camera.main;
        InitObjectsController();
        InitUpdater();
        InitPlayer();
        InitMovementController();
        InitGameUI();
        
        OnInterception+= (x) => ScoreSaver.ChangeData(_currentPlayerObject.GetCoveredWay(), settings.RewardForSquare);
        DOVirtual.DelayedCall(timeToStart, _currentPlayerObject.DeriveControl);
    }

    private void StopGame()
    {
        _updater.Stop();
        _objectsController.StopGame();
        ScoreSaver.Save();
    }

    private void InitPlayer()
    {
        if (_currentPlayerObject == null)
        {
            var playerObject = new CircleObstacle();
            _currentPlayerObject = _objectsController.Spawn(playerObject) as IMovable;
            _currentPlayerObject?.SetSpeed(settings.PlayerSpeed);
            _updater.AddUpdatable(_currentPlayerObject as IUpdatable);
        }
    }

    private void InitObjectsController()
    {
        _objectsController = new ObjectsController();
        _objectsController.Init(this,settings);
    }

    private void InitMovementController()
    {
        if (_clicksReceiver == null)
        {
            _clicksReceiver = new ClicksReceiver(_currentPlayerObject, _mainCamera);
            _updater.AddUpdatable(_clicksReceiver);
        }
    }

    private void InitUpdater()
    {
        if (_updater == null)
        {
            _updater = new Updater(settings.Fps);
            _updater.Start();
        }
    }

    private void InitGameUI()
    {
        _uiController = FindObjectOfType<GameUIController>();
        _uiController.SetData(ScoreSaver.Load());
        _uiController.StartCounter(settings.TimeToStart);
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