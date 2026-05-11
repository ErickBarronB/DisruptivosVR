using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}