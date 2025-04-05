using UnityEngine;

namespace Game.Interactions
{
    public class BaseInteractable : MonoBehaviour, IInteractable
    {

        public virtual void RightInputInteract()
        {
            
        }

        public virtual void LeftInputInteract()
        {
            
        }

        public virtual void HoverInteract()
        {
            
        }
        
        public virtual string RightInteractionMessage => string.Empty;
        public virtual string LeftInteractionMessage => string.Empty;
        
    }
}