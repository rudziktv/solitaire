using Modes;
using UI.Models;
using UnityEngine.UIElements;

namespace UI.Components
{
    public class UIGameModeCard : UIModel
    {
        public IGameMode GameMode { get; set; }
        
        public UIGameModeCard(UIController controller, VisualElement view) : base(controller, view) { }

        public override void OnViewCreated()
        {
            base.OnViewCreated();
            View.Q<Label>("title").text = GameMode.Data.Name;
            // View.Q<Label>("description").text = GameMode.Data.Description;

            View.Q<Button>("play").clicked += OnPlay;
        }

        private void OnPlay()
        {
            var difficulty = View.Q<RadioButtonGroup>("difficulty").value;
            Manager.LoadGameModeAndScene(GameMode, difficulty == 1 ? "hard" : "easy");

            
            var model = CreateView<UIGameplayModel>(Assets.gameplayGUI.Instantiate());
            Controller.NavigateTo(model.View, model);
        }
    }
}