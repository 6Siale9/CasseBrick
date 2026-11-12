using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int _howMany; //How many bricks to spawn
    [SerializeField] private GameObject[] _toSpawn; //Possible bricks to spawn
    [SerializeField] private Transform _brickParent; //Brick container
    [SerializeField] private int[] _toSpawnWeight; //Chances of each bricks to be spawned
    [SerializeField] private float _spawnTimeThreshold; //Time of pause between each spawn
    private float _spawnTime; //Value
    private bool _canSpawn = true; //Can resume spawning

    #region Method
    void Start()
    {
        Move();
        GlobalManager.Instance.Spawners.Add(this);
    }

    void Update()
    {
        AdvanceLogic();
    }

    private void AdvanceLogic()
    {
        if (_spawnTime < _spawnTimeThreshold && _canSpawn)
        {
            _spawnTime += Time.deltaTime;
        }
        else
        {
            _spawnTime = 0;
            Advance();
        }
    }

    public void Move()
    {
        _canSpawn = true;
    }

    private void Advance()
    {
        EDirection dir = EDirection.NONE;
        dir = CheckForDirections();
        if (dir != EDirection.NONE)
        {
            Spawn();
            Advance(dir);
        }
        else
        {
            Wait();
        }
    }

    private EDirection CheckForDirections() //Multiple small Raycasts to check if cornered
    {
        LayerMask mask = 0;
        bool up = !Physics2D.Raycast(gameObject.transform.position, Vector3.up, 1.4f);
        bool down = !Physics2D.Raycast(gameObject.transform.position, Vector3.down, 1.4f);
        bool right = !Physics2D.Raycast(gameObject.transform.position, Vector3.right, 1.4f);
        bool left = !Physics2D.Raycast(gameObject.transform.position, Vector3.left, 1.4f);
        List<EDirection> list = new List<EDirection>();
        if (up)
        {
            list.Add(EDirection.UP);
        }
        if (down)
        {
            list.Add(EDirection.DOWN);
        }
        if (right)
        {
            list.Add(EDirection.RIGHT);
        }
        if (left)
        {
            list.Add(EDirection.LEFT);
        }
        if (list.Count > 0)
        {
            EDirection returnedvalue = list[Random.Range(0, list.Count)];
            return returnedvalue;
        }
        else
        {
            return EDirection.NONE;
        }
    }

    private void Spawn() // Unsure if correct !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        int totalWeight = 0;
        foreach (int i in _toSpawnWeight)
        {
            totalWeight += i;
        }
        int random = Random.Range(0, totalWeight);
        int slot = 0;
        foreach (int i in _toSpawnWeight)
        {
            if (random < _toSpawnWeight[slot])
            {
                Instantiate(_toSpawn[slot], gameObject.transform.position, Quaternion.identity, _brickParent);
                break;
            }
            else
            {
                random -= _toSpawnWeight[slot];
                slot++;
            }
        }
    }

    private void Advance(EDirection dir)
    {
        switch (dir)
        {
            case EDirection.NONE:
                Debug.Log("Wrong Enum in Spawner at 105");
                break;
            case EDirection.UP:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, 0);
                break;
            case EDirection.DOWN:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, 0);
                break;
            case EDirection.LEFT:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, 0);
                break;
            case EDirection.RIGHT:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, 0);
                break;
        }
        _howMany--;
        if (_howMany > 0)
        {
            Move();
        }
        else
        {
            GlobalManager.Instance.Spawners.Remove(this);
            Destroy(gameObject);
        }
    }

    private void Wait()
    {
        _canSpawn = false;


        // ????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
        GlobalManager.Instance.Spawners.Remove(this);
        Destroy(gameObject);
    }
    #endregion Method
}