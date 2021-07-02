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
}
