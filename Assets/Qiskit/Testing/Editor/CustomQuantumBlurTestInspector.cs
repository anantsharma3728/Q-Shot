using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(QuantumBlurTest))]
//Custom Editor for the BazeMaze class, adding some buttons and a representation of the Maze
public class CustomQuantumBlurTestInspector : Editor {

    QuantumBlurTest targetTest;

    void OnEnable() {
        targetTest = target as QuantumBlurTest;
    }

    public override void OnInspectorGUI() {

        // Let the default inspecter draw all the values
        DrawDefaultInspector();

        // Spawn buttons

        if (GUILayout.Button("Do Direct Test"))
        {
            targetTest.DirectTest();
        }


        if (GUILayout.Button("Do indirect Test"))
        {
            targetTest.DoIndirectTest();
        }

        if (GUILayout.Button("Do Creator Test"))
        {
            targetTest.DoCreatorTest();
        }

        if (GUILayout.Button("Do Swap Test"))
        {
            targetTest.SwapTest();
        }

        if (GUILayout.Button("Save file"))
        {
            targetTest.SafeFile();
        }

        if (GUILayout.Button("Line test"))
        {
            targetTest.LineTest();
        }

    }





}