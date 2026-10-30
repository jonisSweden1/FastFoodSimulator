using UnityEngine;

public class CollisionNotifier : MonoBehaviour
{
    public delegate void CollisionEvent(Collision collision);
    public event CollisionEvent OnCollided;
    public event CollisionEvent OnUncollided;

    private void OnCollisionEnter(Collision collision)
    {
        OnCollided?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnUncollided?.Invoke(collision);
    }
}
