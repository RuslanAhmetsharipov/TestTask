using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/LevelSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public int maxCountObstacles = 5;
    public float timeBetweenSpawns = 0.5f;
    public float timeToDestroyObstacle= 0.5f;
    public float playerSpeed = 5f;
    public int timeToStart = 3;
    public int fps = 60;
    public int rewardForSquare = 1;
}
