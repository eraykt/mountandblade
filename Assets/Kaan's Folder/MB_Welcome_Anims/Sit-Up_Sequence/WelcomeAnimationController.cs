using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MountAndBlade
{
    public class WelcomeAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private string SceneToLoad;

        [Header("Buttons")]
        public Button Button_StartGame;
        public Button Button_Lore;
        public Button Button_Credits;

        [Header("Bools")]
        public bool StartGame = false;
        public bool ButtonClicked = false;
        public int IsFirstClicked = 0;
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            StartCoroutine(InitCoroutine());

            // Listeners
            Button_StartGame.onClick.AddListener(Func_StartGame);
            Button_Lore.onClick.AddListener(Func_Lore);
            Button_Credits.onClick.AddListener(Func_Credits);
        }

        private void Func_StartGame()
        {
            IsFirstClicked++;
            ButtonClicked = true;
            if (IsFirstClicked == 1)
                SceneToLoad = "02_MapScene";
        }

        private void Func_Lore()
        {
            ButtonClicked = true;
            IsFirstClicked++;
            if (IsFirstClicked == 1)
                SceneToLoad = "000_Lore";
        }

        private void Func_Credits()
        {
            ButtonClicked = true;
            IsFirstClicked++;
            if (IsFirstClicked == 1)
                SceneToLoad = "00_Credits";
        }

        private IEnumerator InitCoroutine()
        {
            yield return new WaitForSeconds(1);
            AnimParameterReseter();
            _animator.SetBool("IdleToSitup", true);
        }

        public void EndIdleToSitup()
        {
            AnimParameterReseter();
            _animator.SetBool("Situp", true);
        }

        private void Update()
        {
            if (ButtonClicked)
            {
                AnimParameterReseter();
                _animator.SetBool("SitupToIdle", true);
            }
        }

        public void EndSitupToIdle()
        {
            AnimParameterReseter();
            _animator.SetBool("Idle", true);
            StartCoroutine(TransitionCoroutine());
        }

        private void AnimParameterReseter()
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("IdleToSitup", false);
            _animator.SetBool("Situp", false);
            _animator.SetBool("SitupToIdle", false);

        }

        private IEnumerator TransitionCoroutine()
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(SceneToLoad);
        }
    }
}
