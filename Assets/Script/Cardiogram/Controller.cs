using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Cardiogram _cardiogram;
    [SerializeField] private List<float> _beatHistory;
    private int _maxSize = 30;

    // Start is called before the first frame update
    void Start()
    {
        _cardiogram = GetComponent<Cardiogram>();
        _beatHistory = new List<float>();
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        _cardiogram.Beat();
        _beatHistory.Add(Time.time);
        if(_beatHistory.Count > _maxSize)
            _beatHistory.Remove(_beatHistory.First());

    }

    
        public float Frequency(float duration)
        {
            if (duration == 0) return 0.0f;
            int count = 0;
            float time = Time.time;
            while (_beatHistory.Count > count && _beatHistory[_beatHistory.Count - count - 1] >= time -duration)
            {
                ++count;
            }
            Debug.Log(count / duration);
            return count / duration;
        }
}
