using UnityEngine;

namespace Game.Interactions
{
    public interface IInteractable
    {
        Transform transform { get; }
        void RightInputInteract();
        void LeftInputInteract();
        void HoverInteract();
        
    }
}