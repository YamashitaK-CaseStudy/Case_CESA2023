using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow {
	bool isEditorInit = false;	// 初期化チェック
	enum TABNUM{	// タブ列挙
		PutObj = 0,
		CreateRotateObj,
		CreateStair,
	}

	int _tabNumber = 0;

	[MenuItem("Editor/ステージエディター")]
	private static void ShowWindow() {
		EditorWindow.GetWindow(typeof(CStageEditor));
	}
	private void Initalize(){
		isEditorInit = true;
		CreateObjInitialize();
		CreateRotateObjInitialize();
		CreateFloorInitialize();
		PreviewInitialize();
	}

	// GUI 表示
	private void OnGUI() {
		if(isEditorInit == false) Initalize();

		if(EditorApplication.isPlaying){
			EditorGUILayout.HelpBox("実行中は操作できません", MessageType.Warning);
			return;
		}

		string[] tabList = {"オブジェクト配置","回転オブジェクト作成","床作成"};
		TABNUM tabnum = (TABNUM)GUILayout.Toolbar(_tabNumber, tabList, EditorStyles.toolbarButton);
		_tabNumber = (int)tabnum;
		switch(tabnum){
			case TABNUM.PutObj:
				LayoutPrefabList();
				break;
			case TABNUM.CreateRotateObj:
				// EditorGUILayout.HelpBox("現在制作中だよ", MessageType.Info);
				// return;
				LayoutRotateObj();
				break;
			case TABNUM.CreateStair:
				LayoutCreateFloor();
				break;
			default:
				Debug.LogError("Error");
				break;
		}
	}

}
