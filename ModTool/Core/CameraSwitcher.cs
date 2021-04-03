using UnityEngine;

namespace Primus.ModTool.Core
{

    [System.Serializable]
    public class CameraSwitcher
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera secondaryCamera;
        private AudioListener _mainAudioListener;

        private AudioListener _secondaryAudioListener;

        private bool _isSecondaryCamActive;

        internal void Initialize()
        {
            _isSecondaryCamActive = false;

            // Set mod camera if provided
            if (secondaryCamera)
            {
                _secondaryAudioListener = secondaryCamera.GetComponent<AudioListener>();
                _secondaryAudioListener.enabled = false;
                secondaryCamera.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Secondary Camera is not set.");
            }


            // Set main camera
            if (!mainCamera) mainCamera = Camera.main;
            _mainAudioListener = mainCamera.GetComponent<AudioListener>();
        }

        internal void Switch()
        {
            if (secondaryCamera)
            {
                if (_isSecondaryCamActive)
                {
                    mainCamera.gameObject.SetActive(true);
                    _mainAudioListener.enabled = true;
                    _secondaryAudioListener.enabled = false;
                    secondaryCamera.gameObject.SetActive(false);

                    _isSecondaryCamActive = false;
                }
                else
                {
                    secondaryCamera.gameObject.SetActive(true);
                    _secondaryAudioListener.enabled = true;
                    _mainAudioListener.enabled = false;
                    mainCamera.gameObject.SetActive(false);

                    _isSecondaryCamActive = true;
                }
            }
        }
    }
}