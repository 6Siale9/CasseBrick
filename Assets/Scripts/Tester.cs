using UnityEngine;

public class Tester : MonoBehaviour
{
    private bool _dispo;
    [SerializeField] private EDirection _slot;

    public bool Dispo { get => _dispo; set => _dispo = value; }
    public EDirection Slot { get => _slot; set => _slot = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
