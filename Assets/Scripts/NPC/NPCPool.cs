using System.Collections.Generic;
using UnityEngine;

public class NPCPool : MonoBehaviour
{
    [SerializeField] private GameObject[] npcPrefabs;
    [SerializeField] private int totalPoolSize = 50;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < totalPoolSize; i++)
        {
            GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

            GameObject npc = Instantiate(prefab);
            npc.SetActive(false);
            pool.Add(npc);
        }
    }


    public GameObject GetNPC()
    {
        foreach (var npc in pool)
        {
            if (!npc.activeInHierarchy)
            {
                npc.SetActive(true);
                return npc;
            }
        }

        return null;
    }
}