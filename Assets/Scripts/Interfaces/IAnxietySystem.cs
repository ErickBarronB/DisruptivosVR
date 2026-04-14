using UnityEngine;

public interface IAnxietySystem
{
    void AddAnxiety(int amount);
    void RemoveAnxiety(int amount);
    float GetAnxiety();
    bool GetIsAnxious();
    void SetIsAnxious(bool value);
    Enum_AnxietyLevel GetAnxietyLevel();
}