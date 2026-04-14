using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class System_PlayerAnxiety : MonoBehaviour, IAnxietySystem
{


    [Header("Anxiety Level")]
    [SerializeField] private Enum_AnxietyLevel anxietyLevel;
    [SerializeField, Min(0f)] private float anxietyIncreaseRate = 1f;
    [SerializeField] private bool IsAnxious;

    [Header("Events")]
    [SerializeField] private UnityEvent<Enum_AnxietyLevel> onAnxietyLevelChanged;

    public event Action<Enum_AnxietyLevel> AnxietyLevelChanged;

    private float anxiety;
    private readonly float anxietyMax = (float)Enum_AnxietyLevel.Max;
    private readonly float anxietyMin = (float)Enum_AnxietyLevel.Normal;

    
    void Awake() {
        anxiety = (float)anxietyLevel;
    }

    void Start() {
        StartCoroutine(AnxietyDrainRoutine());
    }


    private IEnumerator AnxietyDrainRoutine()
    {
        var tick = new WaitForSeconds(0.1f);
        while (true) {
            if (IsAnxious) {
                anxiety = Mathf.Clamp(anxiety + anxietyIncreaseRate * 0.1f, anxietyMin, anxietyMax);
                SyncAnxietyLevelFromValue();
            }
            else {
                anxiety = Mathf.Clamp(anxiety - anxietyIncreaseRate * 0.1f, anxietyMin, anxietyMax);
                SyncAnxietyLevelFromValue();
            }
            yield return tick;
        }
    }

    #region Anxiety Methods

    public void SetAnxietyIncreaseRate(float rate)
    {
        anxietyIncreaseRate = Mathf.Max(0f, rate);
    }

    private void SyncAnxietyLevelFromValue()
    {
        Enum_AnxietyLevel computed = ComputeLevelFromAnxiety(anxiety);
        if (computed == anxietyLevel)
            return;

        anxietyLevel = computed;
        RaiseAnxietyLevelChanged(computed);
    }

    private static Enum_AnxietyLevel ComputeLevelFromAnxiety(float value)
    {
        Enum_AnxietyLevel highest = Enum_AnxietyLevel.Normal;
        float highestThreshold = float.NegativeInfinity;

        foreach (Enum_AnxietyLevel level in Enum.GetValues(typeof(Enum_AnxietyLevel)))
        {
            float threshold = (float)(int)level;
            if (value >= threshold && threshold > highestThreshold)
            {
                highestThreshold = threshold;
                highest = level;
            }
        }

        return highest;
    }

    private void RaiseAnxietyLevelChanged(Enum_AnxietyLevel newLevel)
    {
        AnxietyLevelChanged?.Invoke(newLevel);
        onAnxietyLevelChanged?.Invoke(newLevel);
    }

    #endregion

    #region Interface Methods


    
    public void AddAnxiety(int amount)
    {
        anxiety += amount;
        anxiety = Mathf.Clamp(anxiety, 0f, anxietyMax);
        SyncAnxietyLevelFromValue();
    }

    public void RemoveAnxiety(int amount)
    {
        anxiety -= amount;
        anxiety = Mathf.Clamp(anxiety, 0f, anxietyMax);
        SyncAnxietyLevelFromValue();
    }

    public float GetAnxiety()
    {
        return anxiety;
    }

    public bool GetIsAnxious()
    {
        return IsAnxious;
    }

    public void SetIsAnxious(bool value)
    {
        IsAnxious = value;
    }

    public Enum_AnxietyLevel GetAnxietyLevel()
    {
        return anxietyLevel;
    }



    #endregion
}
