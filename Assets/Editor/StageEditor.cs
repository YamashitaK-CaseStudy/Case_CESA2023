using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow {
	private string path = "Assets/Prefabs/Stage/";	// 検索するファイル
	private Vector2 prefabListScPos;	// プレハブの一覧用のバー座標
	private Vector2 previewRunScPos;	// プレビュー用のバー座標
	private GameObject prefabData;		// プレハブのデータ
	string[] prefabList;				// プレハブの一覧
	string[] prefabListID;				// プレハブの一覧
	string selectPrefabPath;			// 選択されたプレハブのパス
	string selectPrefabID;				//　選択されたプレハブのID
	Vector3 pos = new Vector3(0,0,0);
	int setnum = 1;
	bool isEditorInit = false;

	private GameObject previewobj;

	[MenuItem("Editor/ステージエディター")]
	private static void ShowWindow() {
		EditorWindow.GetWindow(typeof(CStageEditor));
	}

	// GUI 表示
	private void OnGUI() {
		if(isEditorInit == false) Initalize();

		using (new GUILayout.HorizontalScope()){
			LayoutPrefabList();
			if(selectPrefabPath == null) return;
			LayoutPreviewRun();
		}
	}

	// プレハブのレイアウト
	private void LayoutPrefabList(){
		using(GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox, GUILayout.Width(200))){
			// プレハブのリロード
			if(GUILayout.Button("Prefab Reload")) LoadPrefabList();

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
			EditorGUILayout.LabelField("設置個数");
			setnum = EditorGUILayout.IntField("",setnum);

			// インスタンス化ボタン
			if(GUILayout.Button("オブジェクトの設置")){
				ObjectInst(selectPrefabPath, pos);
			}
		}
	}

	private void Initalize(){
		isEditorInit = true;
		CreateObjInitialize();
		EditorInitialize();
	}

}
