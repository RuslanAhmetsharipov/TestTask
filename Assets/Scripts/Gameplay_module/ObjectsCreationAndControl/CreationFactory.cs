using UnityEngine;

public class CreationFactory: IFactory
{
    private GameControl _gameControl;
    private Camera _mainCamera;

    //Only because of Cheat in GameControl
    public CreationFactory(GameControl gameControl, Camera cam)
    {
        _gameControl = gameControl;
        _mainCamera = cam;
    }

    public GameObject Spawn<T>(T item)
    {
        GameObject go = null;
        switch (item)
        {
            case CircleObstacle:
                go = GameObject.Instantiate(_gameControl.CircleObstacle, Vector2.zero, Quaternion.identity);
                break;
            case SquareObstacle:
                go = GameObject.Instantiate(_gameControl.SquareObstacle, GetRandomPositionOnScreen(), Quaternion.identity, _gameControl.ObstaclesContainer);
                go.SetActive(false);
                break;
            default:
                Debug.LogError("Type of creation for object has not defined");
                break;
        }
        return go;
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
}