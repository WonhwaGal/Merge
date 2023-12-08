using System;
using UnityEngine;

namespace Code.Views
{
    public sealed class DropContainer : MonoBehaviour
    {
        private Camera _cam;
        private Vector3 _startPosition;

        public DropObject CurrentDrop { get; set; }

        public event Func<Transform, DropObject> OnObjectDrop;

        private void Start()
        {
            _cam = Camera.main;
            _startPosition = transform.position;
        }

        private void OnMouseDrag()
        {
            var currentMousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(currentMousePos.x, transform.position.y, 0.0f);
        }

        private void OnMouseUp()
        {
            CurrentDrop.Drop();
            transform.position = _startPosition;
            CurrentDrop = OnObjectDrop?.Invoke(transform);
        }
    }
}