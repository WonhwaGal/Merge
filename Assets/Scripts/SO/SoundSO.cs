using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Code.Sounds;

[CreateAssetMenu(fileName = nameof(SoundSO), menuName = "Scriptable/SoundSO")]
public class SoundSO : ScriptableObject
{
    public AudioMixer AudioMixer;
    public SoundObject SoundObject;

    [SerializeField] private List<ClipObject> _clips;

    public AudioClip FindClip(SoundType type)
    {
        var result = _clips.Find(x => x.Type == type);
        if (result == null)
            Debug.LogError($"{name} : clip of type {type} was not found");
        return result.Clip;
    }

    [System.Serializable]
    private class ClipObject
    {
        public SoundType Type;
        public AudioClip Clip;
    }
}