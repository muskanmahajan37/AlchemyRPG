using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextureCreator))]
public class TextureCreatorInspector : Editor {

  private TextureCreator textureCreator;

  private void OnEnable() {
    textureCreator = target as TextureCreator;
    Undo.undoRedoPerformed += RefreshCreator;
  }

  private void OnDisable() {
    Undo.undoRedoPerformed -= RefreshCreator;
  }

  private void RefreshCreator() {
    if (Application.isPlaying) {
      textureCreator.FillTexture();
    }
  }

	public override void OnInspectorGUI () {
		EditorGUI.BeginChangeCheck();
		DrawDefaultInspector();
		if (EditorGUI.EndChangeCheck() && Application.isPlaying) {
			(target as TextureCreator).FillTexture();
		}
	}
}
