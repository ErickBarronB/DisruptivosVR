using UnityEngine;

public class MinigameSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius = 1.5f;
    [SerializeField] private float spawnInterval = 1f;

    [SerializeField] private int maxTargetsToSpawn = 15;

    private float timer;
    private bool minigameActive = false;
    private int spawnedTargets = 0;

    private void Update()
    {
        if (!minigameActive) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawnedTargets < maxTargetsToSpawn)
        {
            SpawnTarget();
            timer = 0;
        }

        if (spawnedTargets >= maxTargetsToSpawn && GameObject.FindGameObjectsWithTag("MinigameTarget").Length == 0)
            StopMinigame();
    }

    public void StartMinigame()
    {
        minigameActive = true;
        timer = 0;
        spawnedTargets = 0;
    }

    public void StopMinigame()
    {
        minigameActive = false;
    }

    private void SpawnTarget()
    {
        Vector3 randomPos = player.position + Random.onUnitSphere * spawnRadius;

        randomPos.y = Mathf.Clamp(randomPos.y, player.position.y - 0.3f, player.position.y + 0.8f);

        Instantiate(targetPrefab, randomPos, Quaternion.identity);
        spawnedTargets++;
    }
}