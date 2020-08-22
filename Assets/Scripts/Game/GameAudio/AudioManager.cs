using System.Collections.Generic;
using UnityEngine;

namespace GameAudio
{
    class AudioManager:MonoBehaviour
    {
        private Stack<AudioSource> m_sfxAudioPool = new Stack<AudioSource>();

        private List<AudioSource> m_activeSfx = new List<AudioSource>();

        [SerializeField]
        private AudioClip[] m_swapFailClips;

        [SerializeField]
        private AudioClip[] m_swapStartClips;

        [SerializeField]
        private AudioClip[] m_matchClips;

        [SerializeField]
        private AudioClip[] m_gameoverClips;

        public void PlayAudio(ESfxType sfxType)
        {
            AudioSource audioSource;

            if (m_sfxAudioPool.Count > 0)
            {
                audioSource = m_sfxAudioPool.Pop();
            }
            else
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.spatialBlend = 0;
            }

            AudioClip[] clips;

            switch (sfxType)
            {
                case ESfxType.SwapFail:
                    clips = m_swapFailClips;
                    break;
                case ESfxType.SwapStart:
                    clips = m_swapStartClips;
                    break;
                case ESfxType.Gameover:
                    clips = m_gameoverClips;
                    break;
                 default:
                     clips = m_matchClips;
                     break;
            }

            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
            m_activeSfx.Add(audioSource);
        }

        private void Update()
        {
            for (int i = 0; i < m_activeSfx.Count; i++)
            {
                AudioSource audioSource = m_activeSfx[i];

                if (!audioSource.isPlaying)
                {
                    m_activeSfx.RemoveAt(i);
                    m_sfxAudioPool.Push(audioSource);
                    i--;
                }
            }
        }
    }
}
