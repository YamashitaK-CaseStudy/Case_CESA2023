using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public partial class CStageEditor : EditorWindow
{
	private bool CreateObjInitialize(){
		LoadPrefabList();
		return true;
	}

	private void ObjectInst(string path, Vector3 pos){
		prefabData = AssetDatabase.LoadAssetAtPath<GameObject>(path);
		// プレハブからインスタンスを生成
		Debug.Log(pos);
		Debug.Log(prefabData);
		float size = prefabData.transform.localScale.x;
		for(int i = 0; i < setnum; i++){
			Vector3 setpos = pos;
			setpos.x = pos.x + size * i;
			Instantiate(prefabData, setpos, Quaternion.identity);
		}
	}

	private void LoadPrefabList(){
		// GUIDの検索
		string[] tmp = AssetDatabase.FindAssets("t:prefab", new string[]{path});
		prefabList = tmp.Select(tmp => AssetDatabase.GUIDToAssetPath(tmp)).ToArray();
		// nullCheck
		if(prefabListID == null) {
			prefabListID = prefabList;
		}

		// 名前の割り当て
		for(int i = 0; i < prefabList.Length; i++){
			prefabListID[i] = prefabList[i].Remove(0,path.Length);
		}

		return;
	}
}
