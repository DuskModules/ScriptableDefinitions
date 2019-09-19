using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DuskModules.ScriptingDefinitions {

	/// <summary> Utility class with static methods with which the Scripting Define Symbols can be adjusted. </summary>
	public class DefineUtility {

		/// <summary> Amount of build target groups. </summary>
		private const int buildTargetGroupCount = 11;

		/// <summary> Returns whether the symbol is contained within the scripting define symbol for all build target groups. </summary>
		/// <param name="symbol"> The symbol to check </param>
		/// <returns> Whether the symbol is contained within </returns>
		public static bool ContainsDefineSymbol(string symbol) {
			for (int i = 0; i < buildTargetGroupCount; i++) {
				if (ContainsDefineSymbol(symbol, GetBuildTargetGroup(i)))
					return true;
			}
			return false;
		}
		/// <summary> Returns whether the symbol is contained within the scripting define symbol for the build target group. </summary>
		/// <param name="symbol"> The symbol to check </param>
		/// <param name="buildTargetGroup"> What group to target </param>
		/// <returns> Whether the symbol is contained within </returns>
		public static bool ContainsDefineSymbol(string symbol, BuildTargetGroup buildTargetGroup) {
			string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			return symbols.Contains(symbol);
		}

		/// <summary> Clears the list of scripting define symbols for all build target groups. </summary>
		public static void ClearDefineSymbols() {
			for (int i = 0; i < buildTargetGroupCount; i++) {
				ClearDefineSymbols(GetBuildTargetGroup(i));
			}
		}
		/// <summary> Clears the list of scripting define symbols for the build target group. </summary>
		/// <param name="buildTargetGroup"> What group to target </param>
		public static void ClearDefineSymbols(BuildTargetGroup buildTargetGroup) {
			PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, "");
		}

		/// <summary> Adds a symbol to the scripting define symbols for all build target groups. </summary>
		/// <param name="symbol"> The symbol to add </param>
		public static bool AddDefineSymbol(string symbol) {
			EditorUtility.DisplayProgressBar("Adding Define Symbol", "Starting", 0);
			bool change = false;
			for (int i = 0; i < buildTargetGroupCount; i++) {
				EditorUtility.DisplayProgressBar("Adding Define Symbol", GetBuildTargetGroup(i).ToString(), (float)i / (float)buildTargetGroupCount);
				if (AddDefineSymbol(symbol, GetBuildTargetGroup(i)))
					change = true;
			}
			EditorUtility.ClearProgressBar();
			return change;
		}
		/// <summary> Adds a symbol to the scripting define symbols for the build target groups. </summary>
		/// <param name="symbol"> The symbol to add </param>
		/// <param name="buildTargetGroup"> What group to target </param>
		public static bool AddDefineSymbol(string symbol, BuildTargetGroup buildTargetGroup) {
			if (buildTargetGroup == BuildTargetGroup.Unknown) return false;

			string symbolsLine = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			List<string> symbols = new List<string>(symbolsLine.Split(';'));
			if (!symbols.Contains(symbol)) {
				symbols.Add(symbol);
				symbolsLine = CombineDefineSymbols(symbols);

				PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbolsLine);
				return true;
			}
			return false;
		}

		/// <summary> Removes a symbol to the scripting define symbols for all build target groups. </summary>
		/// <param name="symbol"> The symbol to remove </param>
		public static bool RemoveDefineSymbol(string symbol) {
			EditorUtility.DisplayProgressBar("Removing Define Symbol", "Starting", 0);
			bool change = false;
			for (int i = 0; i < buildTargetGroupCount; i++) {
				EditorUtility.DisplayProgressBar("Removing Define Symbol", GetBuildTargetGroup(i).ToString(), (float)i / (float)buildTargetGroupCount);
				if (RemoveDefineSymbol(symbol, GetBuildTargetGroup(i)))
					change = true;
			}
			EditorUtility.ClearProgressBar();
			return change;
		}
		/// <summary> Removes a symbol to the scripting define symbols for the build target groups. </summary>
		/// <param name="symbol"> The symbol to remove </param>
		/// <param name="buildTargetGroup"> What group to target </param>
		public static bool RemoveDefineSymbol(string symbol, BuildTargetGroup buildTargetGroup) {
			if (buildTargetGroup == BuildTargetGroup.Unknown) return false;

			string symbolsLine = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			List<string> symbols = new List<string>(symbolsLine.Split(';'));

			if (symbols.Contains(symbol)) {
				symbols.Remove(symbol);
				symbolsLine = CombineDefineSymbols(symbols);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbolsLine);
				return true;
			}
			return false;
		}

		/// <summary> Modifies the symbols list more performance friendly </summary>
		/// <param name="add"> Symbols to add </param>
		/// <param name="remove"> Symbols to remove </param>
		public static bool ModifyDefineSymbols(List<string> add, List<string> remove) {
			EditorUtility.DisplayProgressBar("Modifying Define Symbols", "Starting", 0);

			bool change = false;
			for (int i = 0; i < buildTargetGroupCount; i++) {
				EditorUtility.DisplayProgressBar("Modifying Define Symbols", GetBuildTargetGroup(i).ToString(), (float)i / (float)buildTargetGroupCount);
				if (ModifyDefineSymbols(add, remove, GetBuildTargetGroup(i)))
					change = true;
			}
			EditorUtility.ClearProgressBar();
			return change;
		}

		/// <summary> Modifies the symbols list more performance friendly </summary>
		/// <param name="add"> Symbols to add </param>
		/// <param name="remove"> Symbols to remove </param>
		/// <param name="buildTargetGroup"> What group to target </param>
		public static bool ModifyDefineSymbols(List<string> add, List<string> remove, BuildTargetGroup buildTargetGroup) {
			if (buildTargetGroup == BuildTargetGroup.Unknown) return false;

			string symbolsLine = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
			List<string> symbols = new List<string>(symbolsLine.Split(';'));
			bool change = false;

			for (int i = 0; i < add.Count; i++) {
				if (!symbols.Contains(add[i])) {
					symbols.Add(add[i]);
					change = true;
				}
			}

			for (int i = 0; i < remove.Count; i++) {
				if (symbols.Contains(remove[i])) {
					symbols.Remove(remove[i]);
					change = true;
				}
			}

			if (change) {
				symbolsLine = CombineDefineSymbols(symbols);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbolsLine);
			}
			return change;
		}


		/// <summary> Combine define symbols into one string </summary>
		protected static string CombineDefineSymbols(List<string> symbols) {
			string symbolsLine = "";
			for (int i = 0; i < symbols.Count; i++) {
				symbolsLine += symbols[i];
				if (i < symbols.Count - 1)
					symbolsLine += ";";
			}
			return symbolsLine;
		}

		/// <summary> Converts i into useful build target group, useful for looping through all. </summary>
		private static BuildTargetGroup GetBuildTargetGroup(int i) {
			switch (i) {
				case 0: return BuildTargetGroup.Android;
				case 1: return BuildTargetGroup.Facebook;
				case 2: return BuildTargetGroup.iOS;
				case 3: return BuildTargetGroup.Lumin;
				case 4: return BuildTargetGroup.PS4;
				case 5: return BuildTargetGroup.Standalone;
				case 6: return BuildTargetGroup.Switch;
				case 7: return BuildTargetGroup.tvOS;
				case 8: return BuildTargetGroup.WebGL;
				case 9: return BuildTargetGroup.WSA;
				case 10: return BuildTargetGroup.XboxOne;
			}
			return BuildTargetGroup.Unknown;
		}

	}
}