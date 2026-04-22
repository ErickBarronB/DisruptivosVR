using System;
using UnityEngine;
using UnityEngine.Events;

public class System_PlayerAnxiety : MonoBehaviour, IAnxietySystem
{
    [Header("Values")]
    [SerializeField] private float anxiety = 70f;
    [SerializeField] private float minAnxiety = 60f; // floor, always Normal
    [SerializeField] private float maxAnxiety = 180f;
    [SerializeField] private bool isIncreasing = false;
    public bool debugsEnabled = false;

    [Header("Rates")]
    [SerializeField] private float increaseRate = 10f;
    [SerializeField] private float decreaseRate = 1.5f;
    [SerializeField] private float increaseRatePerTrigger = 5f;

    [Header("State")]
    [SerializeField] private Enum_AnxietyLevel currentLevel;

    [Header("Events")]
    [SerializeField] private UnityEvent<Enum_AnxietyLevel> onLevelChanged;

    private int AnxietyTriggers = 0;

    public event Action<Enum_AnxietyLevel> AnxietyLevelChanged;
    public event Action<int> AnxietyTriggerAdded;
    public event Action<int> AnxietyTriggerRemoved;
    public event Action<bool> IsIncreasingChanged;

    private void Awake()
    {
        UpdateLevel();
    }

    private void Update()
    {
        TickAnxiety(Time.deltaTime);

        if (debugsEnabled)
        {
            Debug.Log("Anxiety: " + anxiety + " Level: " + currentLevel + " Triggers: " + AnxietyTriggers);
        }
    }

    private void TickAnxiety(float deltaTime)
    {
        if (isIncreasing)
        {
            float dynamicIncrease = increaseRate + (increaseRatePerTrigger * (AnxietyTriggers - 1));
            anxiety += dynamicIncrease * deltaTime;
        }
        else
        {
            float normalized = anxiety / maxAnxiety;
            float dynamicDecrease = decreaseRate * Mathf.Lerp(0.5f, 2f, normalized);
            anxiety -= dynamicDecrease * deltaTime;
        }

        anxiety = Mathf.Clamp(anxiety, minAnxiety, maxAnxiety);
        UpdateLevel();
    }

    private void UpdateLevel()
    {
        Enum_AnxietyLevel newLevel = GetLevelFromValue(anxiety);

        if (newLevel == currentLevel) return;

        currentLevel = newLevel;

        AnxietyLevelChanged?.Invoke(newLevel);
        onLevelChanged?.Invoke(newLevel);
    }

    private Enum_AnxietyLevel GetLevelFromValue(float value)
    {
        if (value >= 160) return Enum_AnxietyLevel.Max;
        if (value >= 120) return Enum_AnxietyLevel.Critical;
        if (value >= 80) return Enum_AnxietyLevel.High;
        return Enum_AnxietyLevel.Normal;
    }

    public bool IsAnxious() => anxiety >= 80f;

    public void CheckAnxietyTriggers()
    {
        SetIsIncreasing(AnxietyTriggers > 0);
    }

    #region Interface Methods

    public void AddAnxiety(float amount)
    {
        anxiety += amount;
        anxiety = Mathf.Clamp(anxiety, minAnxiety, maxAnxiety);
        UpdateLevel();
    }

    public void RemoveAnxiety(float amount)
    {
        anxiety -= amount;
        anxiety = Mathf.Clamp(anxiety, minAnxiety, maxAnxiety);
        UpdateLevel();
    }

    public float GetAnxiety() => anxiety;
    public bool GetIsAnxious() => IsAnxious();
    public Enum_AnxietyLevel GetAnxietyLevel() => currentLevel;
    public bool GetIsIncreasing() => isIncreasing;

    public void SetIsIncreasing(bool NewIncreasing)
    {
        isIncreasing = NewIncreasing;
        IsIncreasingChanged?.Invoke(NewIncreasing);
    }

    public void AddAnxietyTrigger(int TriggerAmounts)
    {
        AnxietyTriggers += TriggerAmounts;
        AnxietyTriggerAdded?.Invoke(TriggerAmounts);
        CheckAnxietyTriggers();
    }

    public void RemoveAnxietyTrigger(int TriggerAmounts)
    {
        AnxietyTriggers = Mathf.Max(0, AnxietyTriggers - TriggerAmounts);
        AnxietyTriggerRemoved?.Invoke(TriggerAmounts);
        CheckAnxietyTriggers();
    }

    #endregion
}