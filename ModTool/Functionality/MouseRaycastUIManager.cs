using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Primus.Core;

namespace Primus.ModTool.Functionality
{
    public class CursorUIRaycastManager : BaseMonoSingleton<CursorUIRaycastManager>
    {
        private GraphicRaycaster[] _graphicRaycasters;
        private PointerEventData _pointerEventData;
        private EventSystem _eventSystem;
        private List<RaycastResult> _results;
        public RaycastResult[] CursorUIRaycastResults
        {
            get
            {
                _results.Clear();
                _graphicRaycasters = FindObjectsOfType<GraphicRaycaster>();
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


                return _results.ToArray();
            }
        }
        protected override void Awake()
        {
            base.Awake();
            _results = new List<RaycastResult>();
        }
    }
}