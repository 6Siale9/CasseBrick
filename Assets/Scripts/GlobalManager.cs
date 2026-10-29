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

    private Ball _ball;
    private List<ParentBrick> _bricks = new List<ParentBrick>();
    private List<Spawner> _spawners = new List<Spawner>();
    public Ball Ball { get => _ball; set => _ball = value; }
    public List<ParentBrick> Bricks { get => _bricks; set => _bricks = value; }
    public List<Spawner> Spawners { get => _spawners; set => _spawners = value; }

    private void Awake()
    {
        TrySetInstance();
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
