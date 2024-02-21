using UnityEngine;
using System.Collections;
using UnityEditor;
using Object = UnityEngine.Object;
using System.IO;
//[CanEditMultipleObjects()]
[CustomEditor(typeof(LanguageText), true)]
public class LanguageTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (EditorApplication.isCompiling)
        {
            GUI.enabled = false;
            base.OnInspectorGUI();
            return;
        }
        LanguageText ct = target as LanguageText;
        EditorGUI.BeginChangeCheck();
        int value = EditorGUILayout.IntField("ID", ct.ID);
        EditorGUI.EndChangeCheck();
        for (int i = 0; i < targets.Length; i++)
        {
            LanguageText control = targets[i] as LanguageText;
            if (control == null) { continue; }

            if (value != control.ID)
            {
                control.ID = value;

                control.Text = GetContentByID(value);
            }
        }

        SceneView.RepaintAll();
        EditorUtility.SetDirty(target);
    }

    string GetContentByID(int id)
    {
#if UNITY_EDITOR
        string value = string.Empty;
#if UNITY_EDITOR
        var data = Resources.Load<LanguageObject>("LanguageObject");
        for (int i = 0; i < data.LanguageList.Count; i++)
        {
            var item = data.LanguageList[i];
            if (id == item.Id)
            {
                return Config.GetLanguageText(item);
            }
        }
#endif
        return value;
#else
        return string.Empty;
#endif
    }
}
