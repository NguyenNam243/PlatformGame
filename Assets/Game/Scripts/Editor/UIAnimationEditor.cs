using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIAnimation))]
public class UIAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        UIAnimation script = (UIAnimation)target;

        if (GUILayout.Button("Play Animtion"))
        {
            script.DoAnimation();
        }
    }
}
