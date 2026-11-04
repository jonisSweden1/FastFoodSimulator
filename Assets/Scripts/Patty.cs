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

    private CollisionNotifier _colNotice;

    public float TopGrillPerc { get { return _topGrillPerc; } }
    public float BottomGrillPerc { get { return _bottomGrillPerc; } }

    private float _pattyTemperature = 0; // Celsius

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
            _grillSystem = null;
        }
    }

    private void _colNotice_OnCollided(Collision collision)
    {
        if(collision.collider.CompareTag("Grill"))
        {
            Debug.Log("There is a grill");
            if (collision.collider.TryGetComponent(out GrillSystem grillSystem))
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
            if(_isBothSide)
            {
                // Percentage of grill on top and bottom side, clamped between 0 and 2 (0 = raw, 1 = cooked, 2 = burnt)
                math.clamp(_topGrillPerc += Time.deltaTime * _grillPerSecond, 0, 2);
                math.clamp(_bottomGrillPerc += Time.deltaTime * _grillPerSecond, 0, 2);
                math.clamp(_pattyTemperature += Time.deltaTime * _grillTemperaturePerSecond * _grillPerSecond, 0, 177);
            }
            else if(_isTopSide)
            {
                // Percentage of grill on top and bottom side, clamped between 0 and 2 (0 = raw, 1 = cooked, 2 = burnt)
                math.clamp(_topGrillPerc += Time.deltaTime * _grillPerSecond, 0, 2);
                math.clamp(_pattyTemperature += Time.deltaTime * _grillTemperaturePerSecond * _grillPerSecond, 0, 79);
            }
            else
            {
                // Percentage of grill on top and bottom side, clamped between 0 and 2 (0 = raw, 1 = cooked, 2 = burnt)
                math.clamp(_bottomGrillPerc += Time.deltaTime * _grillPerSecond, 0, 2);
                math.clamp(_pattyTemperature += Time.deltaTime * _grillTemperaturePerSecond * _grillPerSecond, 25, 79);
            }
        }
        else
        {
            math.clamp(_pattyTemperature -= Time.deltaTime * _reducedTemperaturePerSecond, 1, 100);
        }

        Debug.Log($"Patty temperature: {_pattyTemperature}, Top grill percentage: {_topGrillPerc}, Bottom grill percentage: {_bottomGrillPerc}");

        NotifyCooked();
    }
}
