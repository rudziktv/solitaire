using System;
using Cards;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers
{
    public class GameAnimations : MonoBehaviour
    {
        [SerializeField] private CardAnimationMode animationMode = CardAnimationMode.DistanceBased;

        [Header("Time-based animations")] [SerializeField]
        private float moveTime = 0.5f;

        [Header("Velocity-based animations")] [SerializeField]
        private float moveVelocity = 2f;

        [Header("Distance-based animations")] [SerializeField]
        private float distancePerSecond = 5f;

        [Header("Easing")]
        [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        // [SerializeField] private EasingMode easing;
        // [SerializeField]
        // private float a = 1f;
        public static GameAnimations Instance { get; private set; }
        
        public float MoveTime => moveTime;

        private void Awake()
        {
            GameAnimations.Instance = this;
        }

        public Vector3 AnimatedMove(Vector3 origin, Vector3 destination, float time)
        {
            origin.z = destination.z;
            var duration = (origin - destination).magnitude / distancePerSecond;
            // var t = Mathf.Pow(time / duration, 2);
            if (duration == 0f)
                return destination;
            var t = time / duration;
            t = Mathf.Clamp01(t);
            return Vector3.Lerp(origin, destination, easingCurve.Evaluate(t));
        }
    }
}