using UnityEngine;

namespace Cards
{
    public struct MovementConfig
    {
        public Vector3 Target { get; }
        public bool OnTop { get; }
        public int? FinalOrder { get; }
        
        public MovementConfig(Vector3 target, bool onTop = false)
        {
            Target = target;
            OnTop = onTop;
            FinalOrder = null;
        }
        
        public MovementConfig(Vector3 target, int? finalOrder, bool onTop)
        {
            Target = target;
            OnTop = onTop;
            FinalOrder = finalOrder;
        }
    }
}