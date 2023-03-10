using UnityEngine;
using System;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private Transform grenadeExplodeVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetPosition;
    private Action onGrenadeBehaviourComplete;
    private float totalDistance;
    private Vector3 positionXZ;
    private void Update()
    {
        var moveDir = (targetPosition - positionXZ).normalized;
        var moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        var distance = Vector3.Distance(positionXZ, targetPosition);
        var distanceNormalized = 1 - distance / totalDistance;

        var maxHeight = totalDistance / 4;
        var positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        print(positionY);
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        var reachedDistance = 0.2f;
        if (Vector3.Distance(positionXZ, targetPosition) < reachedDistance)
        {
            var damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
            }
            ScreenShake.Instance.Shake(5f);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVFXPrefab, targetPosition + Vector3.up, Quaternion.identity);
            Destroy(gameObject);

            onGrenadeBehaviourComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this.onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        targetPosition = GridLevel.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }
}
