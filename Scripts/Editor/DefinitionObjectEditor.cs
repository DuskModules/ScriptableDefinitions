#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.ScriptingDefinitions.DuskEditor {

	/// <summary> Editor for game event </summary>
	[CustomEditor(typeof(DefinitionObject))]
	[CanEditMultipleObjects]
	public class DefinitionObjectEditor : Editor {

		/// <summary> Inspector GUI </summary>
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			EditorGUILayout.HelpBox("When a Definition Object is imported into a Unity Project, it adds it's own name as a Scripting Definition Symbol. " +
				"When it's removed, it removes the symbol.", MessageType.Info);
		}
	}
}
#endif