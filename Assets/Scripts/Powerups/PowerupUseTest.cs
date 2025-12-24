using UnityEngine;
using UnityEngine.InputSystem;

public class PowerupUseTest : MonoBehaviour
{
    PowerupInventory inv;

    void Awake()
    {
        inv = GetComponent<PowerupInventory>();
        Debug.Log("PowerupUseTest ready on: " + gameObject.name);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("F pressed");
            inv.Use();
        }
    }
}
