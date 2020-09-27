using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using System;

using Object = UnityEngine.Object;

[CustomEditor(typeof(SpriteSwapper), true)]
public class SpriteSwapperHandler : OdinEditor {

    private SpriteSwapper mySpriteSwapper;

	void Awake() {
		mySpriteSwapper = (SpriteSwapper)target;
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		if(GUILayout.Button("Populate")) {
			Populate();
		}
	}

	public void Populate() {

		mySpriteSwapper.swapSets.Clear();
		Object[] originalAssets = GetAtPath(mySpriteSwapper.originalPath);
		List<Sprite> originalSprites = new List<Sprite>();
		List<Texture2D> originalTextures = new List<Texture2D>();
		foreach(Object obj in originalAssets) {
			if(obj is Sprite) {
				originalSprites.Add(obj as Sprite);
			}
			if(obj is Texture2D) {
				originalTextures.Add(obj as Texture2D);
			}
		}

		Object[] variantAssets = GetAtPath(mySpriteSwapper.variantPath);
		List<Sprite> variantSprites = new List<Sprite>();
		foreach(Object obj in variantAssets) {
			if(obj is Sprite) {
				variantSprites.Add(obj as Sprite);
			}
		}

		Debug.Log("Got " + originalTextures.Count + " textures.");
		Debug.Log("Got " + originalSprites.Count + "original sprites.");
		Debug.Log("Got " + variantSprites.Count + " variant sprites.");

		foreach(Texture2D tex in originalTextures) {
			Debug.Log("Processing swap set:" + tex.name);
			SwapSet swapSet = new SwapSet(tex);
			List<Sprite> spriteList = new List<Sprite>();
			foreach(Sprite sp in originalSprites) {
				if(swapSet.isPartOfSet(sp)) {
					spriteList.Add(sp);
				}
			}
			spriteList.Sort(CompareSprites); // Ensure sprites are sequenced.
			swapSet.original = spriteList.ToArray();

			List<Sprite> variantList = new List<Sprite>();
			foreach(Sprite sp in variantSprites) {
				if(swapSet.isPartOfSet(sp)) {
					variantList.Add(sp);
				}
			}
			variantList.Sort(CompareSprites); // Ensure sprites are sequenced.
			swapSet.variant = variantList.ToArray();

			mySpriteSwapper.swapSets.Add(swapSet);
		}
	}

	private static int CompareSprites(Sprite x, Sprite y) {
		int xIndex = x.name.LastIndexOf("_");
		int yIndex = y.name.LastIndexOf("_");
		int xVal = Convert.ToInt32(x.name.Substring(xIndex + 1));
		int yVal = Convert.ToInt32(y.name.Substring(yIndex + 1));
		return xVal - yVal;
	}


	public static Object[] GetAtPath(string path) {
		//Removing the excess /Assets of the datapath.
		string dataPath = Application.dataPath;
		int index = dataPath.LastIndexOf("/");
		if(index > 0)
			dataPath = dataPath.Substring(0, index);

		string[] fileEntries = Directory.GetFiles(dataPath + "/" + path);

		List<Object> al = new List<Object>();
		foreach(string fileName in fileEntries) {
			string lastPart = fileName;
			lastPart = lastPart.Replace(@"/", @"\");
			index = lastPart.LastIndexOf(@"\");
			lastPart = lastPart.Substring(index);
			string localPath = path;

			if(index > 0)
				localPath += lastPart;

			localPath = localPath.Replace(@"/", @"\");

			Object[] t = AssetDatabase.LoadAllAssetsAtPath(localPath);
			foreach(Object obj in t) {
				if(t != null) {
					al.Add(obj);
				}
			}
		}
		return al.ToArray();
	}

}