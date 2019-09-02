﻿using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace FairyGame
{
	public class UISource : IUISource
	{
		string _fileName;
		bool _loading;

		public UISource(string fileName)
		{
			_fileName = fileName;
		}

		public string fileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		public bool loaded
		{
			get { return UIPackage.GetByName(_fileName) != null; }
		}

		public void Load(UILoadCallback callback)
		{
			if (_loading)
				return;

			_loading = true;
            Debug.Log(">>>>UISource : IUISource");

			UIManager.AddUIPackage(_fileName);
//            AssetManager.inst.LoadAsset("ui", _fileName.ToLower(),
//				(string assetPath, string fileName, object data) => { callback(); });
		}
	}
}
