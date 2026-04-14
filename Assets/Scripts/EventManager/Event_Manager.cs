using System;
using UnityEngine;

public class Event_Manager : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private System_PlayerAnxiety AnxietySystem;

    private void Awake()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");
        if (AnxietySystem == null && Player != null)
            AnxietySystem = Player.GetComponent<System_PlayerAnxiety>();
    }

    private void OnEnable()
    {
        if (AnxietySystem != null)
            AnxietySystem.AnxietyLevelChanged += OnAnxietyLevelChanged;
    }

    private void OnDisable()
    {
        if (AnxietySystem != null)
            AnxietySystem.AnxietyLevelChanged -= OnAnxietyLevelChanged;
    }

    private void OnAnxietyLevelChanged(Enum_AnxietyLevel level)
    {
        Debug.Log($"[Event_Manager] Anxiety level: {level}");
    }
}
