using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4f;
    private readonly int walkingHash = Animator.StringToHash("IsWalking");

    private Vector3 targetPosition;
    private GridPosition gridPosition;
    private void Awake()
    {
        targetPosition = transform.position;
    }
    private void Start()
    {
        gridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        GridLevel.Instance.AddUnitAtGridPosition(gridPosition, this);
    }
    void Update()
    {
        var stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            var rotationSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

            unitAnimator.SetBool(walkingHash, true);
        }
        else
            unitAnimator.SetBool(walkingHash, false);

        GridPosition newGridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridLevel.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }

    }
    public void Move(Vector3 targetPosition) => this.targetPosition = targetPosition;
}
