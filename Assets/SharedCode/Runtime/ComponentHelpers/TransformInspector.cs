using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TransformInspector : MonoBehaviour {
 
}


#if UNITY_EDITOR
[CustomEditor(typeof(TransformInspector))]
public class TransformInspectorEditor : Editor
{
	TransformInspector script;
	void OnEnable()
	{
		script = target as TransformInspector;
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.Vector3Field ("Global Position", script.transform.position);
		EditorGUILayout.Vector3Field ("Global Eulers", script.transform.eulerAngles);
		EditorGUILayout.Vector3Field ("Lossy Scale", script.transform.lossyScale);
	}
}
#endif
