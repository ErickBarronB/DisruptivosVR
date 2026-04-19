using System;
using UnityEngine;
using UnityEngine.Events;

public class System_PlayerAnxiety : MonoBehaviour, IAnxietySystem
{
    [Header("Values")]
    [SerializeField] private float anxiety = 70f;
    [SerializeField] private float minAnxiety = 0f;
    [SerializeField] private float maxAnxiety = 180f;

    private float currentFloor = 0f;

    [Header("Rates")]
    [SerializeField] private float increaseRate = 10f;
    [SerializeField] private float decreaseRate = 1.5f;

    [Header("State")]
    [SerializeField] private Enum_AnxietyLevel currentLevel;

    [Header("Events")]
    [SerializeField] private UnityEvent<Enum_AnxietyLevel> onLevelChanged; // desde el inspector podemos poner que queremos que pase cuando cambia la ansiedad
    public event Action<Enum_AnxietyLevel> AnxietyLevelChanged;

    private void Awake()
    {
        UpdateLevel();
    }

    private void Update()
    {
        TickAnxiety(Time.deltaTime);
    }

    private void TickAnxiety(float deltaTime)
    {
        float normalized = anxiety / maxAnxiety;

        float dynamicDecrease = decreaseRate * Mathf.Lerp(0.5f, 2f,normalized);

        anxiety -= dynamicDecrease * deltaTime;

        anxiety = Mathf.Clamp(anxiety, currentFloor, maxAnxiety);

        UpdateLevel();
    }

    private void UpdateLevel()
    {
        Enum_AnxietyLevel newLevel = GetLevelFromValue(anxiety);

        if (newLevel == currentLevel) return;

        currentLevel = newLevel;

        UpdateFloor();

        AnxietyLevelChanged?.Invoke(newLevel);
        onLevelChanged?.Invoke(newLevel);
    }

    private void UpdateFloor()
    {
        switch (currentLevel)
        {
            case Enum_AnxietyLevel.High:
                currentFloor = 80f;
                break;
            case Enum_AnxietyLevel.Critical:
                currentFloor = 120f;
                break;
            case Enum_AnxietyLevel.Max:
                currentFloor = 160f;
                break;
            case Enum_AnxietyLevel.Normal:
                currentFloor = 0f;
                break;
        }
    } 

    private Enum_AnxietyLevel GetLevelFromValue(float value)
    {
        if (value >= 160) return Enum_AnxietyLevel.Max;
        if (value >= 120) return Enum_AnxietyLevel.Critical;
        if (value >= 80) return Enum_AnxietyLevel.High;
        return Enum_AnxietyLevel.Normal;
    }

    public bool IsAnxious()
    {
        return anxiety >= 80f;
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

    #endregion
}
