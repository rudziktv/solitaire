using System;
using Controllers;
using Preferences;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        protected GameManager Manager => GameManager.Instance;
        protected GameRules Rules => Manager.GameRules;
        protected GameSounds Sounds => GameSounds.Instance;
        
        [SerializeField] private UIDocument uiDoc;
        [SerializeField] private VisualTreeAsset popupModal;
        
        private VisualElement Root => uiDoc.rootVisualElement;
        private VisualElement _popupModal;
        private TemplateContainer _popupTemplate;

        private Label _timer;
        
        
        private void Start()
        {
            Instance = this;
            
            var res = Root.Q<Button>("restart-game");
            var exit = Root.Q<Button>("exit-game");
            var ver = Root.Q<Label>("version");
            
            var undo = Root.Q<Button>("undo");
            // undo.SetEnabled(false);
            undo.clicked += () =>
            {
                Manager.Undo();
            };
            
            undo.SetEnabled(false);

            Manager.OnActionsChanged += (undoNotEmpty, _) =>
            {
                undo.SetEnabled(undoNotEmpty);
            };
            
            // settings work in progress
            var settings = Root.Q<Button>("settings");
            settings.SetEnabled(false);
            
            res.clicked += Manager.ReloadGameMode;
            exit.clicked += Application.Quit;
            ver.text = Application.version;
            
            _timer = Root.Q<Label>("timer");
            
            VolumeControlSetup();
        }

        private void Update()
        {
            _timer.text = TimeUtils.FormatTimer(Manager.Timer);
        }

        private void VolumeControlSetup()
        {
            var mute = Root.Q<Button>("mute");
            var volume = Root.Q<Slider>("volume");
            var volumeBox = Root.Q<VisualElement>("volume-box");
            
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
            InitializePopup();
        }

        private void InitializePopup()
        {
            _popupTemplate = popupModal.Instantiate();
            _popupTemplate.AddToClassList("top-layer");
            _popupTemplate.AddToClassList("hide");
            _popupTemplate.AddToClassList("hiddable-element");
            Root.Add(_popupTemplate);
            _popupModal = _popupTemplate.Q<VisualElement>("backdrop");
            
            var leave = _popupModal.Q<Button>("leave");
            var stay = _popupModal.Q<Button>("stay");

            leave.clicked += Application.Quit;
            stay.clicked += Manager.Escape;
        }

        public void EscapePopupOpen()
        {
            _popupTemplate.ToggleInClassList("hide");
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