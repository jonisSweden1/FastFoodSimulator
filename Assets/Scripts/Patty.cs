using Unity.Mathematics;
using UnityEngine;

public class Patty : MonoBehaviour
{
    public delegate void PattyFlipped(bool isTopSide);
    public delegate void PattyGrillStateChanged(float topGrillPerc, float bottomGrillPerc);
    public event PattyGrillStateChanged OnPattyGrillStateChanged;
    public event PattyFlipped OnPattyFlipped;

    [SerializeField] private GameObject _smokeParticlePrefab;

    private GameObject _smokeParticle;

    private bool _isGrilling = false;
    private bool _isTopSide = false;

    private bool _isParticleInstantiated = false;

    // This is a feature for a future feature where you can upgrade the equipment to grill both sides of the patty at the same time.
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

        StopParticle();
    }

    public void FlipPatty()
    {
        if (_isTopSide)
        {
            _isTopSide = false;
        }
        else
        {
            _isTopSide = true;
        }

        OnPattyFlipped?.Invoke(_isTopSide);
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

                    if (!_isParticleInstantiated)
                    {
                        InstantiateSmokeParticle();
                        _isParticleInstantiated = true;
                    }
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

    private void InstantiateSmokeParticle()
    {
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f); // Rotate the particle system to face upwards
        _smokeParticle = Instantiate(_smokeParticlePrefab, transform.position, rotation);
    }

    private void StopParticle()
    {
        if (_smokeParticle != null)
        {
            if (_smokeParticle.TryGetComponent(out ParticleSystem particleSystem))
            {
                particleSystem.Stop();
            }
            else
            {
                Destroy(_smokeParticle);
            }
            _isParticleInstantiated = false;
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

                if (!_isParticleInstantiated)
                {
                    InstantiateSmokeParticle();
                    _isParticleInstantiated = true;
                }
            }
            else
            {
                _isGrilling = false;
                _grillSystem = null;

                StopParticle();
            }
        }

        if(_isGrilling)
        {
            //Debug.Log($"Patty is grilling. Grill per second: {_grillPerSecond}, Grill temperature per second: {_grillTemperaturePerSecond}");
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

        Debug.Log($"Patty temperature: {_pattyTemperature}, Top grill percentage: {_topGrillPerc}, Bottom grill percentage: {_bottomGrillPerc}");

        NotifyCooked();
    }
}
