using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/LevelSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    public int MaxCountObstacles => maxCountObstacles;
    public float TimeBetweenSpawns => timeBetweenSpawns;
    public float TimeToDestroyObstacle => timeToDestroyObstacle;
    public float PlayerSpeed => playerSpeed;
    public int TimeToStart => timeToStart;
    public int Fps => fps;
    public int RewardForSquare => rewardForSquare;
    
    [SerializeField] private int maxCountObstacles = 5;
    [SerializeField] private float timeBetweenSpawns = 0.5f;
    [SerializeField] private float timeToDestroyObstacle= 0.5f;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private int timeToStart = 3;
    [SerializeField] private int fps = 60;
    [SerializeField] private int rewardForSquare = 1;
}
