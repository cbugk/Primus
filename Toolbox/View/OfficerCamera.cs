using System;
using System.Collections.Generic;
using UnityEngine;
using Primus.Toolbox.Service;

namespace Primus.Toolbox.View
{
    [System.Serializable]
    public class OfficerCamera : MonoBehaviour
    {
        // Mouse-wheel returns +-120f so 0.01f is a good ZoomMultiplier.
        [field: SerializeField] public float ZoomMultiplierOrthographic { get; private set; }

        [field: SerializeField] public float ZoomMultiplierPerspective { get; private set; }

        /// <summary>Whether ActiveCamera is allowed to move.</summary>
        public bool CanActiveMove { get; set; }

        /// <summary>Identifier of the active camera, setting it changes camera.</summary>
        public int IndexActive { get => _indexActive; set => SwitchTo(value); }
        private int _indexActive;

        /// <summary>The camera viewing the scene.</summary>
        public Camera ActiveCamera { get => Cameras[_indexActive]; }

        /// <summary>Adjustes move speed.</summary>
        public float MoveMultiplier
        {
            get
            {
                if (ValidateIndex(_indexActive))
                {
                    return (ActiveCamera.orthographic ? ActiveCamera.orthographicSize : ActiveCamera.fieldOfView * 2.0f) * 2.5f / 1080;
                }
                else { return 0; }
            }
        }

        /// <summar>Currently available cameras.</summary>
        [field: SerializeField] public List<Camera> Cameras { get; private set; }
        private List<AudioListener> _audioListeners;

        private void Awake()
        {
            Cameras = new List<Camera>();
            _audioListeners = new List<AudioListener>();

            SetDefaults();
        }

        private void Start()
        {
            foreach (var camera in FindObjectsOfType<Camera>()) { Add(camera); }

            if (Cameras == null || Cameras.Count == 0)
            {
                Debug.LogError($"{name}: At least 1 Camera must be provided.");
                Destroy(this);
            }

            SwitchTo(0);
        }

        private void SetDefaults()
        {
            _indexActive = 0;
            CanActiveMove = false;
        }

        public void Add(Camera camera)
        {
            if (camera == null) { throw new System.ArgumentNullException("Cannot add null Camera."); }

            AudioListener audioListener = camera.GetComponent<AudioListener>();
            if (audioListener == null) { throw new System.ArgumentNullException("Cannot add Camera with null AudioListener."); }

            // Add first.
            Cameras.Add(camera);
            _audioListeners.Add(audioListener);

            // Count second.
            var cameraCount = Cameras.Count;
            bool isTheOnlyCamera = cameraCount == 1;

            // Unless list was empty, IndexActive stays the same.
            if (isTheOnlyCamera) { _indexActive = 0; }

            _audioListeners[cameraCount - 1].enabled = isTheOnlyCamera;
            Cameras[cameraCount - 1].gameObject.SetActive(isTheOnlyCamera);
        }

        public void RemoveAt(int index)
        {
            if (ValidateIndex(index))
            {
                // Keep reference to ActiveCamera.
                Camera temp = ActiveCamera;

                // Remove specified camera.
                Cameras.RemoveAt(index);
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
            return Cameras.FindIndex(camera.Equals);
        }

        public void Move(float horizontal, float vertical)
        {
            if (!CanActiveMove)
            {
                return;
            }

            horizontal *= MoveMultiplier;
            vertical *= MoveMultiplier;

            ActiveCamera.transform.Translate((Cameras[_indexActive].transform.right * horizontal + Cameras[_indexActive].transform.up * vertical), Space.World);
        }

        public void MoveForward(float distance)
        {
            ActiveCamera.transform.Translate(Cameras[_indexActive].transform.forward * distance, Space.World);
        }

        public void ZoomExponentialPerspective(float scaleFactor)
        {
            // divide-equal because as fieldOfView increases, more area is visible on screen.
            ActiveCamera.fieldOfView /= scaleFactor;
        }

        public void ZoomExponentialOrthographic(float scaleFactor)
        {
            // divide-equal because as fieldOfView increases, more area is visible on screen.
            ActiveCamera.orthographicSize /= scaleFactor;
        }

        public void ZoomLinearPerspective(float linearFactor, float clampMin, float clampMax)
        {
            linearFactor *= ZoomMultiplierPerspective;
            ActiveCamera.fieldOfView -= linearFactor;
            ActiveCamera.fieldOfView = Mathf.Clamp(Cameras[_indexActive].fieldOfView, clampMin, clampMax);
        }

        public void ZoomLinearOrthographic(float linearFactor, float clampMin, float clampMax)
        {
            linearFactor *= ZoomMultiplierOrthographic;
            ActiveCamera.orthographicSize -= linearFactor;
            ActiveCamera.orthographicSize = Mathf.Clamp(Cameras[_indexActive].orthographicSize, clampMin, clampMax);
        }

        private bool ValidateIndex(int index)
        {
            if (0 <= index || index < Cameras.Count) { return true; }
            return false;
        }
        private void SwitchTo(int index)
        {
            if (index == _indexActive) return;
            if (ValidateIndex(index))
            {

                _audioListeners[_indexActive].enabled = false;
                ActiveCamera.gameObject.SetActive(false);

                Cameras[index].gameObject.SetActive(true);
                _audioListeners[index].enabled = true;

                _indexActive = index;
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid camera index.");
            }
        }

        private void OnDestroy()
        {
            for (var index = 0; index < Cameras.Count; index++) { RemoveAt(0); }
        }
    }
}