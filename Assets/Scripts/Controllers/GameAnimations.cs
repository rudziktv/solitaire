using System;
using System.Linq;
using Cards;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers
{
    public class GameAnimations : MonoBehaviour
    {
        [SerializeField] private CardAnimationMode animationMode = CardAnimationMode.DistanceBased;

        [Header("Time-based animations")]
        [SerializeField] private float minMoveTime = 0.1f;
        [SerializeField] private float maxMoveTime = 0.25f;

        [Header("Distance-based animations")] [SerializeField]
        private float distancePerSecond = 25f;
        public static GameAnimations Instance { get; private set; }
        
        private readonly AnimationCurve _easeInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private void Awake()
        {
            Instance = this;
        }

        public Vector3 AnimatedMove(Vector3 origin, Vector3 destination, float time)
        {
            origin.z = destination.z;
            var duration = (origin - destination).magnitude / distancePerSecond;
            if (duration == 0f)
                return destination;
            var t = time / duration;
            t = Mathf.Clamp01(t);
            // Debug.Log($"Time: {duration}, t: {t}, dist: {(origin - destination).magnitude}");
            // return AnimatedMoveExperimental(origin, destination, time);
            return AnimatedMoveV3(origin, destination, time);
            // return Vector3.Lerp(origin, destination, t);
        }

        private Vector3 AnimatedMoveV3(Vector3 origin, Vector3 destination, float time)
        {
            // reset z (to avoid invalid duration&distance calculations)
            origin.z = destination.z;
            
            // time = track / (avg) velocity
            var duration = (origin - destination).magnitude / distancePerSecond;
            duration = Mathf.Clamp(duration, minMoveTime, maxMoveTime);
            if (duration == 0f)
                return destination;
            
            var p = time / duration;
            var t = _easeInOutCurve.Evaluate(p);
            
            return Vector3.Lerp(origin, destination, t);
        }
    }
}