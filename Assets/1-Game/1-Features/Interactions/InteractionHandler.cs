
using Game.Input;
using UnityEngine;

namespace Game.Interactions
{
    public class InteractionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _interactionMask;
        [SerializeField] private float _interactionDistance;
        [SerializeField] private Transform _camera;

        
        private void Update()
        {
            HandleInteractions();
        }

        private void HandleInteractions()
        {

            if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _interactionDistance, _interactionMask))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    interactable.HoverInteract();
                    
                    if (InputManager.RightAction.WasPressedThisFrame())
                    {
                        interactable.RightInputInteract();
                    }
                    
                    if (InputManager.LeftAction.WasPressedThisFrame())
                    {
                        interactable.LeftInputInteract();
                    }
                }
            }
        }
    }
}