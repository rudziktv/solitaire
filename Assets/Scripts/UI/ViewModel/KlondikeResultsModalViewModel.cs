using Unity.Properties;
using UnityEngine.UIElements;

namespace UI.ViewModel
{
    public class KlondikeResultsModalViewModel : BasicViewModel
    {
        [CreateProperty]
        public string TimeSpent { get; set; }
        
        [CreateProperty]
        public string MovesDone { get; set; }
        
        protected KlondikeResultsModalViewModel(UIController controller, VisualElement view, string timeSpent, string movesDone) : base(controller, view)
        {
            TimeSpent = timeSpent;
            MovesDone = movesDone;
        }

        public override void OnViewCreated()
        {
            base.OnViewCreated();

            var nextGame = View.Q<Button>("next-game");
            var mainMenu = View.Q<Button>("main-menu");

            nextGame.clicked += Manager.ReloadGameMode;
            mainMenu.clicked += Manager.LoadMainMenu;
        }
    }
}