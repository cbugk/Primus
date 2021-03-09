using UnityEngine;
using UnityEditor;

namespace Primus.Conveyor
{
    [CustomEditor(typeof(Conveyor))]
    public class ConveyorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Conveyor conveyor = (Conveyor) target;

            if (GUILayout.Button("Update"))
            {
                
            }
        }

    }
}
