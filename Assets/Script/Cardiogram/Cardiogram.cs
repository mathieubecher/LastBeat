using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cardiogram : MonoBehaviour
{

    public AnimationCurve beat;
    public float beatHeight = 5.0f;
    public float beatSpeed = 5.0f;
    
    public Line line1;
    public Line line2;
    public float speed = 10.0f;

    private float _timerBeat = 1.0f;
    private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        line1.ActivateLine( GetScreenLeft());
    }

    // Update is called once per frame
    void Update()
    {
        MoveLine();
    }

    void MoveLine()
    {
        _timerBeat += Time.deltaTime * beatSpeed;
        float yPos = beat.Evaluate(_timerBeat) * beatHeight;
        
        line1.Move(yPos, speed);
        if (line1.transform.position.x > GetScreenRight().x && line1.active)
        {
            line1.DisactivateLine();
            line2.ActivateLine(GetScreenLeft());
        }
        line2.Move(yPos, speed);
        if (line2.transform.position.x > GetScreenRight().x && line2.active)
        {
            line2.DisactivateLine();
            line1.ActivateLine(GetScreenLeft());
        }
    }
    Vector3 GetScreenRight()
    {
        var pos = _camera.ScreenToWorldPoint(new Vector3(Screen.width, .0f, .0f));
        pos.y = 0;
        return pos;
    }
    Vector3 GetScreenLeft()
    {
        var pos = _camera.ScreenToWorldPoint(new Vector3(.0f, .0f, .0f));
        pos.y = 0;
        return pos;
    }

    public void Beat()
    {
        _timerBeat = 0.0f;
    }
}
