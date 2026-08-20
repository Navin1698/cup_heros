using UnityEngine;

namespace OrbRaiders.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public void PlaySFX(AudioClip clip, float volume = 1.0f, float pitchVariance = 0.1f)
        {
            if (clip == null || sfxSource == null) return;
            if (Save.SaveManager.Instance != null && !Save.SaveManager.Instance.CurrentData.SFXEnabled) return;

            sfxSource.pitch = 1.0f + Random.Range(-pitchVariance, pitchVariance);
            sfxSource.PlayOneShot(clip, volume);
        }

        public void PlayMusic(AudioClip musicClip)
        {
            if (musicSource == null) return;
            if (Save.SaveManager.Instance != null && !Save.SaveManager.Instance.CurrentData.MusicEnabled) return;

            if (musicSource.clip == musicClip && musicSource.isPlaying) return;

            musicSource.clip = musicClip;
            musicSource.Play();
        }

        public void StopMusic()
        {
            if (musicSource != null) musicSource.Stop();
        }
    }
}
