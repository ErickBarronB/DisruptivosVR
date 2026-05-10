using UnityEngine;

public class AutoDestroyTarget : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float anxietyPenalty = 3f;

    private System_PlayerAnxiety anxietySystem;
    private bool wasDestroyedByPlayer = false;

    private void Start()
    {
        anxietySystem = FindObjectOfType<System_PlayerAnxiety>();
        Destroy(gameObject, lifeTime);
    }

    public void DestroyedByPlayer()
    {
        wasDestroyedByPlayer = true;
    }

    private void OnDestroy()
    {
        if (!wasDestroyedByPlayer && gameObject.scene.isLoaded && anxietySystem != null)
            anxietySystem.AddAnxiety(anxietyPenalty);
    }
}