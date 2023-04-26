using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public RaycastHit rayHit;
    public float range;
    public LayerMask whatIsHittable;
    public IInteractable selected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out rayHit, range, whatIsHittable))
        {
            IInteractable interactable = rayHit.collider.transform.root.GetComponent<IInteractable>();
            if (interactable != null)
            {
                selected = interactable;
                interactable.OnSelected();
            }
        }
        else
        {
            if (selected != null)
            {
                selected.OnDeselected();
                selected = null;
            }
        }
        if (selected != null && Input.GetKeyDown(KeyCode.F))
        {
            selected.OnInteracted();
        }
    }
    
}
