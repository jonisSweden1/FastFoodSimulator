using UnityEngine;

public class PattyModelHandler : MonoBehaviour
{
    private Patty _patty;
    Material _pattyMaterial;

    private Animator _animator;

    private float _xRotation = 0f;

    [SerializeField]
    private string _topGrillPercProperty = "_Top_side_grill";
    [SerializeField]
    private string _bottomGrillPercProperty = "_Bottom_side_grill";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pattyMaterial = GetComponent<Renderer>().material;
    }

    private void OnEnable()
    {
        _patty = transform.parent.GetComponent<Patty>();
        

        _animator = transform.GetComponent<Animator>();

        if (_patty == null)
        {
            Debug.LogError($"PattyGrillTexHandler: Patty component not found on parent object.");
            return;
        }

        if (_patty.IsTopSide)
        {
            _animator.Play("patty_idle_bottom");
        }
        else
        {
            _animator.Play("patty_idle_top");
        }

        _patty.OnPattyFlipped += _patty_OnPattyFlipped;
    }

    private void _patty_OnPattyFlipped(bool isTopSide)
    {
        if (isTopSide)
        {
            _animator.Play("Flip_patty_from_top");
            
            //_xRotation = -90f;
        }
        else
        {
            _animator.Play("Flip_patty_from_bottom");
            //_xRotation = -270f;
        }
        Vector3 currentRotation = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(_xRotation, currentRotation.y, currentRotation.z);
    }

    private void OnDisable()
    {
        _patty.OnPattyFlipped -= _patty_OnPattyFlipped;
    }

    // Update is called once per frame
    void Update()
    {
        float topGrillPerc = _patty.TopGrillPerc;
        float bottomGrillPerc = _patty.BottomGrillPerc;

        // 1. Verify the shader actually has these property names
        if (!_pattyMaterial.HasFloat(_topGrillPercProperty))
        {
            Debug.LogError($"Shader does NOT contain property: {_topGrillPercProperty}");
            return;
        }

        _pattyMaterial.SetFloat(_topGrillPercProperty, topGrillPerc);
        _pattyMaterial.SetFloat(_bottomGrillPercProperty, bottomGrillPerc);

        // 2. Log both the source value and the applied material value
        Debug.Log($"Source: {topGrillPerc} | Applied on Mat: {_pattyMaterial.GetFloat(_topGrillPercProperty)}");
    }
}
