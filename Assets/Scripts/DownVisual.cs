using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownVisual : MonoBehaviour
{
    private List<Image> _imgs = new List<Image>();
    private Color _color = new Color(1,1,1,0);

    private void Start()
    {
        Image[] imgs = GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            _imgs.Add(img);
        }
    }

    private void Update()
    {
        ColorLogic();
    }

    private void ColorLogic()
    {
        if (_color.a > 0)
        {
            _color = new Color(1,1,1,Mathf.Clamp(_color.a - (Time.deltaTime * 4.5f), 0, 1));
        }
        foreach (Image img in _imgs)
        {
            img.color = _color;
        }
    }

    public void Activate(Vector3 pos)
    {
        _color = new Color(1,1,1,1);
        transform.position = pos;
        transform.position = new Vector3(transform.position.x, -7f, 0);
    }
}
