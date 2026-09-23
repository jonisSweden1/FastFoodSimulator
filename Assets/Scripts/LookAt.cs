using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform _lookObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_lookObject)
        {
            Vector3 direction = _lookObject.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }
    }
}
