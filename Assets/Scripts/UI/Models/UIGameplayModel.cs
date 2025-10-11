using Preferences;
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
            _popupTemplate = Assets.popupModal.Instantiate();
            _popupTemplate.AddToClassList("top-layer");
            _popupTemplate.AddToClassList("hide");
            _popupTemplate.AddToClassList("hiddable-element");
            View.Add(_popupTemplate);
            _popupModal = _popupTemplate.Q<VisualElement>("backdrop");
            
            var leave = _popupModal.Q<Button>("leave");
            var stay = _popupModal.Q<Button>("stay");

            leave.clicked += Application.Quit;
            stay.clicked += Manager.Escape;
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