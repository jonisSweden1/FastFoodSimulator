using UnityEngine;

public class GrillSystem : MonoBehaviour
{
    [SerializeField] private float m_grillPerSecond = 0.025f;

    private bool _isActivated = false;

    public bool CheckIfActivated(out float grillPerSecond)
    {
        grillPerSecond = m_grillPerSecond;
        return _isActivated;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
