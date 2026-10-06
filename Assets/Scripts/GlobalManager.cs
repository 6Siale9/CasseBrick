using UnityEngine;
using System.Collections.Generic;

public class GlobalManager : MonoBehaviour
{
    #region Instance
    private static GlobalManager _instance;
    public static GlobalManager Instance { get => _instance; set => _instance = value; }

    private void TrySetInstance()
    {
        if (GlobalManager.Instance == null)
        {
            GlobalManager.Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion Instance

    private List<Ball> _balls = new List<Ball>();
    private List<ParentBrick> _bricks = new List<ParentBrick>();
    private List<Spawner> _spawners = new List<Spawner>();
    private int _bounce = 0;
    public List<Ball> Balls { get => _balls; set => _balls = value; }
    public List<ParentBrick> Bricks { get => _bricks; set => _bricks = value; }
    public List<Spawner> Spawners { get => _spawners; set => _spawners = value; }

    private void Awake()
    {
        TrySetInstance();
    }

    public void AddBounce()
    {
        if (_bricks.Count > 0)
        {
        _bounce++;
            if (_bounce > (_balls.Count * 2) + 1)
            {
                _balls[Random.Range(0, _balls.Count)].SeekingBall(_bricks[Random.Range(0, _bricks.Count)]);
                _bounce = 0;
            }
        }
    }

    public void ResetBounce()
    {
        _bounce = 0;
    }

    public void AllSpawnerMove()
    {
        if (_spawners.Count > 0)
        {
            foreach (Spawner spawner in _spawners)  
            {
                spawner.Move();
            }
        }
    }
}
