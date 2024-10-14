using System;
using UI.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocument))]
    public class UIController : MonoBehaviour
    {
        public UIResources Resources => resources;
        
        [SerializeField] private UIResources resources;
        
        
        private UIDocument _uiDocument;
        private VisualElement Root => _uiDocument.rootVisualElement;
        private UIModel _currentModel;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            _uiDocument = GetComponent<UIDocument>();
            // _currentModel = new UIMainMenuModel(this, Root);
            ChangeModel(new UIMainMenuModel(this, Root));
        }

        private void Update()
        {
            _currentModel?.OnUpdate();
        }

        private void ChangeModel(UIModel model)
        {
            _currentModel = model;
            model.OnViewCreated();
        }

        public void NavigateTo(VisualElement view, UIModel model)
        {
            _currentModel.OnViewDestroy();
            Root.Clear();
            view.style.flexGrow = 1;
            _uiDocument.rootVisualElement.Add(view);
            ChangeModel(model);
        }
    }
}