using UnityEngine;

// This script is to manipulate the texture so that it shows how much amount there is in the sauce
public class SauceAmountDisplayer : MonoBehaviour
{
    private SauceDispenser sauceDispenser;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sauceDispenser = transform.parent.GetComponent<SauceDispenser>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
