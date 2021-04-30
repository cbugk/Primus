using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Primus.Core;

namespace Primus.ModTool.Functionality
{
    public class MouseRaycastUIManager : BaseMonoSingleton<MouseRaycastUIManager>
    {
        GraphicRaycaster[] _graphicRaycasters;
        PointerEventData _pointerEventData;
        EventSystem _eventSystem;

        public bool isHoveringButton;

        List<RaycastResult> results;
        protected override void Awake()
        {
            base.Awake();
            results = new List<RaycastResult>();
        }

        public RaycastResult[] GetGlobalRaycastUI()
        {
            results.Clear();
            _graphicRaycasters = FindObjectsOfType<GraphicRaycaster>();
            _eventSystem = EventSystem.current;
            isHoveringButton = false;
            //Check if the left Mouse button is clicked
            if (true)
            {
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
                        results.Add(result);
                    }
                }
            }

            return results.ToArray();
        }
    }
}