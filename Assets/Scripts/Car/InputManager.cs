using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public float Throttle;
    public float Steering;
    public bool Brake;
    public bool Drift;

    CarInputActions actions;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        actions = new CarInputActions();
    }

    void OnEnable()
    {
        actions.Enable();
    }

    void OnDisable()
    {
        actions.Disable();
    }

    void Update()
    {
        Throttle = actions.Car.Throttle.ReadValue<float>();
        Steering = actions.Car.Steering.ReadValue<float>();
        Brake = actions.Car.Brake.IsPressed();
        Drift = actions.Car.Drift.IsPressed();
    }
}
