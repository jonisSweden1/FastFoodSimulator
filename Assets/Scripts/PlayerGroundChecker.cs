using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    private float groundCheckDistance = 0.1f; // Distance to check for ground

    [SerializeField] private LayerMask groundLayerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayerMask);
        
    }
}
