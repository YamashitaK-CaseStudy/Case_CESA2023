using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// AssetPostprocessor�̊g���N���X
/// </summary>
public class AssetPostprocessorAudio: AssetPostprocessor {

	// �I�[�f�B�I�ǉ����ꂽ��̏���
	public void OnPostprocessAudio(AudioClip clip) {
		AudioImporter audioImporter = assetImporter as AudioImporter;
		string path = audioImporter.assetPath;

		// MeneSE�t�H���_�̒��g�̃I�[�f�B�I�͂��ׂă��m�����ɂ���
		audioImporter.forceToMono = path.Contains("MenuSE");

		// BGM�t�H���_�̒��g�̃I�[�f�B�I�̓o�b�N�O���E���h�œǂݍ���
		audioImporter.loadInBackground = path.Contains("BGM");
	}

}