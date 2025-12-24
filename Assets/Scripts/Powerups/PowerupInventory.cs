using UnityEngine;

public class PowerupInventory : MonoBehaviour
{
    public PowerupType current = PowerupType.None;

    KartPowerupActions actions;

    void Awake()
    {
        actions = GetComponent<KartPowerupActions>();
    }

    public void Use()
    {
        if (current == PowerupType.None) return;
        if (actions == null)
        {
            Debug.LogError("No KartPowerupActions found on this car.");
            return;
        }

        switch (current)
        {
            case PowerupType.Boost:  actions.Boost();  break;
            case PowerupType.Shield: actions.Shield(); break;
            case PowerupType.Pulse:  actions.Pulse();  break;
        }

        current = PowerupType.None;
    }
}
