using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class MainMenuSFX : MonoBehaviour
    {
        public static MainMenuSFX Instance;


        private AudioSource audioSource;
        [SerializeField] private AudioClip StartButtonSFX;
        [SerializeField] private AudioClip LoreButtonSFX;
        [SerializeField] private AudioClip WorkSFX;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else DontDestroyOnLoad(Instance);
        }

        void Start() => audioSource = GetComponent<AudioSource>();

        public void Play_StartButtonSFX() => audioSource.PlayOneShot(StartButtonSFX);
        public void Play_LoreButtonSFX() => audioSource.PlayOneShot(LoreButtonSFX);
        public void Play_CreditsButtonSFX() => audioSource.PlayOneShot(LoreButtonSFX);
        

        public void Play_AH_SFX()
        {
            audioSource.PlayOneShot(WorkSFX);
        }

    }
}
