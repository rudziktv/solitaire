using System;
using Controllers;
using Preferences;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        protected GameManager Manager => GameManager.Instance;
        protected GameRules Rules => Manager.GameRules;
        protected GameSounds Sounds => GameSounds.Instance;
        
        [SerializeField] private UIDocument uiDoc;
        
        private VisualElement Root => uiDoc.rootVisualElement;
        
        private void Start()
        {
            var res = Root.Q<Button>("restart-game");
            var exit = Root.Q<Button>("exit-game");
            var ver = Root.Q<Label>("version");
            
            var undo = Root.Q<Button>("undo");
            undo.SetEnabled(false);

            res.clicked += Manager.LoadGameMode;
            exit.clicked += Application.Quit;
            ver.text = Application.version;
            
            VolumeControlSetup();
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