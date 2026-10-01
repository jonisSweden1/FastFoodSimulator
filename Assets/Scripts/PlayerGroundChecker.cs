using UnityEngine;

// This script is to check if the player is on the ground, so that the player can jump or not
public class PlayerGroundChecker : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    private float groundCheckDistance = 0.1f; // Distance to check for ground

    // The ground layer
    [SerializeField] private LayerMask groundLayerMask;

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayerMask);
    }
}
