using Modes;
using UI.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Models
{
    public class UIMainMenuModel : UIModel
    {
        private ScrollView _scrollView;
        
        public UIMainMenuModel(UIController controller, VisualElement view) : base(controller, view) { }

        public override void OnViewCreated()
        {
            base.OnViewCreated();
            
            _scrollView = View.Q<ScrollView>("menu-scroll");

            foreach (var mode in GameModes.List)
            {
                var model = CreateView<UIGameModeCard>(Assets.gamemodeComponent.Instantiate());
                model.GameMode = mode;
                model.OnViewCreated();
                _scrollView.Add(model.View);

                // SceneManager.LoadScene(0);
            }
            
            var exit = View.Q<Button>("exit");
            exit.clicked += Application.Quit;
            
            var settings = View.Q<Button>("settings");
            settings.SetEnabled(false);
        }
    }
}