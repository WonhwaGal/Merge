using Code.Pools;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EffectList), menuName = "Scriptable/EffectList")]
public class EffectList : ScriptableObject
{
    [SerializeField] private List<EffectObject> _prefabs;

    public EffectObject FindType(PrefabType type)
    {
        var result = _prefabs.Find(x => x.Type == type);
        if(result == null)
            Debug.LogError($"{name} : object of type {type} was not found");
        return result;
    }

    [System.Serializable]
    public class EffectObject
    {
        public FXView View;
        public PrefabType Type;
    }
}