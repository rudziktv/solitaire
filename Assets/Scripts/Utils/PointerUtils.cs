using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Utils
{
    public static class PointerUtils
    {
        public static Vector2 GetPointerPosition()
        {
            // Debug.Log("Pointer Position: " + Pointer.current.position.ReadValue());
            // return Touch.activeTouches.Count > 0 ? Touch.activeTouches[0].screenPosition : Mouse.current.position.ReadValue();
            return Pointer.current.position.ReadValue();
        }
    }
}