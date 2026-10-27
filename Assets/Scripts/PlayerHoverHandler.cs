using UnityEngine;

public class PlayerHoverHandler : MonoBehaviour
{
    [SerializeField] private Material _hoverMaterial;

    Material _originalMaterial;

    GameObject _hoveredGameObject;

    [SerializeField] private float hoverDistance;

    public static PlayerHoverHandler Instance { get; private set; }

    private void Start()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, hoverDistance) && hit.collider.tag == "Item")
        {
            GameObject currentHitObj = hit.collider.gameObject;

            // We want to always reset the material before assigning it with something
            // This way, if the game object gets destroyed, then it will reset the material
            if (_hoveredGameObject != null && _hoveredGameObject != currentHitObj)
            {
                ResetHover();
            }

            if (_hoveredGameObject == null)
            {
                if (currentHitObj.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    _hoveredGameObject = currentHitObj;
                    _originalMaterial = renderer.material;
                    renderer.material = _hoverMaterial;
                }
            }
        }
        else
        {
            if(_hoveredGameObject != null)
            {
                ResetHover();
            }
        }
    }

    public void ResetHover()
    {
        if (_hoveredGameObject != null)
        {
            if (_hoveredGameObject.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.material = _originalMaterial;
            }
        }

        _hoveredGameObject = null;
        _originalMaterial = null;
    }
}
