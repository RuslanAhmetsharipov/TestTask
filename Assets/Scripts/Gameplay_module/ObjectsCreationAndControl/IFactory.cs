using UnityEngine;

public interface IFactory
{
        GameObject Spawn<T>(T type);
}