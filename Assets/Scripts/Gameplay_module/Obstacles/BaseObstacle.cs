using DG.Tweening;
using UnityEngine;

public abstract class BaseObstacle
{
    public GameObject AssociatedObject { get; private set; }
    public float Size { get; private set; } = 1f;

    protected GameControl Controller;

    public virtual void Spawn(GameControl controller, GameObject go, float size)
    {
        AssociatedObject = go;
        Controller = controller;
        SetSize(size);
    }

    public virtual void ApplyDestroy(float time)
    {
        
    }

    private void SetSize(float newSize)
    {
        Size = newSize;
        AssociatedObject.transform.localScale = Vector3.zero;
        AssociatedObject.transform.DOScale(Vector3.one * Size,0.3f);
    }

    public bool IsActive()
    {
        return AssociatedObject is { activeInHierarchy: true };
    }
}