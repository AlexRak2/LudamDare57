using UnityEngine;

namespace Game.Interactions
{
    public interface IInteractable
    {
        Transform transform { get; }
        string RightInteractionMessage { get; }
        string LeftInteractionMessage { get; }
        void RightInputInteract();
        void LeftInputInteract();
        void HoverInteract();
        
    }
}