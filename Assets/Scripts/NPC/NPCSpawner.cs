using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private NPCPool pool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] targets;

    [SerializeField] private float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnNPC), 0f, spawnInterval);
    }

    void SpawnNPC()
    {
        GameObject npc = pool.GetNPC();
        if (npc == null) return;

        int index = Random.Range(0, spawnPoints.Length);

        Transform spawn = spawnPoints[index];

        Transform target = targets[index];

        npc.transform.position = spawn.position;

        npc.GetComponent<NPCMovement>().SetTarget(target.position);
    }
}