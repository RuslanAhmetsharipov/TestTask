using UnityEngine;

public class ClicksReceiver : IUpdatable
{
    private IMovable _player;
    private Camera _camera;

    public ClicksReceiver(IMovable player, Camera cam)
    {
        _player = player;
        _camera = cam;
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _player.Move(_camera.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
