using System;
using UnityEngine;
using Primus.ModTool.Core;

namespace Primus.ModTool.Functionality
{
    [System.Serializable]
    public class CameraManager : IFunctionality
    {
        // Always the _cameras[0] is active.
        [SerializeField] private Camera[] _cameras;
        public Camera ActiveCamera { get => _cameras[0]; }
        public bool IsActiveOrthographic { get => _cameras[0].orthographic; }
        public float ActiveOrthographicSize { get => _cameras[0].orthographicSize; }
        public float ActiveFieldOfView { get => _cameras[0].fieldOfView; }
        private AudioListener[] _audioListeners;
        private Camera _temporaryCamera;
        private AudioListener _temporaryAudioListener;


        public CameraManager(Camera[] cameras)
        {
            _cameras = cameras;
        }

        public void ManualAwake()
        {
            if (_cameras == null || _cameras.Length == 0)
            {
                throw new System.Exception("No camera found while initializing.");
            }
            if (_cameras[0] == null)
            {
                throw new System.Exception("Active camera null while initializing.");
            }

            _audioListeners = new AudioListener[_cameras.Length];
            _audioListeners[0] = _cameras[0].GetComponent<AudioListener>();

            _audioListeners[0].enabled = true;
            _cameras[0].gameObject.SetActive(true);

            for (int index = 1; index < _cameras.Length; index++)
            {
                if (_cameras[index] == null)
                {
                    throw new System.Exception("Null camera found while initializing.");
                }
                _audioListeners[index] = _cameras[index].GetComponent<AudioListener>();

                _audioListeners[index].enabled = false;
                _cameras[index].gameObject.SetActive(false);
            }
        }

        public void SwitchTo(int cameraIndex)
        {
            if (cameraIndex == 0) return;
            if (cameraIndex < 1 || _cameras.Length <= cameraIndex)
            {
                throw new IndexOutOfRangeException("Invalid camera index.");
            }

            _temporaryCamera = _cameras[0];
            _temporaryAudioListener = _audioListeners[0];

            _cameras[0] = _cameras[cameraIndex];
            _audioListeners[0] = _audioListeners[cameraIndex];

            _cameras[cameraIndex] = _temporaryCamera;
            _audioListeners[cameraIndex] = _temporaryAudioListener;

            _cameras[cameraIndex].gameObject.SetActive(false);
            _audioListeners[cameraIndex].enabled = false;

            _cameras[0].gameObject.SetActive(true);
            _audioListeners[0].enabled = true;
        }

        public void Move(float horizontal, float vertical)
        {
            _cameras[0].transform.Translate((_cameras[0].transform.right * horizontal + _cameras[0].transform.up * vertical), Space.World);
        }

        public void MoveForward(float distance)
        {
            _cameras[0].transform.Translate(_cameras[0].transform.forward * distance, Space.World);
        }

        public void ZoomExponentialPerspective(float scaleFactor)
        {
            // divide-equal because as fieldOfView increases, more area is visible on screen.
            _cameras[0].fieldOfView /= scaleFactor;
        }

        public void ZoomExponentialOrthographic(float scaleFactor)
        {
            // divide-equal because as fieldOfView increases, more area is visible on screen.
            _cameras[0].orthographicSize /= scaleFactor;
        }

        public void ZoomLinearPerspective(float linearFactor, float clampMin, float clampMax)
        {
            _cameras[0].fieldOfView -= linearFactor;
            _cameras[0].fieldOfView = Mathf.Clamp(_cameras[0].fieldOfView, clampMin, clampMax);
        }

        public void ZoomLinearOrthographic(float linearFactor, float clampMin, float clampMax)
        {
            _cameras[0].orthographicSize -= linearFactor;
            _cameras[0].orthographicSize = Mathf.Clamp(_cameras[0].orthographicSize, clampMin, clampMax);
        }

    }
}