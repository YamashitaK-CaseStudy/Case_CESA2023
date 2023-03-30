using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow
{
	private bool isCreateStart;	// 回転オブジェクトを作成開始する
	GameObject _parentObject;
	GameObject _object;
	GameObject[] _childObject;
	Vector3 _rotobjpos;
	void CreateRotateObjInitialize()
	{
		isCreateStart = false;
		_parentObject = null;
		_object = null;
	}

	void LayoutRotateObj()
	{
		EditorGUILayout.HelpBox("現在制作中だよ", MessageType.Info);
		return;

		if (isCreateStart)
		{
			// ヌルチェック
			if(!CheckNowCreateRotObj()) return;

			using (new GUILayout.HorizontalScope())
			{
				// 親子関係表示
				using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox, GUILayout.Width(250)))
				{
					// タイトル
					GUILayout.Box("回転オブジェクト親子関係");
					LayoutParentList();
				}
				// アタッチするオブジェクト項目
				using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(prefabListScPos, EditorStyles.helpBox, GUILayout.Width(250)))
				{
					// タイトル
					GUILayout.Box("オブジェクト項目");
					LayoutObjectList();
				}
				// 詳細設定
				using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(previewRunScPos, EditorStyles.helpBox))
				{
					// タイトル
					GUILayout.Box("詳細設定");
					LayoutSettings();
				}
			}
		}
		else
		{
			// オブジェクトの生成開始
			if (GUILayout.Button("オブジェクトの生成開始")){
				isCreateStart = true;
				CreateBaseRotateObject();
			}
		}
	}

	private void LayoutParentList(){
		GUILayout.Box(_parentObject.name);
		using (new EditorGUI.IndentLevelScope())
		{
			GUILayout.Box(_object.name);
			using (new EditorGUI.IndentLevelScope())
			{
				var childNum = _object.gameObject.transform.childCount;
				for(int i = 0; i < childNum; i++){
					GUILayout.Box(_object.gameObject.transform.GetChild(childNum).name);
				}
			}
		}
	}
	private void LayoutObjectList(){

	}
	private void LayoutSettings(){
		if(GUILayout.Button("生成終了")){
			isCreateStart = false;
		}
	}

	private void CreateBaseRotateObject(){
		_parentObject = new GameObject("RotatableObject");
		_object = new GameObject("Object");
		_object.gameObject.transform.parent = _parentObject.gameObject.transform;
	}

	private bool CheckNowCreateRotObj(){
		if(isCreateStart && _parentObject == null){
			isCreateStart = false;
			return false;
		}
		return true;
	}
}
