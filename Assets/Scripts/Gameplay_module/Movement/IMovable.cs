using UnityEngine;

public interface IMovable
{
    void DeriveControl();
    void Move(Vector2 position);
    void SetSpeed(float speed);
    bool IsMoving();
}