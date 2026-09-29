using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int _howMany;
    [SerializeField] private GameObject[] _toSpawn;
    [SerializeField] private int[] _toSpawnWeight;
    [SerializeField] private Tester[] _testers;
    private EDirection _blockedDir = EDirection.None;


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
        EDirection dir = EDirection.None;
        dir = CheckForDirections();
        if (dir != EDirection.None)
        {
            Spawn();
            ResetTesters();
            Advance(dir);
        }
        else
        {
            Wait();
        }
    }

    private EDirection CheckForDirections()
    {
        List<Tester> goodTesters = new List<Tester>(_testers);
        foreach (Tester tester in _testers)
        {
            if (tester.Dispo)
            {
                if (tester.Slot == _blockedDir)
                {
                    goodTesters.Remove(tester);
                }
            }
            else
            {
                goodTesters.Remove(tester);
            }
        }
        if (goodTesters.Count > 0)
        {
            int i = Random.Range(0, goodTesters.Count);
            return goodTesters[i].Slot;
        }
        else
        {
            return EDirection.None;
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
                Instantiate(_toSpawn[slot]);
            }
            else
            {
                slot++;
            }
        }
    }

    private void ResetTesters()
    {
        foreach (Tester tester in _testers)
        {
            tester.Dispo = true;
        }
    }

    private void Advance(EDirection dir)
    {
        switch (dir)
        {
            case EDirection.None:
                Debug.Log("Wrong Enum in Spawner at 105");
                break;
            case EDirection.Up:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, 0);
                break;
            case EDirection.Down:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, 0);
                break;
            case EDirection.Left:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, 0);
                break;
            case EDirection.Right:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, 0);
                break;
        }
        _howMany--;
        if (_howMany > 0)
        {
            Move();
        }
    }

    private void Wait()
    {

    }
}

public enum EDirection
{
    Up,
    Down,
    Left,
    Right,
    None
}