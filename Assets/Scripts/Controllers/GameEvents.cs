using System;
using UnityEngine;

namespace Controllers
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance { get; private set; }

        private void Start()
        {
            Instance = this;
        }
    }
}