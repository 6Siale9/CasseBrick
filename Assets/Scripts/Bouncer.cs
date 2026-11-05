using System.Collections.Specialized;
using UnityEngine;
public class Bouncer : MonoBehaviour
{
    [SerializeField] private ParentBrick _brick; //If relevent the script of the parent brick
    [SerializeField] private Palet _palet; //If relevent the script of the parent palet
    [SerializeField] private bool _stop = false; //If true : Long rebounce, else fast rebounce
    [SerializeField] private bool _addBounce = false; //If true : Adds bounce for seeking ball upon collision

    public bool Stop { get => _stop; set => _stop = value; }
    public bool AddBounce { get => _addBounce; set => _addBounce = value; }
    public Palet Palet { get => _palet; set => _palet = value; }

    public void Activate()
    {
        if (_brick != null)
        {
            _brick.TakeDamage();
        }
        else if (Palet != null)
        {
            Palet.Bounce();
        }
    }
}