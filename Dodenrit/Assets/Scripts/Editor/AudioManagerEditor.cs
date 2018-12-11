using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor {


    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        AudioManager manager = (AudioManager)target;

        if (GUILayout.Button("Get Folders"))
        {
            manager.LoadPaths();

        }
    }
}
