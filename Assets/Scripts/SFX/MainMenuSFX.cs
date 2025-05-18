using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class MainMenuSFX : MonoBehaviour
    {
        public static MainMenuSFX Instance;


        private AudioSource audioSource;
        [SerializeField] private AudioClip ButtonSFX;
        [SerializeField] private AudioClip WorkSFX;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else DontDestroyOnLoad(Instance);
        }

        void Start() => audioSource = GetComponent<AudioSource>();

        public void Play_ButtonSFX()
        {
            audioSource.PlayOneShot(ButtonSFX);
        }

        public void Play_AH_SFX()
        {
            audioSource.PlayOneShot(WorkSFX);
        }

    }
}
