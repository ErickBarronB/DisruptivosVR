using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 target;

    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        Vector3 dir = (target - transform.position);

        if (dir.magnitude < 0.1f)
            return;

        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5f * Time.deltaTime);
        }
    }
}