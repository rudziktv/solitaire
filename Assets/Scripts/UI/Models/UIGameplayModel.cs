using Preferences;
using UI.ViewModel;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace UI.Models
{
    public class UIGameplayModel : UIModel
    {
        public UIGameplayModel(UIController controller, VisualElement view) : base(controller, view) { }

        private Label _timer;
        private VisualElement _popupModal;
        private TemplateContainer _popupTemplate;

        private KlondikeResultsModalViewModel _resultsModal;
        
        public override void OnViewCreated()
        {
            base.OnViewCreated();

            var menu = View.Q<Button>("main-menu");
            var res = View.Q<Button>("restart-game");
            var exit = View.Q<Button>("exit-game");
            var ver = View.Q<Label>("version");
            
            var undo = View.Q<Button>("undo");
            undo.SetEnabled(false);
            undo.clicked += () =>
            {
                Manager.Undo();
            };
            Manager.OnActionsChanged += (undoNotEmpty, _) =>
            {
                undo.SetEnabled(undoNotEmpty);
            };
            
            var settings = View.Q<Button>("settings");
            settings.SetEnabled(false);
            
            var getItDone = View.Q<Button>("get-it-done");
            getItDone.SetEnabled(false);
            Manager.GameRules.OnGetItDoneChanged += possible =>
            {
                getItDone.SetEnabled(possible);
            };
            getItDone.clicked += Manager.GameRules.GetItDone;
            
            
            menu.clicked += GameManager.Instance.LoadMainMenu;
            res.clicked += Manager.ReloadGameMode;
            exit.clicked += Application.Quit;
            ver.text = Application.version;
            
            _timer = View.Q<Label>("timer");
            
            VolumeControlSetup();
            InitializePopup();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _timer.text = TimeUtils.FormatTimer(Manager.Timer);
        }
        
        private void InitializePopup()
        {
            _resultsModal = new KlondikeResultsModalViewModel(Controller, this.CreateModalLayer());
            _resultsModal.OnViewCreated();

            Manager.GameRules.OnSessionFinished += () =>
            {
                _resultsModal.SetSessionInfo(_timer.text, Manager.Moves.ToString());
                _resultsModal.SetVisibility(true);
            };
        }
        
        private void VolumeControlSetup()
        {
            var mute = View.Q<Button>("mute");
            var volume = View.Q<Slider>("volume");
            var volumeBox = View.Q<VisualElement>("volume-box");
            
            // Setup callbacks
            mute.clicked += () =>
            {
                var muted = Sounds.ToggleMute();
                volume.SetEnabled(!muted);
                VolumeControlSyncUI(mute, volume, volumeBox);
            };
            
            volume.RegisterValueChangedCallback((e) =>
            {
                Sounds.SetVolume(e.newValue);
                VolumeControlSyncUI(mute, volume, volumeBox);
            });
            
            // Initialize with saved values
            volume.value = Prefs.GetFloat(PreferencesList.Volume);
            VolumeControlSyncUI(mute, volume, volumeBox);
        }
        
        public static Sprite CurrentVolumeIcon()
        {
            var v = Prefs.GetFloat(PreferencesList.Volume);
            var m = Prefs.GetBool(PreferencesList.Mute);
        
            if (m)
                return Resources.Load<Sprite>("UI/Icons/volume-mute-line");
            if (v > 0.5f)
                return Resources.Load<Sprite>("UI/Icons/volume-up-line");
            
            return Resources.Load<Sprite>("UI/Icons/volume-down-line");
        }

        private void VolumeControlSyncUI(Button btn, Slider volume, VisualElement volumeBox)
        {
            var iconImg = UIManager.CurrentVolumeIcon();
            var iconElement = btn.Q<VisualElement>("icon");
            iconElement.style.backgroundImage = iconImg.texture;
            var muted = Prefs.GetBool(PreferencesList.Mute);
            volume.SetEnabled(!muted);
            if (muted)
                volumeBox.AddToClassList("hide");
            else
                volumeBox.RemoveFromClassList("hide");
        }
    }
}