using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        protected GameManager Manager => GameManager.Instance;
        protected GameRules Rules => Manager.GameRules;
        
        [SerializeField] private UIDocument uiDoc;
        
        private VisualElement Root => uiDoc.rootVisualElement;
        
        private void Start()
        {
            var res = Root.Q<Button>("restart-game");
            var exit = Root.Q<Button>("exit-game");
            var ver = Root.Q<Label>("version");

            res.clicked += Manager.LoadGameMode;
            exit.clicked += Application.Quit;
            ver.text = Application.version;
        }
    }
}