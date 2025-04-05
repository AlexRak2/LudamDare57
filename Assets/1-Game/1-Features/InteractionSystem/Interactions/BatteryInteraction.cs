using UnityEngine;

namespace Game.Interactions.Lever
{
    public class BatteryInteraction : BaseInteractable
    {
        public override void LeftInputInteract()
        {
            Destroy(gameObject);
        }
        
        public override string LeftInteractionMessage => "Pick Up Battery";
    }
}