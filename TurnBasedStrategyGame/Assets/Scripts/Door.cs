using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private GridPosition gridPosition;
    private Animator animator;
    private bool isOpen;
    private Action onInteractionComplete;
    private bool isActive;
    private float timer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        gridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        GridLevel.Instance.SetInteractableAtGridPosition(gridPosition, this);
        if (isOpen) { OpenDoor(); }
        else { CloseDoor(); }
    }
    private void Update()
    {
        if (!isActive) { return; }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }
    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;
        if (isOpen) { CloseDoor(); }
        else { OpenDoor(); }
    }
    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, isOpen);
    }
    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, isOpen);
    }
}
