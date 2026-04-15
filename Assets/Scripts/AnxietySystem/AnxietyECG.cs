using UnityEngine;

public class AnxietyECG : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private int resolution = 120;
    [SerializeField] private float baseSpeed = 1.5f;
    [SerializeField] private float xSpacing = 0.03f;

    [SerializeField] private System_PlayerAnxiety anxietySystem;

    private float time;

    void Update()
    {
        if (anxietySystem == null || line == null) return;

        float anxiety = anxietySystem.GetAnxiety() / 180f;

        float dynamicSpeed = baseSpeed + anxiety * 3f;
        time += Time.deltaTime * dynamicSpeed;

        line.positionCount = resolution;

        for (int i = 0; i < resolution; i++)
        {
            float x = i * xSpacing;

            float t = (i * xSpacing + time) % 1f;

            float y = GenerateECG(t, anxiety);

            line.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    float GenerateECG(float t, float anxiety)
    {
        float y = 0;

        float peak = 1.2f + anxiety * 2f;

        if (t < 0.05f)
        {
            y = Mathf.Lerp(0, peak, t / 0.05f); 
        }
        else if (t < 0.1f)
        {
            y = Mathf.Lerp(peak, -0.6f, (t - 0.05f) / 0.05f); 
        }
        else if (t < 0.2f)
        {
            y = Mathf.Lerp(-0.6f, 0, (t - 0.1f) / 0.1f);
        }
        else
        {
            y = Mathf.Sin(t * 25f) * 0.03f;
        }

        return y;
    }
}
