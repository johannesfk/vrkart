using UnityEngine;

[CreateAssetMenu(
    fileName = "KartTuning",
    menuName = "Kart/Kart Tuning Sheet"
)]
public class KartTuning : ScriptableObject
{
    [Header("Speed")]
    [Tooltip("Maximum speed in km/h")]
    public float maxSpeed = 120f;

    [Tooltip("How fast the kart accelerates")]
    public float acceleration = 30f;

    [Tooltip("How fast the kart slows when not accelerating")]
    public float deceleration = 20f;

    // ------------------------------------

    [Header("Turning")]
    [Tooltip("Base turning strength (degrees/sec)")]
    public float turnStrength = 120f;

    [Tooltip("Extra turning while drifting")]
    public float driftTurnMultiplier = 1.4f;

    [Tooltip("Control while airborne (0 = none, 1 = full)")]
    [Range(0f, 1f)]
    public float airControl = 0.4f;

    // ------------------------------------

    [Header("Stability")]
    [Tooltip("Extra gravity to keep kart glued to ground")]
    public float gravityBoost = 30f;

    [Tooltip("How fast the kart stands upright")]
    public float uprightStrength = 10f;

    [Tooltip("Lower = more stable, heavier feel")]
    public float centerOfMassY = -0.5f;
}
