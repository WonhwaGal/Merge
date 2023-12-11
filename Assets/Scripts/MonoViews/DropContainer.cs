using System;
using UnityEngine;
using System.Collections;

namespace Code.DropLogic
{
    public sealed class DropContainer : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _queueMoveDelay;
        private Camera _cam;
        private Vector3 _startPosition;
        private bool _isDragging;

        public DropObject CurrentDrop { get; set; }

        public event Func<Transform, DropObject> OnObjectDrop;

        private void Start()
        {
            _cam = Camera.main;
            _startPosition = transform.position;
        }

        private void OnMouseDrag()
        {
            if (Time.timeScale == 0)
                return;
            _isDragging = true;
            var currentMousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            CurrentDrop.transform.position = new Vector3(currentMousePos.x, transform.position.y, 0.0f);
        }

        private void OnMouseUp()
        {
            if (!_isDragging)
                return;
            CurrentDrop.Drop();
            transform.position = _startPosition;
            StartCoroutine(DelayQueueMove());
        }

        IEnumerator DelayQueueMove()
        {
            float delay = 0;
            while (delay < _queueMoveDelay)
            {
                if (Time.timeScale != 0)
                    delay += Time.deltaTime;
                yield return null;
            }

            CurrentDrop = OnObjectDrop?.Invoke(transform);
            _isDragging = false;
        }
    }
}