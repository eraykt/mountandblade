using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class EnemySFX : MonoBehaviour
    {
        public static EnemySFX Instance;
        public List<AudioClip> gruntClips;
        public List<AudioClip> swordClips;
        public AudioSource audioSource;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else DontDestroyOnLoad(Instance);
        }
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayHurtSFX()
        {
            int random = Random.Range(0, gruntClips.Count);
            audioSource.PlayOneShot(gruntClips[random]);
        }

        public void SwordSFX()
        {
            int random = Random.Range(0, gruntClips.Count);
            audioSource.PlayOneShot(swordClips[random]);
        }
    }
}
