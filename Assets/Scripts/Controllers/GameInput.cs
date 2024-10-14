using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class GameInput : MonoBehaviour
    {
        private GameManager Manager => GameManager.Instance;
        
        public SolitaireInputActions Actions { get; private set; }

        #region InputCallbacks

        private void Escape(InputAction.CallbackContext ctx) => Manager.Escape();
        private void OnUndo(InputAction.CallbackContext ctx)
        {
            if (_leftControlDown) Manager.Undo();
        }

        private void OnLeftControlPress(InputAction.CallbackContext ctx) => _leftControlDown = true;
        private void OnLeftControlReleased(InputAction.CallbackContext ctx) => _leftControlDown = false;

        #endregion
        
        private bool _leftControlDown = false;

        private void Awake()
        {
            Actions = new ();
        }

        private void OnEnable()
        {
            Actions.Enable();
        }

        private void OnDisable()
        {
            Actions.Disable();
        }

        private void Start()
        {
            Actions.Standard.Exit.performed += Escape;
            Actions.Standard.Undo.performed += OnUndo;
            
            Actions.Standard.LeftControl.started += OnLeftControlPress;
            Actions.Standard.LeftControl.canceled += OnLeftControlReleased;
        }

        private void OnDestroy()
        {
            Actions.Standard.Exit.performed -= Escape;
            Actions.Standard.Undo.performed -= OnUndo;

            Actions.Standard.LeftControl.started -= OnLeftControlPress;
            Actions.Standard.LeftControl.canceled -= OnLeftControlReleased;
        }
    }
}