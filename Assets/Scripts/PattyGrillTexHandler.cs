using UnityEngine;

public class PattyGrillTexHandler : MonoBehaviour
{
    private Patty _patty;
    Material _pattyMaterial;

    [SerializeField]
    private string _topGrillPercProperty = "_TopGrillPerc";
    [SerializeField]
    private string _bottomGrillPercProperty = "_BottomGrillPerc";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _patty = transform.parent.GetComponent<Patty>();
        if (_patty == null)
        {
            Debug.LogError($"PattyGrillTexHandler: Patty component not found on parent object.");
            return;
        }

        _pattyMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float topGrillPerc = _patty.TopGrillPerc;
        float bottomGrillPerc = _patty.BottomGrillPerc;

        _pattyMaterial.SetFloat(_topGrillPercProperty, topGrillPerc);
        _pattyMaterial.SetFloat(_bottomGrillPercProperty, bottomGrillPerc);
    }
}
