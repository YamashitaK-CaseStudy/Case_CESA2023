using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CStageEditor : EditorWindow {
	private string Path = "Assets/Prefabs/Stage/";	// 検索するファイル
	private Vector2 PrefabListScPos;	// プレハブの一覧用のバー座標
	private Vector2 PreviewRunScPos;	// プレビュー用のバー座標
	private GameObject PrefabData;		// プレハブのデータ
	string[] PrefabList;				// プレハブの一覧
	string[] PrefabListID;				// プレハブの一覧
	string SelectPrefabPath;			// 選択されたプレハブのパス
	string SelectPrefabID;				//　選択されたプレハブのID
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
			if(SelectPrefabPath == null) return;
			LayoutPreviewRun();
		}
	}

	// プレハブのレイアウト
	private void LayoutPrefabList(){
		using(GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(PrefabListScPos, EditorStyles.helpBox, GUILayout.Width(200))){
			// プレハブのリロード
			if(GUILayout.Button("Prefab Reload")) LoadPrefabList();

			EditorGUILayout.LabelField("Prefab List");
			PrefabListScPos = scroll.scrollPosition;

			// プレハブのリストを表示する
			for(int i = 0; i < PrefabList.Length; i++){
				if(GUILayout.Button(PrefabListID[i])) {
					SelectPrefabPath = PrefabList[i];
					SelectPrefabID = PrefabListID[i];
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
		using(GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(PreviewRunScPos, EditorStyles.helpBox)){
			// タイトル
			GUILayout.Box("Preview");
			PreviewRunScPos = scroll.scrollPosition;
			// 現在の選択オブジェクトの設定
			EditorGUILayout.LabelField("選んでいるオブジェクト",centerbold);
			EditorGUILayout.TextField("",SelectPrefabID);

			// 座標指定
			EditorGUILayout.LabelField("座標", centerbold);
			pos = EditorGUILayout.Vector3Field("",pos);

			// まとめ置き
			EditorGUILayout.LabelField("設置個数");
			setnum = EditorGUILayout.IntField("",setnum);

			// プレビューの表示
			Rect rect = new Rect(0,0,100,100);
			//PreView.BeginStaticPreview(rect);
			// インスタンス化ボタン
			if(GUILayout.Button("オブジェクトの設置")){
				ObjectInst(SelectPrefabPath, pos);
			}
		}
	}

	private void Initalize(){
		isEditorInit = true;
		LoadPrefabList();
		//PreView = new UnityEditor.PreviewRenderUtility();
	}
	private void ObjectInst(string path, Vector3 pos){
		PrefabData = AssetDatabase.LoadAssetAtPath<GameObject>(path);
		// プレハブからインスタンスを生成
		Debug.Log(pos);
		Debug.Log(PrefabData);
		float size = PrefabData.transform.localScale.x;
		for(int i = 0; i < setnum; i++){
			Vector3 setpos = pos;
			setpos.x = pos.x + size * i;
			Instantiate(PrefabData, setpos, Quaternion.identity);
		}
	}

	private void LoadPrefabList(){
		// GUIDの検索
		string[] tmp = AssetDatabase.FindAssets("t:prefab", new string[]{Path});
		PrefabList = tmp.Select(tmp => AssetDatabase.GUIDToAssetPath(tmp)).ToArray();
		// nullCheck
		if(PrefabListID == null) {
			PrefabListID = PrefabList;
		}

		// 名前の割り当て
		for(int i = 0; i < PrefabList.Length; i++){
			PrefabListID[i] = PrefabList[i].Remove(0,Path.Length);
		}

		return;
	}

}
