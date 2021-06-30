using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private TrailRenderer _trail;
    private bool m_activate = false;
    public bool active{get => m_activate;}
    
    private bool m_started = false;
    
    void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
    }
    public void ResetLine(Vector3 _resetPos)
    {
        transform.position = _resetPos;
        _trail.Clear();
    }

    public void ActivateLine(Vector3 _resetPos)
    {
        m_started = true;
        m_activate = true;
        ResetLine(_resetPos);
    }

    public void DisactivateLine()
    {
        m_activate = false;
    }

    public void Move(float _y, float _speed)
    {
        if (!m_started) return;
        transform.position = new Vector3(transform.position.x + _speed * Time.deltaTime,_y,0.0f);
    }
}
