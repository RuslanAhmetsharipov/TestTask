using DG.Tweening;
using UnityEngine;

public class SquareObstacle : BaseObstacle
{
    public override void ApplyDestroy(float time)
    {
        AssociatedObject.transform.DOScale(Vector3.zero, time).OnComplete(()=>AssociatedObject.SetActive(false));
    }
}