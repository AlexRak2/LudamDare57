
using Game.Input;
using Game.UI;
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
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    if(interactable.LeftInteractionMessage != string.Empty)
                        PlayerHud.Instance.ShowLeftInteractionUI(interactable.LeftInteractionMessage);
                    else
                        PlayerHud.Instance.HideLeftInteractionUI();

                    if(interactable.RightInteractionMessage != string.Empty)
                        PlayerHud.Instance.ShowRightInteractionUI(interactable.RightInteractionMessage);
                    else
                        PlayerHud.Instance.HideRightInteractionUI();
                    
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
                else
                {
                    PlayerHud.Instance.HideLeftInteractionUI();
                    PlayerHud.Instance.HideRightInteractionUI();
                }
            }
            else
            {
                PlayerHud.Instance.HideLeftInteractionUI();
                PlayerHud.Instance.HideRightInteractionUI();            
            }
        }
    }
}