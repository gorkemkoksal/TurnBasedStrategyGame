using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;

    private Vector3 targetPosition;
    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
    private void Update()
    {
        Vector3 dir = (targetPosition - transform.position).normalized;

        float beforeHitDistance = Vector3.Distance(targetPosition, transform.position);

        float moveSpeed = 200f;
        transform.position += dir * moveSpeed * Time.deltaTime;

        float afterHitDistance = Vector3.Distance(targetPosition, transform.position);


        if (beforeHitDistance < afterHitDistance)
        {
            transform.position = targetPosition;

            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVfxPrefab, targetPosition, Quaternion.identity);
        }
    }
}
