using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartController : MonoBehaviour
{
    [Header("Tuning")]
    public KartTuning tuning;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (tuning != null)
            rb.centerOfMass = new Vector3(0, tuning.centerOfMassY, 0);
    }

    void FixedUpdate()
    {
        var input = InputManager.Instance;
        if (input == null || tuning == null) return;

        Vector3 velocity = rb.linearVelocity;
        Vector3 forward = transform.forward;

        float speed = velocity.magnitude;
        float maxSpeedMs = tuning.maxSpeed / 3.6f;

        // --------------------
        // ACCELERATION
        // --------------------
        if (Mathf.Abs(input.Throttle) > 0.01f)
        {
            velocity +=
                forward *
                input.Throttle *
                tuning.acceleration *
                Time.fixedDeltaTime;
        }
        else
        {
            velocity = Vector3.MoveTowards(
                velocity,
                Vector3.zero,
                tuning.deceleration * Time.fixedDeltaTime
            );
        }

        velocity = Vector3.ClampMagnitude(velocity, maxSpeedMs);

        // --------------------
        // TURNING
        // --------------------
        if (speed > 0.1f)
        {
            float control = IsGrounded() ? 1f : tuning.airControl;
            float driftBonus = input.Drift ? tuning.driftTurnMultiplier : 1f;

            float turn =
                input.Steering *
                tuning.turnStrength *
                driftBonus *
                control *
                Time.fixedDeltaTime;

            velocity = Quaternion.Euler(0f, turn, 0f) * velocity;

            rb.MoveRotation(
                rb.rotation * Quaternion.Euler(0f, turn, 0f)
            );
        }

        rb.linearVelocity = velocity;

        // --------------------
        // STABILITY
        // --------------------
        rb.AddForce(Vector3.down * tuning.gravityBoost, ForceMode.Acceleration);

        Quaternion upright =
            Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        rb.MoveRotation(
            Quaternion.Slerp(
                rb.rotation,
                upright,
                tuning.uprightStrength * Time.fixedDeltaTime
            )
        );
    }

    bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            0.3f
        );
    }
}
