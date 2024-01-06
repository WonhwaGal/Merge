using UnityEngine;

namespace Code.Sounds
{
    public class SoundObject : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;

        public AudioSource MusicSource => _musicSource;
        public AudioSource SoundSource => _soundSource;

        private void Awake() => DontDestroyOnLoad(this);
    }
}