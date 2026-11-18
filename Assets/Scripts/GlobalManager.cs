using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

    private Ball _ball; //Current active ball
    private List<ParentBrick> _bricks = new List<ParentBrick>(); //Current active bricks
    private List<Spawner> _spawners = new List<Spawner>(); //Current active Spawners (only relevent if Spawners don't explode when cornered
    private float _flipThreshold;
    private float _flipValue;
    private int _flipCount;
    
    public Ball Ball { get => _ball; set => _ball = value; }
    public List<ParentBrick> Bricks { get => _bricks; set => _bricks = value; }
    public List<Spawner> Spawners { get => _spawners; set => _spawners = value; }

    private void Awake()
    {
        TrySetInstance();
    }

    private void Update()
    {
        FlipLogic();
    }

    private void FlipLogic()
    {
        if (_flipValue < _flipThreshold)
        {
            _flipValue += Time.deltaTime;
        }
        else
        {
            _flipValue = 0;
            _flipThreshold = Random.Range(0f, 0.75f);
            _flipCount = 1;//_bricks.Count / 10;
            Flip();
        }
    }

    private void Flip()
    {
        for (int i = 0; i < _flipCount; i++)
        {
            int n = Random.Range(0, _bricks.Count);
            if (n != 0)
            {
                _bricks[n].Img.rectTransform.up = new Vector3(1, 1, 0);
            }
        }
    }

    public void AllSpawnerMove()
    {
        if (_bricks.Count <= 0 && _spawners.Count <= 0)
        {
            EndGame();
        }
        if (_spawners.Count > 0)
        {
            foreach (Spawner spawner in _spawners)  
            {
                spawner.Move();
            }
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0;
    }
}
