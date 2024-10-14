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
        
        [Header("Card Sounds")]
        [SerializeField] private AudioClip pickUpCardClip;
        [SerializeField] private AudioClip putDownCardClip;
        [SerializeField] private AudioClip dropDownCardClip;
        [SerializeField] private AudioClip flipCardClip;
        [SerializeField] private AudioClip resetDeckClip;
        [SerializeField] private AudioClip deckCardUndoClip;
        [SerializeField] private AudioClip resetDeckUndoClip;

        [Header("Interface sounds")]
        [SerializeField] private AudioClip moveNotAllowedClip;
        
        public AudioClip PickUpCardSound => pickUpCardClip;
        public AudioClip PutDownCardSound => putDownCardClip;
        public AudioClip DropDownCardSound => dropDownCardClip;
        public AudioClip FlipCardSound => flipCardClip;
        public AudioClip ResetDeckSound => resetDeckClip;
        public AudioClip ResetDeckUndoSound => resetDeckUndoClip;
        public AudioClip MoveNotAllowedSound => moveNotAllowedClip;
        public AudioClip DeckCardUndoSound => deckCardUndoClip;
        
        
        private void Awake()
        {
            Instance = this;
        }
        
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