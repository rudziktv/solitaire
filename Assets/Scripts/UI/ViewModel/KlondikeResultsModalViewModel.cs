using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.ViewModel
{
    // TODO - better UI
    public class KlondikeResultsModalViewModel : ModalViewModel
    {
        private string timeSpent = "00:00";
        private string movesDone = "0";

        [CreateProperty]
        public string TimeSpent
        {
            get => timeSpent;
            set { timeSpent = value; Notify(); }
        }

        [CreateProperty]
        public string MovesDone
        {
            get => movesDone;
            set { movesDone = value;  Notify(); }
        }

        public KlondikeResultsModalViewModel(UIController controller, VisualElement view) : base(controller, view)
        {
        }

        public override void OnViewCreated()
        {
            base.OnViewCreated();

            var modalView = Controller.Resources.klondikeResultModal.Instantiate();
            modalView.AddToClassList("modal-container");
            modalView.style.flexGrow = 1;
            View.Add(modalView);
            
            modalView.dataSource = this;

            var nextGame = View.Q<Button>("next-game");
            var mainMenu = View.Q<Button>("main-menu");

            nextGame.clicked += OnNextGameClicked;
            mainMenu.clicked += OnMainMenuClicked;
        }

        private void OnNextGameClicked()
        {
            SetVisibility(false);
            Manager.ReloadGameMode();
        }

        private void OnMainMenuClicked()
        {
            SetVisibility(false);
            Manager.LoadMainMenu();
        }

        public void SetSessionInfo(string timeSpent, string movesDone)
        {
            TimeSpent = timeSpent;
            MovesDone = movesDone;
        }

        public override void SetVisibility(bool visible)
        {
            base.SetVisibility(visible);
            if (visible)
                Manager.TimerPaused = true;
        }
    }
}