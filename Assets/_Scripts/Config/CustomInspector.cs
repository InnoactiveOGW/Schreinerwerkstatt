using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GameConfig))]
public class CustomInspector : Editor
{

    private void OnEnable()
    {
        update();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Save"))
        {
            update();
        }
    }

    private void update()
    {
        if (target.GetType() == typeof(GameConfig))
        {
            GameConfig getterSetter = (GameConfig)target;
            getterSetter.measureSizeFactor = getterSetter.measureSizeFactor_;
        }

    }


}