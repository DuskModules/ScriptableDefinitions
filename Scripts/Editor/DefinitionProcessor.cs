#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.ScriptingDefinitions.DuskEditor {

	/// <summary> Checks if DefinitionObjects are added or deleted. </summary>
	public class DefinitionProcessor : AssetPostprocessor {

		private const string prefsKey = "scriptingDefinitions";

		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
			foreach (string str in importedAssets) {
				DefinitionObject definition = AssetDatabase.LoadAssetAtPath<DefinitionObject>(str);
				if (definition != null) {
					UpdateDefinitions();
					return;
				}
			}
			foreach (string str in deletedAssets) {
				if (str.Contains(".asset")) {
					int start = str.LastIndexOf("/") + 1;
					int end = str.IndexOf(".asset");
					string name = str.Substring(start, end - start);

					// Check if it is contained
					if (DefineUtility.ContainsDefineSymbol(name.ToUpper())) {
						UpdateDefinitions();
						return;
					}
				}
			}
		}

		/// <summary> Gets all DefinitionObjects in the AssetDatabase. Pushes any changes to DefineUtility </summary>
		public static void UpdateDefinitions() {
			string[] guids = AssetDatabase.FindAssets("t:DefinitionObject");
			List<DefinitionObject> defObjects = new List<DefinitionObject>();
			for (int i = 0; i < guids.Length; i++) {
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				defObjects.Add(AssetDatabase.LoadAssetAtPath<DefinitionObject>(path));
			}

			string definitions = EditorPrefs.GetString(prefsKey, "");
			List<string> savedDefs = new List<string>(definitions.Split(';'));

			// Handle those that exist. Added to new save file and define utility.
			string newSave = "";
			for (int i = 0; i < defObjects.Count; i++) {

				// Convert name into something legit
				string defName = defObjects[i].name.ToUpper(); ;
				char[] chars = defName.ToCharArray();
				for (int c = 0; c < chars.Length; c++) {
					if (!IsAllowedCharacter(chars[c])) {
						chars[c] = '_';
					}
				}
				defName = new string(chars);
				defName = defName.Replace("__", "_");
				defObjects[i].name = defName;

				// Add define symbol
				DefineUtility.AddDefineSymbol(defName);

				// Save, but no duplicates
				if (!newSave.Contains(defName)) {
					if (newSave != "") newSave += ";";
					newSave += defName;
				}

				// Prevent removal
				if (savedDefs.Contains(defName)) {
					savedDefs.Remove(defName);
				}
			}

			// Handle all those saved, which are no longer in the assets. They are gone.
			for (int i = 0; i < savedDefs.Count; i++) {
				if (savedDefs[i] != "") {
					DefineUtility.RemoveDefineSymbol(savedDefs[i]);
				}
			}
			
			EditorPrefs.SetString(prefsKey, newSave);
		}

		/// <summary> Check whether the character is allowed or not </summary>
		private static bool IsAllowedCharacter(char c) {
			switch (c) {
				case 'A': return true;
				case 'B': return true;
				case 'C': return true;
				case 'D': return true;
				case 'E': return true;
				case 'F': return true;
				case 'G': return true;
				case 'H': return true;
				case 'I': return true;
				case 'J': return true;
				case 'K': return true;
				case 'L': return true;
				case 'M': return true;
				case 'N': return true;
				case 'O': return true;
				case 'P': return true;
				case 'Q': return true;
				case 'R': return true;
				case 'S': return true;
				case 'T': return true;
				case 'U': return true;
				case 'V': return true;
				case 'W': return true;
				case 'X': return true;
				case 'Y': return true;
				case 'Z': return true;
				case '0': return true;
				case '1': return true;
				case '2': return true;
				case '3': return true;
				case '4': return true;
				case '5': return true;
				case '6': return true;
				case '7': return true;
				case '8': return true;
				case '9': return true;
			}
			return false;
		}

	}
	
}
#endif