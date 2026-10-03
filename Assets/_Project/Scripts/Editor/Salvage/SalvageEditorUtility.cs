using UnityEditor;
using UnityEngine;

namespace gishadev.eclipse.Editor.Salvage
{
    internal static class SalvageEditorUtility
    {
        /// <summary>
        /// Fills a serialized array field on <paramref name="owner"/> with every <typeparamref name="T"/>
        /// in its hierarchy (inactive included). Goes through SerializedObject, so it's undoable and marks
        /// the scene/prefab dirty.
        /// </summary>
        public static void FillFromChildren<T>(Component owner, string arrayField) where T : Component
        {
            T[] found = owner.GetComponentsInChildren<T>(true);

            var serializedOwner = new SerializedObject(owner);
            SerializedProperty array = serializedOwner.FindProperty(arrayField);
            array.arraySize = found.Length;
            for (int i = 0; i < found.Length; i++)
                array.GetArrayElementAtIndex(i).objectReferenceValue = found[i];
            serializedOwner.ApplyModifiedProperties();
        }
    }
}
