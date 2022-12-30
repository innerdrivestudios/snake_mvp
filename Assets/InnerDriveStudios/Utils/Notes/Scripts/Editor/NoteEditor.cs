using UnityEngine;
using UnityEditor;

namespace InnerDriveStudios.Util
{
    /**
     * Defines the editor for the Note component which is 
     * a simple sticky note like component through which you
     * can document your scene.
     */
    [CustomEditor(typeof(Note))]
    public class NoteEditor : Editor
    {
        private static readonly GUILayoutOption[] TEXT_AREA_OPTIONS = {
            GUILayout.ExpandWidth (true),
            GUILayout.ExpandHeight(false)
        };

        private static readonly string[] DESCRIPTIONS = { "Documentation", "Todo", "Nice to have", "Minor bug", "Critical bug" };
        private static readonly Color[] COLORS = {
            new Color(1, 1, 0.4f),
            new Color(1, 0.75f, 0.3f),
            new Color(1, 0.5f, 0.2f),
            new Color(1, 0.25f, 0.1f),
            new Color(1, 0, 0)
        };

        private static GUIStyle[] noteStyles = null;
        private static bool backgroundTexturesInitialized = false;

        private static void GenerateBackgroundTexturesForDescriptions()
        {
            if (backgroundTexturesInitialized) return;

            noteStyles = new GUIStyle[DESCRIPTIONS.Length];
            noteStyles = new GUIStyle[DESCRIPTIONS.Length];

            for (int i = 0; i < DESCRIPTIONS.Length; i++)
            {
                Texture2D texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, COLORS[i]);
                texture.Apply();

                GUIStyle style = new GUIStyle();
                style.normal.background = texture;
                style.wordWrap = true;
                style.fontSize = 12;
                style.padding = new RectOffset(5, 5, 5, 5);
                style.margin = new RectOffset(5, 5, 5, 5);

                noteStyles[i] = style;
            }
        }

        SerializedProperty noteType;
        SerializedProperty noteText;

        private void OnEnable()
        {
            GenerateBackgroundTexturesForDescriptions();

            noteType = serializedObject.FindProperty("noteType");
            noteText = serializedObject.FindProperty("noteText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            noteType.intValue = EditorGUILayout.Popup(noteType.intValue, DESCRIPTIONS);
            noteText.stringValue = EditorGUILayout.TextArea(noteText.stringValue, noteStyles[noteType.intValue], TEXT_AREA_OPTIONS);

            serializedObject.ApplyModifiedProperties();
        }

    }

}