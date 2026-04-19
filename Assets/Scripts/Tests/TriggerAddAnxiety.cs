using UnityEngine;

public class TriggerAddAnxiety : MonoBehaviour
{
    [SerializeField] private System_PlayerAnxiety anxiety;
    [SerializeField] private float amount = 20f;
    private void OnTriggerEnter(Collider other)
    {
        anxiety.AddAnxiety(amount);
    }
}
