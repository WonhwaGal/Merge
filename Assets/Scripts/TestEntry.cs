using Code.Views;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    [SerializeField] private DropContainer _container;
    [SerializeField] private DropObjectSO _so;
    private DropService _dropService;

    private void Awake()
    {
        _dropService = new DropService(_so);
        _container.OnObjectDrop += _dropService.CreateDropObject;
        _container.CurrentDrop = _dropService.CreateDropObject(_container.transform);
    }
}