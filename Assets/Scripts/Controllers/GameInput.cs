using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class GameInput : MonoBehaviour
    {
        private GameManager Manager => GameManager.Instance;
        
        private SolitaireInputActions _actions;

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
            _actions = new ();
        }

        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }

        private void Start()
        {
            _actions.Standard.Exit.performed += Escape;
            _actions.Standard.Undo.performed += OnUndo;
            
            _actions.Standard.LeftControl.started += OnLeftControlPress;
            _actions.Standard.LeftControl.canceled += OnLeftControlReleased;
        }

        private void OnDestroy()
        {
            _actions.Standard.Exit.performed -= Escape;
            _actions.Standard.Undo.performed -= OnUndo;

            _actions.Standard.LeftControl.started -= OnLeftControlPress;
            _actions.Standard.LeftControl.canceled -= OnLeftControlReleased;
        }
    }
}