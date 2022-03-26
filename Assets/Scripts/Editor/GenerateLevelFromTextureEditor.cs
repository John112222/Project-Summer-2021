using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateLevelFromTexture))]
public class GenerateLevelFromTextureEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		GenerateLevelFromTexture spawner = (GenerateLevelFromTexture)target;
		if (GUILayout.Button("Generate"))
        {
            spawner.RunGenerator();
        }
		if (GUILayout.Button("Delete"))
        {
            spawner.ResetMaze();
        }
		
	}
}
