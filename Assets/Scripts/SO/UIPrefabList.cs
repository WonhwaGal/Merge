using Code.MVC;
using Code.DropLogic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(UIPrefabList), menuName = "Scriptable/UIPrefabList")]
public class UIPrefabList : ScriptableObject
{
    public DropContainer DropContainer;
    public PauseView LoseView;
}