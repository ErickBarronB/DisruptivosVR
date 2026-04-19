using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform head;

    [Header("Settings")]
    [SerializeField] private float rotateSpeed = 5f;

    private void LateUpdate()
    {
        if (head == null) return;

        Vector3 direction = transform.position - head.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);
    }
}
