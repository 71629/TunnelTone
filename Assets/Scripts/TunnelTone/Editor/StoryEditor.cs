using System.IO;
using System.Reflection;
using TunnelTone.StorySystem;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TunnelTone.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Story))]
    public class StoryEditor : UnityEditor.Editor
    {
        private ReorderableList list;
        private SerializedProperty timeline;
        
        private struct ElementCreationParams {
            public string Path;
        }
        
        private void OnEnable()
        {
            timeline = serializedObject.FindProperty("timeline");
            
            list = new( serializedObject, timeline, true, true, true, true);

            list.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Timeline");
            };
            
            list.onRemoveCallback = (ReorderableList l) => {
                var element = l.serializedProperty.GetArrayElementAtIndex(l.index); 
                var obj = element.objectReferenceValue;

                AssetDatabase.RemoveObjectFromAsset(obj);

                DestroyImmediate(obj, true);
                l.serializedProperty.DeleteArrayElementAtIndex(l.index);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            };
            
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
                SerializedProperty element = timeline.GetArrayElementAtIndex(index);

                rect.y += 2;
                rect.width -= 10;
                rect.height = EditorGUIUtility.singleLineHeight;

                if (element.objectReferenceValue == null) {
                    return;
                }
                string label = element.objectReferenceValue.name;
                EditorGUI.LabelField(rect, label, EditorStyles.boldLabel);

                // Convert this element's data to a SerializedObject so we can iterate
                // through each SerializedProperty and render a PropertyField.
                SerializedObject nestedObject = new SerializedObject(element.objectReferenceValue);

                // Loop over all properties and render them
                SerializedProperty prop = nestedObject.GetIterator();
                float y = rect.y;
                while (prop.NextVisible(true)) {
                    if (prop.name is "m_Script") {
                        continue;
                    }
                    
                    FieldInfo field = element.objectReferenceValue.GetType().GetField(prop.name);
                    if (field is not null)
                    {
                        var attributes = field.GetCustomAttributes(typeof(TextAreaAttribute), false);
                        if (attributes.Length > 0)
                        {
                            var textAreaAttribute = (TextAreaAttribute)attributes[0];
                            rect.height = EditorGUIUtility.singleLineHeight * textAreaAttribute.minLines;
                        }
                    }

                    rect.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(rect, prop);
                }

                nestedObject.ApplyModifiedProperties();

                // Mark edits for saving
                if (GUI.changed) {
                    EditorUtility.SetDirty(target);
                }
            };
            
            list.elementHeightCallback = (int index) => {
                float baseProp = EditorGUI.GetPropertyHeight(
                    list.serializedProperty.GetArrayElementAtIndex(index), true);

                float additionalProps = 0;
                SerializedProperty element = timeline.GetArrayElementAtIndex(index);
                if (element.objectReferenceValue != null) {
                    SerializedObject ability = new SerializedObject(element.objectReferenceValue);
                    SerializedProperty prop = ability.GetIterator();
                    while (prop.NextVisible(true)) {
                        // XXX: This logic stays in sync with loop in drawElementCallback.
                        if (prop.name == "m_Script") {
                            continue;
                        }
                        FieldInfo field = element.objectReferenceValue.GetType().GetField(prop.name);
                        var attributes = field.GetCustomAttributes(typeof(TextAreaAttribute), false);
                        if (attributes.Length > 0)
                        {
                            var textAreaAttribute = (TextAreaAttribute)attributes[0];
                            additionalProps += EditorGUIUtility.singleLineHeight * textAreaAttribute.minLines;
                        }
                        
                        additionalProps += EditorGUIUtility.singleLineHeight;
                    }
                }

                float spacingBetweenElements = EditorGUIUtility.singleLineHeight / 2;

                return baseProp + spacingBetweenElements + additionalProps;
            };
            
            list.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {
                var menu = new GenericMenu();
                var guids = AssetDatabase.FindAssets("", new[]{"Assets/Scripts/TunnelTone/StorySystem"});
                foreach (var guid in guids) {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var type = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
                    if (type.name == "StoryElement") {
                        continue;
                    }

                    menu.AddItem(
                        new GUIContent(Path.GetFileNameWithoutExtension(path)),
                        false,
                        AddClickHandler,
                        new ElementCreationParams() {Path = path});
                }
                menu.ShowAsContext();
            };
        }
        
        private void AddClickHandler(object dataObj) {
            // Make room in list
            var data = (ElementCreationParams)dataObj;
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);

            // Create the new Ability
            var type = AssetDatabase.LoadAssetAtPath(data.Path, typeof(UnityEngine.Object));
            var newElement = CreateInstance(type.name);
            newElement.name = type.name;

            // Add it to Story
            var elementData = (Story)target;
            AssetDatabase.AddObjectToAsset(newElement, elementData);
            AssetDatabase.SaveAssets();
            element.objectReferenceValue = newElement;
            serializedObject.ApplyModifiedProperties();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}