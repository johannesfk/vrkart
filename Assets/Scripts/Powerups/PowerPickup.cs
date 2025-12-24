using UnityEngine;

public class PowerPickup : MonoBehaviour
{
    public bool random = true;
    public PowerupType gives = PowerupType.Boost;
    public float respawnTime = 3f;

    Collider col;
    Renderer[] rends;

    void Awake()
    {
        col = GetComponent<Collider>();
        rends = GetComponentsInChildren<Renderer>(true);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PICKUP TRIGGERED by: " + other.name);

        PowerupInventory inv = other.GetComponentInParent<PowerupInventory>();
        if (inv == null)
        {
            Debug.Log("No PowerupInventory found on object entering trigger.");
            return;
        }

        if (inv.current != PowerupType.None)
        {
            Debug.Log("Kart already has a powerup, not overwriting.");
            return;
        }

        inv.current = random ? (PowerupType)Random.Range(1, 4) : gives;

        SetActive(false);
        Invoke(nameof(Respawn), respawnTime);
    }

    void Respawn() => SetActive(true);

    void SetActive(bool active)
    {
        if (col != null) col.enabled = active;
        foreach (var r in rends) if (r != null) r.enabled = active;
    }
}
