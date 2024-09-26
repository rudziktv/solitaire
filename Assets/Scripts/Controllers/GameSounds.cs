using System;
using UnityEngine;

namespace Controllers
{
    public class GameSounds : MonoBehaviour
    {
        public static GameSounds Instance { get; private set; }
        
        [SerializeField] private AudioClip pickUpCardClip;
        [SerializeField] private AudioClip putDownCardClip;
        [SerializeField] private AudioClip dropDownCardClip;


        private void Awake()
        {
            Instance = this;
        }

        public AudioClip PickUpCardSound => pickUpCardClip;
        public AudioClip PutDownCardSound => putDownCardClip;
        public AudioClip DropDownCardSound => dropDownCardClip;
    }
}