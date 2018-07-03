using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using Gonity.Core;

namespace Gonity.Profiler
{
    [CustomEditor(typeof(ECSProfiler))]
    public class ECSProfilerInspector : Editor
    {
        protected static bool showGeneralSettings = true;

        public override void OnInspectorGUI()
        {
            ECSProfiler taskProfilerBehaviour = (ECSProfiler)target;
            IEntityDatabaseProfiled database = taskProfilerBehaviour.entityDatabase as IEntityDatabaseProfiled;

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                if (database != null)
                {
                    DrawComponents(database.componentList);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Entities", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                if (database != null)
                {
                    DrawEntities(database.entities);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();

            EditorUtility.SetDirty(target);
        }

        private void DrawComponents(Dictionary<Type, IComponentList> componentList)
        {
            var enumerator = componentList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                EditorGUILayout.LabelField(string.Format("{0} : {1}", enumerator.Current.Key, enumerator.Current.Value.Count));
            }
        }

        private void DrawEntities(List<Entity> entities)
        {
            var enumerator = entities.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IEntityProfiled entityProfiled = enumerator.Current as IEntityProfiled;
                string title = "Entity";
                if (entityProfiled.tags.Count > 0)
                {
                    title += "(";
                    for (int i = 0; i < entityProfiled.tags.Count; ++i)
                    {
                        title += entityProfiled.tags[i];
                        if (i < entityProfiled.tags.Count - 1)
                        {
                            title += ",";
                        }
                    }
                    title += ")";
                }

                showGeneralSettings = EditorGUILayout.Foldout(showGeneralSettings, title);
                if (showGeneralSettings)
                {
                    EditorGUI.indentLevel++;
                    
                    var componentEnumerator = entityProfiled.components.GetEnumerator();
                    while (componentEnumerator.MoveNext())
                    {
                        EditorGUILayout.LabelField(string.Format("{0}", componentEnumerator.Current.Key));
                    }
                    EditorGUI.indentLevel--;
                }

            }
        }
    }
}
