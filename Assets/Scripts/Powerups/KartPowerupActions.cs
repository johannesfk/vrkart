using UnityEngine;

public class KartPowerupActions : MonoBehaviour
{
    [Header("Boost")]
    public float boostMultiplier = 1.7f;
    public float boostDuration = 1.5f;

    float boostTimer;
    public float CurrentSpeedMultiplier { get; private set; } = 1f;

    [Header("Shield")]
    public Transform powerupVisualRoot;
    public GameObject shieldVisualPrefab;
    public float shieldDuration = 5f;

    GameObject activeShield;
    float shieldTimer;

    [Header("Pulse")]
    public Transform pulseOrigin;
    public float pulseRadius = 5f;
    public float pulseForce = 10f;

    [Header("Pulse VFX")]
    public GameObject pulseVfxPrefab;   // <-- ONLY THIS ONE (one variable)

    // optional: prevents double-firing if input is weird
    float pulseCooldown = 0f;

    void Update()
    {
        // Boost timer
        if (boostTimer > 0f)
        {
            boostTimer -= Time.deltaTime;
            CurrentSpeedMultiplier = boostMultiplier;
        }
        else
        {
            CurrentSpeedMultiplier = 1f;
        }

        // Shield timer
        if (activeShield != null)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0f)
                DisableShield();
        }

        // Pulse cooldown
        if (pulseCooldown > 0f)
            pulseCooldown -= Time.deltaTime;
    }

    public void Boost()
    {
        boostTimer = boostDuration;
    }

    public void Shield()
    {
        if (activeShield != null) return;

        if (powerupVisualRoot == null || shieldVisualPrefab == null)
        {
            Debug.LogWarning("Shield: assign powerupVisualRoot + shieldVisualPrefab in Inspector.");
            return;
        }

        activeShield = Instantiate(shieldVisualPrefab, powerupVisualRoot);
        activeShield.transform.localPosition = Vector3.zero;
        shieldTimer = shieldDuration;

        Debug.Log("Shield ON");
    }

    void DisableShield()
    {
        if (activeShield != null) Destroy(activeShield);
        activeShield = null;
        shieldTimer = 0f;

        Debug.Log("Shield OFF");
    }

    public bool TryBlockHit()
    {
        if (activeShield == null) return false;

        DisableShield(); // consumes shield
        Debug.Log("Shield BLOCKED a hit!");
        return true;
    }

    public void Pulse()
    {
        if (pulseCooldown > 0f) return;
        pulseCooldown = 0.05f;

        Vector3 center = pulseOrigin != null ? pulseOrigin.position : transform.position;

        Debug.Log("PULSE pressed. VFX assigned? " + (pulseVfxPrefab != null));

        if (pulseVfxPrefab != null)
        {
            Instantiate(pulseVfxPrefab, center, Quaternion.identity);
        }

        Collider[] hits = Physics.OverlapSphere(center, pulseRadius);
        foreach (var h in hits)
        {
            var other = h.GetComponentInParent<KartPowerupActions>();
            if (other == null || other == this) continue;

            if (other.TryBlockHit())
                continue;

            Rigidbody otherRb = other.GetComponent<Rigidbody>();
            if (otherRb == null) continue;

            Vector3 dir = (other.transform.position - transform.position);
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.001f) continue;

            otherRb.AddForce(dir.normalized * pulseForce, ForceMode.Impulse);
        }

        Debug.Log("Pulse fired");
    }
}
