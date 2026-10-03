using gishadev.eclipse.Gameplay.Salvage;
using UnityEditor;
using UnityEngine;

namespace gishadev.eclipse.Editor.Salvage
{
    [CustomEditor(typeof(Vehicle)), CanEditMultipleObjects]
    public class VehicleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Collect Parts & Bolts From Children"))
                foreach (Object t in targets)
                    Collect((Vehicle)t);
        }

        // Recursive: parts of the vehicle, then bolts of each part.
        private static void Collect(Vehicle vehicle)
        {
            SalvageEditorUtility.FillFromChildren<Part>(vehicle, "parts");
            foreach (Part part in vehicle.GetComponentsInChildren<Part>(true))
                SalvageEditorUtility.FillFromChildren<Bolt>(part, "bolts");
        }
    }
}
