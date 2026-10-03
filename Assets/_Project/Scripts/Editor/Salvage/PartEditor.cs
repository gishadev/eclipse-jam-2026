using gishadev.eclipse.Gameplay.Salvage;
using UnityEditor;
using UnityEngine;

namespace gishadev.eclipse.Editor.Salvage
{
    [CustomEditor(typeof(Part)), CanEditMultipleObjects]
    public class PartEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            if (GUILayout.Button("Collect Bolts From Children"))
                foreach (Object t in targets)
                    SalvageEditorUtility.FillFromChildren<Bolt>((Part)t, "bolts");
        }
    }
}
