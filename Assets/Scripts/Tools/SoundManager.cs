using UnityEngine;
using GamePush;

namespace Code.Sounds
{
    public class SoundManager
    {
        private readonly SoundSO _soundSO;
        private readonly AudioSource _musicSource;
        private readonly AudioSource _soundSource;

        public SoundManager(SoundSO soundSO)
        {
            _soundSO = soundSO;
            GameEventSystem.Subscribe<SoundEvent>(PlaySound);
            var soundView = GameObject.Instantiate(_soundSO.SoundObject);
            _musicSource = soundView.MusicSource;
            _soundSource = soundView.SoundSource;
            //_soundSO.AudioMixer.SetFloat("Music", GP_Player.GetFloat("MusicLevel"));
            _musicSource.clip = _soundSO.FindClip(SoundType.BackGround);
            _musicSource.Play();
        }

        private void PlaySound(SoundEvent @event)
        {
            if (@event.SoundType == SoundType.BackGround)
                HandleMusic(@event.TurnOn);
            else
                HandleSounds(@event);
        }

        private void HandleMusic(bool turnOn)
        {
            if (turnOn)
                _musicSource.Play();
            else
                _musicSource.Stop();
        }

        private void HandleSounds(SoundEvent @event)
        {
            if (@event.TurnOn)
                _soundSource.PlayOneShot(_soundSO.FindClip(@event.SoundType));
        }
    }
}