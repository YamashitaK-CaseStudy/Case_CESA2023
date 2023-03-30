using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public partial class CStageEditor : EditorWindow{

	private string path = "Assets/Prefabs/Stage/";	// 検索するファイル
	private Vector2 prefabListScPos;	// プレハブの一覧用のバー座標
	private Vector2 previewRunScPos;	// プレビュー用のバー座標
	private GameObject prefabData;		// プレハブのデータ
	string[] prefabList;				// プレハブの一覧
	string[] prefabListID;				// プレハブの一覧
	string selectPrefabPath;			// 選択されたプレハブのパス
	string selectPrefabID;				//　選択されたプレハブのID
	Vector3 pos = new Vector3(0,0,0);
	private bool isMultiple = false; 	// 同時設置するかどうか
	private bool isMultipleX,isMultipleY,isMultipleZ;		// 同時設置
	private bool isFrnMultiX,isFrnMultiY,isFrnMultiZ;	// 前設置
	private bool isInvMultiX,isInvMultiY,isInvMultiZ;		// 逆設置
	int instNumX = 0,instNumY = 0,instNumZ = 0;

	private bool CreateObjInitialize(){
		LoadPrefabList();
		return true;
	}
	// プレハブのレイアウト
	private void LayoutPrefabList(){
		// プレハブの再読み込みボタン
		using (new GUILayout.HorizontalScope()){
			if(GUILayout.Button("Prefab Reload")) LoadPrefabList();
			// プレハブの一覧表示
			using(GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox, GUILayout.Width(200))){
				EditorGUILayout.LabelField("Prefab List");
				prefabListScPos = scroll.scrollPosition;

				// プレハブのリストを表示する
				for(int i = 0; i < prefabList.Length; i++){
					if(GUILayout.Button(prefabListID[i])) {
						selectPrefabPath = prefabList[i];
						selectPrefabID = prefabListID[i];
					}
				}
			}
		// オブジェクト選んだら
		if(selectPrefabPath == null) return;
			LayoutPreviewRun();
		}
	}

	// プレビューのレイアウト
	private void LayoutPreviewRun(){
		// スタイルの設定
		GUIStyleState style = new GUIStyleState(){
			textColor = new Color(1.0f,1.0f,1.0f,1.0f),
		};
		GUIStyle centerbold = new GUIStyle(){
			alignment = TextAnchor.MiddleCenter,
			fontStyle = FontStyle.Bold,
			normal = style,
		};
		// レイアウトの作成
		using(GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(previewRunScPos, EditorStyles.helpBox)){
			// タイトル
			GUILayout.Box("Preview");
			previewRunScPos = scroll.scrollPosition;
			// 現在の選択オブジェクトの設定
			EditorGUILayout.LabelField("選んでいるオブジェクト",centerbold);
			EditorGUILayout.TextField("",selectPrefabID);

			// 座標指定
			EditorGUILayout.LabelField("座標", centerbold);
			pos = EditorGUILayout.Vector3Field("",pos);
			// まとめ置き
			isMultiple = EditorGUILayout.ToggleLeft ("同時設置", isMultiple);

			if(isMultiple){
				using (new GUILayout.VerticalScope("HelpBox")){
					MultipleSetLayout();
				}
			}
			// インスタンス化ボタン
			if(GUILayout.Button("オブジェクトの設置")){
				ObjectInst(selectPrefabPath, pos);
			}
		}
	}
	private void MultipleSetLayout(){
		// 各方向の同時設置
		// X
		isMultipleX = EditorGUILayout.ToggleLeft ("X", isMultipleX);
		if(isMultipleX){
			using (new GUILayout.HorizontalScope("HelpBox")){
				using (new EditorGUI.IndentLevelScope()){
					isFrnMultiX = EditorGUILayout.ToggleLeft ("正方向", isFrnMultiX);
					GUILayout.FlexibleSpace();
					isInvMultiX = EditorGUILayout.ToggleLeft ("逆方向", isInvMultiX);
					GUILayout.FlexibleSpace();
					instNumX 	= EditorGUILayout.IntField("個数",instNumX);
				}
			}
		}
		// Y
		isMultipleY = EditorGUILayout.ToggleLeft ("Y", isMultipleY);
		if(isMultipleY){
			using (new GUILayout.HorizontalScope("HelpBox")){
				using (new EditorGUI.IndentLevelScope()){
					isFrnMultiY = EditorGUILayout.ToggleLeft ("正方向", isFrnMultiY);
					GUILayout.FlexibleSpace();
					isInvMultiY = EditorGUILayout.ToggleLeft ("逆方向", isInvMultiY);
					GUILayout.FlexibleSpace();
					instNumY 	= EditorGUILayout.IntField("個数",instNumY);
				}
			}
		}
		// Z
		isMultipleZ = EditorGUILayout.ToggleLeft ("Z", isMultipleZ);
		if(isMultipleZ){
			using (new GUILayout.HorizontalScope("HelpBox")){
				using (new EditorGUI.IndentLevelScope()){
					isFrnMultiZ = EditorGUILayout.ToggleLeft ("正方向", isFrnMultiZ);
					GUILayout.FlexibleSpace();
					isInvMultiZ = EditorGUILayout.ToggleLeft ("逆方向", isInvMultiZ);
					GUILayout.FlexibleSpace();
					instNumY 	= EditorGUILayout.IntField("個数",instNumZ);
				}
			}
		}
	}
	private void ObjectInst(string path, Vector3 pos){
		prefabData = AssetDatabase.LoadAssetAtPath<GameObject>(path);
		// プレハブからインスタンスを生成
		Debug.Log(pos);
		Debug.Log(prefabData);
		float size = prefabData.transform.localScale.x;
		Instantiate(prefabData, pos, Quaternion.identity);
		for(int i = 0; i < instNumX; i++){
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
