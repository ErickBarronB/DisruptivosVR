public interface IAnxietySystem
{
    void AddAnxiety(float amount);
    void RemoveAnxiety(float amount);
    float GetAnxiety();
    bool GetIsAnxious();
    Enum_AnxietyLevel GetAnxietyLevel();
}