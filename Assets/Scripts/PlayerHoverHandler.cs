using UnityEngine;

public class PlayerHoverHandler : MonoBehaviour
{
    [SerializeField] private Material _hoverMaterial;

    Material _originalMaterial;

    GameObject _hoveredGameObject;

    bool hasHovered = false;

    [SerializeField] private float hoverDistance;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, hoverDistance))
        {
            if (_hoveredGameObject != null)
            {
                if (hit.collider.gameObject != _hoveredGameObject)
                {
                    _hoveredGameObject.GetComponent<Renderer>().material = _originalMaterial;
                    _hoveredGameObject = null;
                    hasHovered = false;
                    return;
                }
            }

            if (hit.collider.tag == "Item")
            {
                Renderer renderer;

                if (!hit.collider.gameObject.TryGetComponent<Renderer>(out renderer))
                {
                    Debug.LogError("There is no renderer");
                    return;
                }

                _hoveredGameObject = hit.collider.gameObject;
                renderer = _hoveredGameObject.GetComponent<Renderer>();

                if (!hasHovered)
                {
                    _originalMaterial = renderer.material;
                    hasHovered = true;
                }

                _hoveredGameObject.GetComponent<Renderer>().material = _hoverMaterial;
            }
        }
    }
}
