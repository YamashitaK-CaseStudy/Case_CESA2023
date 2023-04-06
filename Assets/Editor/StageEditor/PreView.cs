using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class CStageEditor : EditorWindow
{
	private PreviewRenderUtility _previewUtility;
	private Camera _previewCamera;
	private bool PreviewInitialize(){
		// カメラの作成を行う
		_previewUtility = new PreviewRenderUtility();
		_previewCamera = _previewUtility.camera;
		_previewCamera.fieldOfView = 30f;
		return true;
	}

	private void UpdatePreview(GameObject previewObj){
		if (previewObj == null)
		{
			EditorGUILayout.HelpBox("Select an object to preview", MessageType.Info);
			return;
		}

		// レンダリングテクスチャを更新
		_previewUtility.BeginPreview(new Rect(0, 0, position.width, position.height), GUIStyle.none);
		_previewCamera.Render();
		Texture previewTexture = _previewUtility.EndPreview();

		// プレビュー用のテクスチャを描画
		GUI.DrawTexture(new Rect(0, 0, position.width, position.height), previewTexture, ScaleMode.ScaleToFit);

		// プレビュー用のカメラを更新
		if (_previewUtility != null && _previewCamera != null)
		{
			_previewUtility.camera.transform.position = previewObj.transform.position;
			_previewUtility.camera.transform.rotation = Quaternion.identity;
			_previewUtility.camera.Render();
		}else{
			return;
		}
	}

	private void PreviewDisable(){
		// カメラの解放
		_previewCamera = null;
		// リソースの解放
		_previewUtility.Cleanup();
	}
}
