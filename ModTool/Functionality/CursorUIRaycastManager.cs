using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace Primus.ModTool.Functionality
{
    public class CursorUIRaycastManager
    {
        private List<RaycastResult> _results;
        private GraphicRaycaster[] _graphicRaycasters;
        private PointerEventData _pointerEventData;
        private EventSystem _eventSystem;

        public RaycastResult[] CursorUIRaycastResults
        {
            get
            {
                SetCurrentResults();
                return _results.ToArray();
            }
        }

        public CursorUIRaycastManager()
        {
            _results = new List<RaycastResult>();
        }

        private void SetCurrentResults()
        {
            _results.Clear();
            _graphicRaycasters = UnityEngine.MonoBehaviour.FindObjectsOfType<GraphicRaycaster>();
            _eventSystem = EventSystem.current;

            //Set up the new Pointer Event
            _pointerEventData = new PointerEventData(_eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            _pointerEventData.position = Mouse.current.position.ReadValue();

            //Create a list of Raycast Results
            List<RaycastResult> resultsTemp = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            foreach (var graphicRaycaster in _graphicRaycasters)
            {
                graphicRaycaster.Raycast(_pointerEventData, resultsTemp);

                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in resultsTemp)
                {
                    _results.Add(result);
                }
            }
        }
    }
}