using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
public class AbstractComponentGenerator : EditorWindow {

    public string ComponentName = "DefaultComponent";
    public string NameSpace = "VaalsECS";
    public string BaseClass = "AbstractComponent";
    public string Folder = "Assets/Scripts/Components/";

    public List<string> variables = new List<string>();
    public int curIndex = 0;
    [MenuItem("Window/AbstractComponentGenerator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AbstractComponentGenerator));
    }

    public void OnGUI()
    {
        ComponentName = EditorGUILayout.TextField("ComponentName", ComponentName);
        NameSpace = EditorGUILayout.TextField("NameSpace", NameSpace);
        BaseClass = EditorGUILayout.TextField("BaseClass", BaseClass);
        Folder = EditorGUILayout.TextField("Folder", Folder);

        

        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Add Variable"))
        {
            AddVariable();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove Index Variable"))
        {
            if(variables.Count > 0)
            {
                variables.RemoveAt(curIndex);
                curIndex = Mathf.Max(0,variables.Count - 1);
            }

        }
        
        curIndex = EditorGUILayout.IntField("Index: ", curIndex);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("GenerateFile"))
        {
            GenerateFile();
        }

        for(int i=0;i < variables.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            variables[i] = EditorGUILayout.TextField("Variable " +i, variables[i]);

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
    public void RemoveLast()
    {
        variables.RemoveAt(variables.Count - 1);
    }

    public void AddVariable()
    {
        variables.Add("public float variableName = 10;\n");
    }

    public void GenerateFile()
    {
        string copyPath = Folder + ComponentName + ".cs";
        Debug.Log("Creating Classfile: " + copyPath);
        if (!File.Exists(copyPath))
        {
            using (StreamWriter outfile =
                new StreamWriter(copyPath))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using System.Collections;");
                outfile.WriteLine("");
                if(NameSpace.Length > 0)
                    outfile.WriteLine("namespace " + NameSpace + " \n{ ");
                outfile.WriteLine("\tpublic class " + ComponentName + " : " + BaseClass +"\n\t{");

                for(int i=0;i < variables.Count; i++)
                {
                    outfile.WriteLine("\t\t"+variables[i]);
                }


                outfile.WriteLine(" ");
                outfile.WriteLine(" ");
                //outfile.WriteLine(" // Use this for initialization");
                //outfile.WriteLine(" void Start () {");
                //outfile.WriteLine(" ");
                //outfile.WriteLine(" }");
                //outfile.WriteLine(" ");
                //outfile.WriteLine(" ");
                //outfile.WriteLine(" // Update is called once per frame");
                //outfile.WriteLine(" void Update () {");
                //outfile.WriteLine(" ");
                //outfile.WriteLine(" }");
                outfile.WriteLine("\t}");

                if (NameSpace.Length > 0)
                    outfile.WriteLine("\n}");
            }//File written
        }
        AssetDatabase.Refresh();
        
    }

}

