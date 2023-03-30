using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public partial class CStageEditor : EditorWindow
{

	private string path = "Assets/Prefabs/Stage/";	// 検索するファイル
	private Vector2 prefabListScPos;	// プレハブの一覧用のバー座標
	private Vector2 previewRunScPos;	// プレビュー用のバー座標
	private GameObject prefabData;		// プレハブのデータ
	string[] prefabList;				// プレハブの一覧
	string[] prefabListID;				// プレハブの一覧
	string selectPrefabPath;			// 選択されたプレハブのパス
	string selectPrefabID;				//　選択されたプレハブのID
	Vector3 pos = new Vector3(0, 0, 0);
	private bool isMultiple = false;	// 同時設置するかどうか
	private bool isMultipleX, isMultipleY, isMultipleZ;	// 同時設置
	private bool isFrnMultiX, isFrnMultiY, isFrnMultiZ; // 前設置
	private bool isInvMultiX, isInvMultiY, isInvMultiZ;	// 逆設置
	int instNumX = 0, instNumY = 0, instNumZ = 0;
	int instInvNumX = 0, instInvNumY = 0, instInvNumZ = 0;

	private bool CreateObjInitialize()
	{
		LoadPrefabList();
		return true;
	}
	// プレハブのレイアウト
	private void LayoutPrefabList()
	{
		using (new GUILayout.HorizontalScope())
		{
			// プレハブの一覧表示
			using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox, GUILayout.Width(250)))
			{
				// プレハブの再読み込みボタン
				if (GUILayout.Button("Prefab Reload")) LoadPrefabList();
				EditorGUILayout.LabelField("Prefab List");
				prefabListScPos = scroll.scrollPosition;

				// プレハブのリストを表示する
				for (int i = 0; i < prefabList.Length; i++)
				{
					if (GUILayout.Button(prefabListID[i]))
					{
						selectPrefabPath = prefabList[i];
						selectPrefabID = prefabListID[i];
					}
				}
			}
			// オブジェクト選んだら
			if (selectPrefabPath == null) return;
			LayoutPreviewRun();
		}
	}

	// プレビューのレイアウト
	private void LayoutPreviewRun()
	{
		// スタイルの設定
		GUIStyleState style = new GUIStyleState()
		{
			textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f),
		};
		GUIStyle centerbold = new GUIStyle()
		{
			alignment = TextAnchor.MiddleCenter,
			fontStyle = FontStyle.Bold,
			normal = style,
		};
		// レイアウトの作成
		using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(previewRunScPos, EditorStyles.helpBox))
		{
			// タイトル
			GUILayout.Box("詳細設定");
			previewRunScPos = scroll.scrollPosition;
			// 現在の選択オブジェクトの設定
			EditorGUILayout.LabelField("選んでいるオブジェクト", centerbold);
			EditorGUILayout.TextField("", selectPrefabID);

			// 座標指定
			EditorGUILayout.LabelField("座標", centerbold);
			pos = EditorGUILayout.Vector3Field("", pos);
			// まとめ置き
			isMultiple = EditorGUILayout.ToggleLeft("同時設置", isMultiple);

			if (isMultiple)
			{
				using (new GUILayout.VerticalScope("HelpBox"))
				{
					MultipleSetLayout();
				}
			}
			// インスタンス化ボタン
			if (GUILayout.Button("オブジェクトの設置"))
			{
				ObjectInst(selectPrefabPath, pos);
			}
		}
	}
	private void MultipleSetLayout()
	{
		EditorGUILayout.HelpBox("正方向はUnityの座標系で+の方向　逆方向は-の方向です", MessageType.Info);

		// 各方向の同時設置
		// X
		isMultipleX = EditorGUILayout.ToggleLeft("X", isMultipleX);
		if (isMultipleX)
		{
			using (new GUILayout.VerticalScope("HelpBox"))
			{
				using (new GUILayout.HorizontalScope())
				{
					using (new EditorGUI.IndentLevelScope())
					{
						isFrnMultiX = EditorGUILayout.ToggleLeft("正方向", isFrnMultiX, GUILayout.Width(100.0f));
						instNumX = EditorGUILayout.IntField("正方向個数", instNumX, GUILayout.Width(250.0f));
					}
				}
				using (new GUILayout.HorizontalScope())
				{
					using (new EditorGUI.IndentLevelScope())
					{
						isInvMultiX = EditorGUILayout.ToggleLeft("逆方向", isInvMultiX, GUILayout.Width(100.0f));
						instInvNumX = EditorGUILayout.IntField("逆方向個数", instInvNumX, GUILayout.Width(250.0f));
					}
				}
			}
		}
		// Y
		isMultipleY = EditorGUILayout.ToggleLeft("Y", isMultipleY);
		if (isMultipleY)
		{
			using (new GUILayout.VerticalScope("HelpBox"))
			{
				using (new GUILayout.HorizontalScope())
				{
					using (new EditorGUI.IndentLevelScope())
					{
						isFrnMultiY = EditorGUILayout.ToggleLeft("正方向", isFrnMultiY, GUILayout.Width(100.0f));
						instNumY = EditorGUILayout.IntField("正方向個数", instNumY, GUILayout.Width(250.0f));
					}
				}
				using (new GUILayout.HorizontalScope())
				{
					using (new EditorGUI.IndentLevelScope())
					{
						isInvMultiY = EditorGUILayout.ToggleLeft("逆方向", isInvMultiY, GUILayout.Width(100.0f));
						instInvNumY = EditorGUILayout.IntField("逆方向個数", instInvNumY, GUILayout.Width(250.0f));
					}
				}
			}
		}
		// Z
		isMultipleZ = EditorGUILayout.ToggleLeft("Z", isMultipleZ);
		if (isMultipleZ)
		{
			using (new GUILayout.VerticalScope("HelpBox"))
			{
				using (new GUILayout.HorizontalScope())
				{
					using (new EditorGUI.IndentLevelScope())
					{
						isFrnMultiZ = EditorGUILayout.ToggleLeft("正方向", isFrnMultiZ, GUILayout.Width(100.0f));
						instNumZ = EditorGUILayout.IntField("正方向個数", instNumZ, GUILayout.Width(250.0f));
					}
				}
				using (new GUILayout.HorizontalScope())
				{
					using (new EditorGUI.IndentLevelScope())
					{
						isInvMultiZ = EditorGUILayout.ToggleLeft("逆方向", isInvMultiZ, GUILayout.Width(100.0f));
						instInvNumZ = EditorGUILayout.IntField("逆方向個数", instInvNumZ, GUILayout.Width(250.0f));
					}
				}
			}
		}
	}
	private void ObjectInst(string path, Vector3 pos)
	{
		prefabData = AssetDatabase.LoadAssetAtPath<GameObject>(path);
		// プレハブからインスタンスを生成
		float size = prefabData.transform.localScale.x;
		Instantiate(prefabData, pos, Quaternion.identity);
		// 追加分
		AddInstance(prefabData, pos);
	}

	private void AddInstance(GameObject prefabData, Vector3 pos){
		// X
		// 正
		if(isFrnMultiX){
			for(int i = 1; i < instNumX; i++){
				var tmppos = pos;
				tmppos.x += i;
				Instantiate(prefabData, tmppos, Quaternion.identity);
			}
		}
		// 負
		if(isInvMultiX){
			for(int i = 1; i < instInvNumX; i++){
				var tmppos = pos;
				tmppos.x -= i;
				Instantiate(prefabData, tmppos, Quaternion.identity);
			}
		}
		// Y
		// 正
		if(isFrnMultiY){
			for(int i = 1; i < instNumY; i++){
				var tmppos = pos;
				tmppos.y += i;
				Instantiate(prefabData, tmppos, Quaternion.identity);
			}
		}
		// 負
		if(isInvMultiY){
			for(int i = 1; i < instInvNumY; i++){
				var tmppos = pos;
				tmppos.y -= i;
				Instantiate(prefabData, tmppos, Quaternion.identity);
			}
		}
		// Z
		// 正
		if(isFrnMultiZ){
			for(int i = 1; i < instNumZ; i++){
				var tmppos = pos;
				tmppos.z += i;
				Instantiate(prefabData, tmppos, Quaternion.identity);
			}
		}
		// 負
		if(isInvMultiY){
			for(int i = 1; i < instInvNumZ; i++){
				var tmppos = pos;
				tmppos.z -= i;
				Instantiate(prefabData, tmppos, Quaternion.identity);
			}
		}
	}

	private void LoadPrefabList()
	{
		// GUIDの検索
		string[] tmp = AssetDatabase.FindAssets("t:prefab", new string[] { path });
		prefabList = tmp.Select(tmp => AssetDatabase.GUIDToAssetPath(tmp)).ToArray();
		// nullCheck
		if (prefabListID == null)
		{
			prefabListID = prefabList;
		}

		// 名前の割り当て
		for (int i = 0; i < prefabList.Length; i++)
		{
			prefabListID[i] = prefabList[i].Remove(0, path.Length);
		}

		return;
	}
}
