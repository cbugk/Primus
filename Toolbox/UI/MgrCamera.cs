using System;
using System.Collections.Generic;
using UnityEngine;
using Primus.Toolbox.Scene;

namespace Primus.Toolbox.UI
{
    [System.Serializable]
    public class MgrCamera : MonoBehaviour
    {
        // Mouse-wheel returns +-120f so 0.01f is a good ZoomMultiplier.
        [field: SerializeField] public float ZoomMultiplierOrthographic { get; private set; }
        [field: SerializeField] public float ZoomMultiplierPerspective { get; private set; }
        [SerializeField] private List<Camera> _cameras;
        private int _indexActive;
        public int IndexActive { get => _indexActive; set => SwitchTo(value); }
        public Camera ActiveCamera
        {
            get
            {
                if (_indexActive == -1) { return null; }
                else { return _cameras[_indexActive]; }
            }
        }
        public Camera[] Cameras { get => _cameras.ToArray(); }
        public bool IsPaused { get; private set; }
        public bool IsActiveOrthographic { get => _cameras[_indexActive].orthographic; }
        public float ActiveOrthographicSize { get => _cameras[_indexActive].orthographicSize; }
        public float ActiveFieldOfView { get => _cameras[_indexActive].fieldOfView; }
        private List<AudioListener> _audioListeners;
        public bool CanCameraMove { get; set; }
        public float MoveMultiplier
        {
            get
            {
                return (IsActiveOrthographic ? ActiveOrthographicSize : ActiveFieldOfView * 2.0f) * 2.5f / 1080;
            }
        }

        private void Awake()
        {
            CanCameraMove = false;

            if (_cameras == null || _cameras.Count == 0)
            {
                Debug.LogError($"{name}: At least 1 Camera must be provided.");
                Destroy(this);
            }

            _audioListeners = new List<AudioListener>();

            if (0 < _cameras.Count)
            {
                _indexActive = 0;
                for (int index = 0; index < _cameras.Count; index++)
                {

                    if (_cameras[index] == null)
                    {
                        throw new System.ArgumentNullException("Cannot add null Camera.");
                    }

                    AudioListener audioListener = _cameras[index].GetComponent<AudioListener>();
                    if (audioListener == null)
                    {
                        throw new System.ArgumentNullException("Cannot add Camera with null AudioListener.");
                    }
                    _audioListeners.Add(audioListener);

                    if (index == _indexActive)
                    {
                        _cameras[index].gameObject.SetActive(true);
                        _audioListeners[index].enabled = true;
                    }
                    else
                    {
                        _audioListeners[index].enabled = false;
                        _cameras[index].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                _indexActive = -1;
            }
        }

        private void Update()
        {
            _cameras.Clear();
            _cameras.Add(Camera.main);
        }

        public void Add(Camera camera)
        {
            if (camera == null)
            {
                throw new System.ArgumentNullException("Cannot add null Camera.");
            }

            AudioListener audioListener = camera.GetComponent<AudioListener>();
            if (audioListener == null)
            {
                throw new System.ArgumentNullException("Cannot add Camera with null AudioListener.");
            }

            // Unless list was empty, IndexActive stays the same.
            if (_cameras.Count == 0)
            {
                _indexActive = 0;
            }

            _audioListeners.Add(audioListener);
            _cameras.Add(camera);

            _audioListeners[_audioListeners.Count].enabled = false;
            _cameras[_audioListeners.Count].gameObject.SetActive(false);
        }

        public void RemoveAt(int index)
        {
            if (ValidateIndex(index))
            {
                // Keep reference to ActiveCamera.
                Camera temp = ActiveCamera;

                // Remove specified camera.
                _cameras.RemoveAt(index);
                _audioListeners.RemoveAt(index);

                // Get current index of ActiveCamera.
                _indexActive = FindIndex(temp);
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid camera index.");
            }
        }

        public int FindIndex(Camera camera)
        {
            return _cameras.FindIndex(camera.Equals);
        }

        public void Move(float horizontal, float vertical)
        {
            if (!CanCameraMove)
            {
                return;
            }

            _cameras[_indexActive].transform.Translate((_cameras[_indexActive].transform.right * horizontal + _cameras[_indexActive].transform.up * vertical), Space.World);
        }

        public void MoveForward(float distance)
        {
            _cameras[_indexActive].transform.Translate(_cameras[_indexActive].transform.forward * distance, Space.World);
        }

        public void ZoomExponentialPerspective(float scaleFactor)
        {
            // divide-equal because as fieldOfView increases, more area is visible on screen.
            _cameras[_indexActive].fieldOfView /= scaleFactor;
        }

        public void ZoomExponentialOrthographic(float scaleFactor)
        {
            // divide-equal because as fieldOfView increases, more area is visible on screen.
            _cameras[_indexActive].orthographicSize /= scaleFactor;
        }

        public void ZoomLinearPerspective(float linearFactor, float clampMin, float clampMax)
        {
            linearFactor *= ZoomMultiplierPerspective;
            _cameras[_indexActive].fieldOfView -= linearFactor;
            _cameras[_indexActive].fieldOfView = Mathf.Clamp(_cameras[_indexActive].fieldOfView, clampMin, clampMax);
        }

        public void ZoomLinearOrthographic(float linearFactor, float clampMin, float clampMax)
        {
            linearFactor *= ZoomMultiplierOrthographic;
            _cameras[_indexActive].orthographicSize -= linearFactor;
            _cameras[_indexActive].orthographicSize = Mathf.Clamp(_cameras[_indexActive].orthographicSize, clampMin, clampMax);
        }

        private bool ValidateIndex(int index)
        {
            if (0 <= index || index < _cameras.Count)
            {
                return true;
            }

            return false;
        }
        private void SwitchTo(int index)
        {
            if (index == _indexActive) return;
            if (ValidateIndex(index))
            {

                _audioListeners[_indexActive].enabled = false;
                _cameras[_indexActive].gameObject.SetActive(false);

                _cameras[index].gameObject.SetActive(true);
                _audioListeners[index].enabled = true;

                _indexActive = index;
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid camera index.");
            }
        }
    }
}