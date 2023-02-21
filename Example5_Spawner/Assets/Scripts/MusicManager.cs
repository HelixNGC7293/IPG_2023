using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eddy.Utilities
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance;

        AudioSource source;

        // Start is called before the first frame update
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }

            source = GetComponent<AudioSource>();
        }

        public void SwitchMusic(AudioClip aClip)
        {
            source.clip = aClip;
            source.Play();
        }
    }
}
