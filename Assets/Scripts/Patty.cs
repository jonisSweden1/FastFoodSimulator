using UnityEngine;

public class Patty : MonoBehaviour
{
    private bool _isGrilling = false;
    private bool _isTopSide = false;
    private bool _isBothSide = false;

    private CollisionNotifier _colNotice;

    [SerializeField] private float m_grillPerSecond;

    private float _topGrillPerc = 0;
    private float _bottomGrillPerc = 0;

    private void Awake()
    {
        _colNotice = transform.GetChild(0).GetComponent<CollisionNotifier>();
        _colNotice.OnCollided += _colNotice_OnCollided;
        _colNotice.OnUncollided += _colNotice_OnUncollided;
    }

    private void _colNotice_OnUncollided(Collision collision)
    {
        if (collision.collider.CompareTag("Grill"))
        {
            Debug.Log("It was a grill");
            _isGrilling = false;
        }
    }

    private void _colNotice_OnCollided(Collision collision)
    {
        if(collision.collider.CompareTag("Grill"))
        {
            Debug.Log("There is a grill");
            _isGrilling = true;
        }
    }

    private void Update()
    {
        
    }
}
