using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[AddComponentMenu("Fulcrum/Anim/SpriteSwapper")]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSwapper : SerializedMonoBehaviour {

	[SerializeField]
	[TableList(DrawScrollView = true, MaxScrollViewHeight = 200, MinScrollViewHeight = 100)]
	public List<SwapSet> swapSets = new List<SwapSet>();

	[SerializeField, FolderPath]
	public string originalPath;
	[SerializeField, FolderPath]
	public string variantPath;


	SpriteRenderer sr;
	private void Awake() {
		sr = this.GetComponent<SpriteRenderer>();
	}

	private void LateUpdate() {
		for(int i = 0; i < swapSets.Count; i++) {
			if(sr.sprite.texture == swapSets[i].tex) {
				for(int j = 0; j < swapSets[i].original.Length; j++) {
					if(sr.sprite == swapSets[i].original[j] && swapSets[i].variant.Length > 0) {
						int index = j % swapSets[i].variant.Length;
						sr.sprite = swapSets[i].variant[index];
					}
				}
			}
		}
	}



}

[Serializable]
public class SwapSet {

	//[TableColumnWidth(57, Resizable = true)]
	[ReadOnly]
	public string name;

	[PreviewField(64, ObjectFieldAlignment.Center), TableColumnWidth(64, Resizable = false)]
	public Texture2D tex;

	//[AssetList(CustomFilterMethod = "isPartOfSet", Path = @"Images/Platformer/")]
	public Sprite[] original;

	public Sprite[] variant;

	public SwapSet(Texture2D texture) {
		tex = texture;
		name = texture.name;
	}

	public bool isPartOfSet(Sprite sp) {
		return sp.texture.name == name;
	}
}

