using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardiogramController : MonoBehaviour
{

    public AnimationCurve beat;
    public float beatHeight = 5.0f;
    public float beatSpeed = 5.0f;
    
    public Line line1;
    public Line line2;
    public float speed = 10.0f;

    private float m_timerTouch = 1.0f;
    private Camera m_camera;
    
    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
        line1.ActivateLine( GetScreenLeft());
    }

    // Update is called once per frame
    void Update()
    {
        MoveLine();
    }

    void MoveLine()
    {
        m_timerTouch += Time.deltaTime * beatSpeed;
        float yPos = beat.Evaluate(m_timerTouch) * beatHeight;
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
        var pos = m_camera.ScreenToWorldPoint(new Vector3(Screen.width, .0f, .0f));
        Debug.Log(Screen.width);
        pos.y = 0;
        return pos;
    }
    Vector3 GetScreenLeft()
    {
        var pos = m_camera.ScreenToWorldPoint(new Vector3(.0f, .0f, .0f));
        pos.y = 0;
        return pos;
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        m_timerTouch = 0.0f;
    }
}
