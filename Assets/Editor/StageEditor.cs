using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow {
	bool isEditorInit = false;	// 初期化チェック
	enum TABNUM{	// タブ列挙
		PutObj = 0,
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
		EditorInitialize();
	}

	// GUI 表示
	private void OnGUI() {
		if(isEditorInit == false) Initalize();

		string[] tabList = {"オブジェクト配置","階段作成"};
		TABNUM tabnum = (TABNUM)GUILayout.Toolbar(_tabNumber, tabList, EditorStyles.toolbarButton);

		switch(tabnum){
			case TABNUM.PutObj:
				LayoutPrefabList();
				break;
			case TABNUM.CreateStair:
				break;
			default:
				Debug.LogError("Error");
				break;
		}
	}

}
