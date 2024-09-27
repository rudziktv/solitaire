using System;
using Preferences;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controllers
{
    public class GameSounds : MonoBehaviour
    {
        public static GameSounds Instance { get; private set; }
        
        [SerializeField] private AudioClip pickUpCardClip;
        [SerializeField] private AudioClip putDownCardClip;
        [SerializeField] private AudioClip dropDownCardClip;
        [SerializeField] private AudioClip flipCardClip;
        [SerializeField] private AudioClip resetDeckClip;


        private void Awake()
        {
            Instance = this;
        }

        public AudioClip PickUpCardSound => pickUpCardClip;
        public AudioClip PutDownCardSound => putDownCardClip;
        public AudioClip DropDownCardSound => dropDownCardClip;
        public AudioClip FlipCardSound => flipCardClip;
        public AudioClip ResetDeckSound => resetDeckClip;

        public bool ToggleMute()
        {
            var prefMute = Prefs.GetBool(PreferencesList.Mute);
            SetMute(!prefMute);
            return !prefMute;
        }

        public void SetMute(bool mute)
        {
            var prefVolume = Prefs.GetFloat(PreferencesList.Volume);
            Prefs.SetBool(PreferencesList.Mute, mute);
            AudioListener.volume = mute ? 0f : prefVolume;
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
            Prefs.SetFloat(PreferencesList.Volume, volume);
        }
    }
}