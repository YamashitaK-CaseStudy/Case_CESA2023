using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// AssetPostprocessorの拡張クラス
/// </summary>
public class AssetPostprocessorAudio: AssetPostprocessor {

	// オーディオ追加されたらの処理
	public void OnPostprocessAudio(AudioClip clip) {
		AudioImporter audioImporter = assetImporter as AudioImporter;
		string path = audioImporter.assetPath;

		// MeneSEフォルダの中身のオーディオはすべてモノラルにする
		audioImporter.forceToMono = path.Contains("MenuSE");

		// BGMフォルダの中身のオーディオはバックグラウンドで読み込み
		audioImporter.loadInBackground = path.Contains("BGM");
	}

}