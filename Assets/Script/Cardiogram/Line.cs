using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private TrailRenderer _trail;
    private bool _activate = false;
    public bool active{get => _activate;}
    
    private bool _started = false;
    
    void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
    }
    public void ResetLine(Vector3 resetPos)
    {
        transform.position = resetPos;
        _trail.Clear();
    }

    public void ActivateLine(Vector3 resetPos)
    {
        _started = true;
        _activate = true;
        ResetLine(resetPos);
    }

    public void DisactivateLine()
    {
        _activate = false;
    }

    public void Move(float y, float speed)
    {
        if (!_started) return;
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime,y,0.0f);
    }
}
