using Unity.Mathematics;
using UnityEngine;

public class Patty : MonoBehaviour
{
    public delegate void PattyGrillStateChanged(float topGrillPerc, float bottomGrillPerc);
    public event PattyGrillStateChanged OnPattyGrillStateChanged;

    private bool _isGrilling = false;
    private bool _isTopSide = false;
    private bool _isBothSide = false;

    private float _grillPerSecond = 0;

    [SerializeField] private float _reducedTemperaturePerSecond = 0;

    [SerializeField] private float _grillTemperaturePerSecond = 0;

    private GrillSystem _grillSystem = null;

    private TriggerNotifier _triNotice;

    public float TopGrillPerc { get { return _topGrillPerc; } }
    public float BottomGrillPerc { get { return _bottomGrillPerc; } }

    private float _pattyTemperature = 0; // Celsius

    private float _topGrillPerc = 0;
    private float _bottomGrillPerc = 0;

    private void Awake()
    {
        _triNotice = transform.GetChild(0).GetComponent<TriggerNotifier>();
        _triNotice.OnTriggerCollided += _triNotice_OnCollided;
        _triNotice.OnTriggerUncollided += _triNotice_OnUncollided;
    }

    private void _triNotice_OnUncollided(Collider other)
    {
        _isGrilling = false;
        _grillSystem = null;
    }

    private void _triNotice_OnCollided(Collider other)
    {
        Debug.Log("Collided with: " + other.name);

        if(other.CompareTag("Grill"))
        {
            Debug.Log("There is a grill");
            if (other.transform.parent.TryGetComponent(out GrillSystem grillSystem))
            {
                _grillSystem = grillSystem;

                if (_grillSystem.CheckIfActivated(out float grillPerSecond))
                {
                    Debug.Log($"Grill system found with grill per second: {grillPerSecond}");
                    _grillPerSecond = grillPerSecond;
                    _isGrilling = true;
                }
            }
        }
    }

    // This is a feature that can be used to notify other systems when the patty is fully cooked on either side.
    // This is also an suggestion for a future feature where you can upgrade the equipment to notify you when the patty is fully cooked on either side, among other things.
    private void NotifyCooked()
    {
        if (_isBothSide)
        {
            if (_topGrillPerc >= 1 && _bottomGrillPerc >= 1)
            {
                // Both sides are cooked
                OnPattyGrillStateChanged?.Invoke(_topGrillPerc, _bottomGrillPerc);
            }
        }
        else if (_isTopSide)
        {
            if (_topGrillPerc >= 1)
            {
                // Top side is cooked
                OnPattyGrillStateChanged?.Invoke(_topGrillPerc, _bottomGrillPerc);
            }
        }
        else
        {
            if (_bottomGrillPerc >= 1)
            {
                // Bottom side is cooked
                OnPattyGrillStateChanged?.Invoke(_topGrillPerc, _bottomGrillPerc);
            }
        }
    }

    private void Update()
    {
        if(_grillSystem != null)
        {
            if(_grillSystem.CheckIfActivated(out float grillPerSecond))
            {
                _grillPerSecond = grillPerSecond;
                _isGrilling = true;
            }
            else
            {
                _isGrilling = false;
                _grillSystem = null;
            }
        }

        if(_isGrilling)
        {
            Debug.Log($"Patty is grilling. Grill per second: {_grillPerSecond}, Grill temperature per second: {_grillTemperaturePerSecond}");
            if (_isBothSide)
            {
                // Percentage of grill on top and bottom side, clamped between 0 and 2 (0 = raw, 1 = cooked, 2 = burnt)
                _topGrillPerc = math.clamp(_topGrillPerc += Time.deltaTime * _grillPerSecond, 0.0f, 2.0f);
                _bottomGrillPerc = math.clamp(_bottomGrillPerc += Time.deltaTime * _grillPerSecond, 0.0f, 2.0f);
                _pattyTemperature = math.clamp(_pattyTemperature += Time.deltaTime * _grillTemperaturePerSecond * _grillPerSecond, 0.0f, 177.0f);
            }
            else if(_isTopSide)
            {
                // Percentage of grill on top and bottom side, clamped between 0 and 2 (0 = raw, 1 = cooked, 2 = burnt)
                _topGrillPerc = math.clamp(_topGrillPerc += Time.deltaTime * _grillPerSecond, 0.0f, 2.0f);
                _pattyTemperature = math.clamp(_pattyTemperature += Time.deltaTime * _grillTemperaturePerSecond * _grillPerSecond, 0.0f, 79.0f);
            }
            else
            {
                // Percentage of grill on top and bottom side, clamped between 0 and 2 (0 = raw, 1 = cooked, 2 = burnt)
                _bottomGrillPerc = math.clamp(_bottomGrillPerc += Time.deltaTime * _grillPerSecond, 0.0f, 2.0f);
                _pattyTemperature = math.clamp(_pattyTemperature += Time.deltaTime * _grillTemperaturePerSecond * _grillPerSecond, 25.0f, 79.0f);
            }
        }
        else
        {
            _pattyTemperature = math.clamp(_pattyTemperature -= Time.deltaTime * _reducedTemperaturePerSecond, 20.0f, 100.0f);
        }

        //Debug.Log($"Patty temperature: {_pattyTemperature}, Top grill percentage: {_topGrillPerc}, Bottom grill percentage: {_bottomGrillPerc}");

        NotifyCooked();
    }
}
