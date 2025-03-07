using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MountAndBlade
{
    public class asd : MonoBehaviour
    {

        private Animator animator;
        public float val = 0.0f;

        void Start()
        {
        animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            val += Time.deltaTime;
            animator.SetFloat("Velocity", val);
            if (val == 1.1f)
                val = 0.0f;
        }
    }
}
