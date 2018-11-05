using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Syateki
{

    public class Bgm : MonoBehaviour
    {

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayBgm()
        {
            audioSource.Play();
        }

        public void EndBgm()
        {
            audioSource.Stop();
        }
    }
}