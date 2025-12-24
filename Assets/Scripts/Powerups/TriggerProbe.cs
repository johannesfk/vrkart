using UnityEngine;

public class TriggerProbe : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("CAR ENTERED TRIGGER: " + other.name);
    }
}
