using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int _howMany;
    [SerializeField] private GameObject[] _toSpawn;
    [SerializeField] private int[] _toSpawnWeight;
    private EDirection _blockedDir = EDirection.NONE;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Move()
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

    private EDirection CheckForDirections()
    {
        LayerMask mask = 0;
        bool up = !Physics2D.Raycast(gameObject.transform.position, Vector3.up, 1.4f);
        bool down = !Physics2D.Raycast(gameObject.transform.position, Vector3.down, 1.4f);
        bool right = !Physics2D.Raycast(gameObject.transform.position, Vector3.right, 1.4f);
        bool left = !Physics2D.Raycast(gameObject.transform.position, Vector3.left, 1.4f);
        Debug.Log(up + " " + down + " " + right + " " + left);
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
            Debug.Log(returnedvalue.ToString());
            return returnedvalue;
        }
        else
        {
            return EDirection.NONE;
        }
    }

    private void Spawn()
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
                Instantiate(_toSpawn[slot], gameObject.transform.position, Quaternion.identity);
            }
            else
            {
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
            Destroy(gameObject);
        }
    }

    private void Wait()
    {

    }
}

public enum EDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
}