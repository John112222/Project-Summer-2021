using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeAnimate))]
public class MazeAnimateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeAnimate spawner = (MazeAnimate)target;
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
