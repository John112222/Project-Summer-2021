using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeSpawner))]
public class MazeInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeSpawner spawner = (MazeSpawner)target;
        if (GUILayout.Button("Create Maze"))
        {
            spawner.CreateMaze();
        }
        if (GUILayout.Button("Delete All Maze"))
        {
            spawner.ResetMaze();
        }
    }
}
