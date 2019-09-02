﻿//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

///// <summary>
///// 消息事件在这里添加
///// </summary>
//public enum UIMsgID
//{
//    NONE = 0,
//    PLAYERINFO = 1,
//    OK = 200,
//    FAIL = 201
//}

//public class UIManager : Singleton<UIManager>
//{
//    private RectTransform m_UIRoot;//UI节点
//    private RectTransform m_WndRoot;//窗口节点
//    private Camera m_UICamera;//UI相机
//    private EventSystem m_EventSystem;//事件系统
//    private float m_CanvasRate = 0;//屏幕的宽高比

//    private string m_UIPrefabPath = "Assets/GameData/Prefabs/UGUI/Panel/";
//    private Dictionary<string,WindowUGUI> m_WindowDic = new Dictionary<string, WindowUGUI>();//窗口缓存
//    private Dictionary<string,Type> m_RegisterDic = new Dictionary<string, Type>();//注册的窗口名对应的窗口类型

//    /// <summary>
//    /// 初始化
//    /// </summary>
//    /// <param name="m_UIRoot"></param>
//    /// <param name="m_WndRoot"></param>
//    /// <param name="m_UICamera"></param>
//    /// <param name="m_EventSystem"></param>
//    public void Init(RectTransform m_UIRoot, RectTransform m_WndRoot, Camera m_UICamera, EventSystem m_EventSystem)
//    {
//        this.m_UIRoot = m_UIRoot;
//        this.m_WndRoot = m_WndRoot;
//        this.m_UICamera = m_UICamera;
//        this.m_EventSystem = m_EventSystem;

//        m_CanvasRate = Screen.height /( this.m_UICamera.orthographicSize * 2 );//计算屏幕宽高比
//    }
//    /// <summary>
//    /// 设置所有uiprefab路径
//    /// </summary>
//    /// <param name="path"></param>
//    public void SetUIPrefabPath(string path)
//    {
//        this.m_UIPrefabPath = path;
//    }
//    /// <summary>
//    /// 窗口注册方法
//    /// </summary>
//    /// <typeparam name="T">窗口泛型类</typeparam>
//    /// <param name="name">窗口名</param>
//    public void Register<T>(string name) where T : WindowUGUI
//    {
//        m_RegisterDic[name] = typeof(T);
//    }

//    /// <summary>
//    /// 根据窗口名获得窗口对象
//    /// </summary>
//    /// <typeparam name="T">窗口类型</typeparam>
//    /// <param name="name">窗口名</param>
//    /// <returns></returns>
//    public T FindWndByName<T>(string name)where T: WindowUGUI
//    {
//        WindowUGUI wnd = null;
//        if (m_WindowDic.TryGetValue(name, out wnd) && wnd != null)
//        {
//            return (T)wnd;
//        }

//        return null;
//    }
//    /// <summary>
//    /// 打开指定窗口
//    /// </summary>
//    /// <param name="wndName"></param>
//    /// <param name="bTop"></param>
//    /// <param name="paramList"></param>
//    /// <returns></returns>
//    public WindowUGUI OpenWnd(string wndName,bool bTop, params object[] paramList)
//    {
//        WindowUGUI wnd = FindWndByName<WindowUGUI>(wndName);
//        if (wnd == null)
//        {
//            Type type = null;
//            if (m_RegisterDic.TryGetValue(wndName, out type) && type != null)
//            {
//                wnd = System.Activator.CreateInstance(type) as WindowUGUI;
//            }
//            else
//            {
//                Debug.Log("找不到对应的窗口脚本，窗口名为：" + wndName);
//            }
//            //拼接字符串耗gc，换种方式
//            StringBuilder sb = new StringBuilder();
//            if (wndName.EndsWith(".prefab"))
//                sb.Append(m_UIPrefabPath).Append(wndName);
//            else
//                sb.Append(m_UIPrefabPath).Append(wndName).Append(".prefab");
//            GameObject wndObj = ObjectManger.Instance.InstantiateObject(sb.ToString(), false, false);
//            if (wndObj == null)
//            {
//                Debug.Log("窗口Prefab创建失败" + wndName);
//                return null;
//            }
//            if (!m_WindowDic.ContainsKey(wndName))
//                m_WindowDic.Add(wndName,wnd);
//            //初始化obj
//            wnd.GameObject = wndObj;
//            wnd.Transform = wndObj.transform;
//            wnd.Name = wndName;
//
//            wnd.Awake(paramList);
//            wndObj.transform.SetParent(m_WndRoot,false);
//
//            if (bTop)
//                wndObj.transform.SetAsLastSibling();
//
//            wnd.OnShow(paramList);
//        }
//        else
//        {
//            ShowWindow(wndName,bTop,paramList);
//        }
//        return wnd;
//    }
//    /// <summary>
//    /// 在指定页面打开弹窗，弹窗禁止页面后部点击事件
//    /// </summary>
//    /// <param name="wnd">当前窗口</param>
//    /// <param name="popupsName">弹窗名</param>
//    /// <param name="paramList">参数列表</param>
//    /// <returns></returns>
//    public void PopUpWnd(WindowUGUI wnd,string popupsName, params object[] paramList)
//    {
//        if (wnd == null)
//        {
//            Debug.LogError("未指定弹窗父页面");
//            return;
//        }

//        wnd.OnDisable();//界面禁用

//        Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.6f);//弹窗遮罩颜色

//        WindowUGUI popups = FindWndByName<WindowUGUI>(popupsName);
//        if (popups == null)
//        {
//            Type type = null;
//            if (m_RegisterDic.TryGetValue(popupsName, out type) && type != null)
//            {
//                popups = System.Activator.CreateInstance(type) as WindowUGUI;
//            }
//            else
//            {
//                Debug.Log("找不到对应的弹窗脚本，弹窗名为：" + popupsName);
//            }
//            //拼接字符串耗gc，换种方式
//            StringBuilder sb = new StringBuilder();
//            if (popupsName.EndsWith(".prefab"))
//                sb.Append(m_UIPrefabPath).Append(popupsName);
//            else
//                sb.Append(m_UIPrefabPath).Append(popupsName).Append(".prefab");
//            GameObject wndObj = ObjectManger.Instance.InstantiateObject(sb.ToString(), false, false);
//            if (wndObj == null)
//            {
//                Debug.Log("弹窗Prefab创建失败" + popupsName);
//            }
//            if (!m_WindowDic.ContainsKey(popupsName))
//                m_WindowDic.Add(popupsName, popups);
//            //初始化obj
//            popups.GameObject = wndObj;
//            popups.Transform = wndObj.transform;
//            popups.Name = popupsName;

//            popups.Awake(paramList);
//            wndObj.transform.SetParent(m_WndRoot, false);
//            wndObj.transform.SetAsLastSibling();

//            popups.PopupBackground = AddBackground(wndObj, backgroundColor);//添加弹窗遮罩
//            popups.ParentWnd = wnd;
//            popups.OnShow(paramList);
//        }
//        else
//        {
//            popups.PopupBackground = AddBackground(popups.GameObject, backgroundColor);//添加弹窗遮罩
//            popups.ParentWnd = wnd;
//            ShowWindow(popups, true, paramList);
//        }
//    }

//    /// <summary>
//    /// 关闭指定窗口
//    /// </summary>
//    /// <param name="wndName">窗口名</param>
//    /// <param name="destory">是否彻底销毁</param>
//    public void CloseWindow(string wndName, bool destory = false)
//    {
//        WindowUGUI wnd = FindWndByName<WindowUGUI>(wndName);
//        CloseWindow(wnd, destory);
//    }
//    /// <summary>
//    /// /关闭指定窗口
//    /// </summary>
//    /// <param name="wnd">窗体</param>
//    /// <param name="destory">是否彻底销毁</param>
//    public void CloseWindow(WindowUGUI wnd, bool destory = false)
//    {
//        if (wnd != null)
//        {
//            wnd.OnDisable();
//            wnd.OnClose();
//            if (m_WindowDic.ContainsKey(wnd.Name))
//                m_WindowDic.Remove(wnd.Name);
//            if (wnd.PopupBackground != null)//是弹窗
//            {
//                wnd.ParentWnd.OnShow();
//                wnd.ParentWnd.Transform.SetAsLastSibling();
//                GameObject.Destroy(wnd.PopupBackground);
//                wnd.PopupBackground = null;
//                wnd.ParentWnd = null;
//            }
//            if (destory)
//            {
//                ObjectManger.Instance.ReleaseObject(wnd.GameObject,0,true);
//            }
//            else
//            {
//                ObjectManger.Instance.ReleaseObject(wnd.GameObject,recycleParent:false);//回收到回收节点会导致ui重绘，耗gc
//            }

//            wnd.GameObject = null;
//            wnd.Transform = null;
//            wnd = null;
//        }
//    }
//    /// <summary>
//    /// 关闭所有窗口
//    /// </summary>
//    public void CloseAllWindow()
//    {
//        List<WindowUGUI> tempList = new List<WindowUGUI>(m_WindowDic.Values);
//        for (int i = 0; i < tempList.Count; i++)
//        {
//            CloseWindow(tempList[i].Name);
//        }
//    }

//    /// <summary>
//    /// 添加弹窗遮罩
//    /// </summary>
//    /// <param name="wnd">弹窗</param>
//    /// <param name="backgroundColor"></param>
//    /// <param name="m_background"></param>
//    private GameObject AddBackground(GameObject wnd, Color backgroundColor)
//    {
//        GameObject background; //弹窗遮罩
//        var bgTex = new Texture2D(1, 1);
//        bgTex.SetPixel(0, 0, backgroundColor);
//        bgTex.Apply();

//        background = new GameObject("PopupBackground");
//        var image = background.AddComponent<Image>();
//        var rect = new Rect(0, 0, bgTex.width, bgTex.height);
//        var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
//        image.material.mainTexture = bgTex;
//        image.sprite = sprite;
//        var newColor = image.color;
//        image.color = newColor;
//        image.canvasRenderer.SetAlpha(0.0f);
//        image.CrossFadeAlpha(1.0f, 0.4f, false);

//        //var canvas = GameObject.Find("Canvas");
//        background.transform.localScale = new Vector3(1, 1, 1);
//        background.GetComponent<RectTransform>().sizeDelta = m_UIRoot.GetComponent<RectTransform>().sizeDelta;
//        background.transform.SetParent(m_WndRoot.transform, false);
//        background.transform.SetSiblingIndex(wnd.transform.GetSiblingIndex());
//        return background;
//    }

//    /// <summary>
//    /// 切换到唯一窗口
//    /// </summary>
//    /// <param name="wndName"></param>
//    /// <param name="bTop"></param>
//    /// <param name="paramList"></param>
//    public void SwitchStateByName(string wndName, bool bTop = true, params object[] paramList)
//    {
//        CloseAllWindow();//关闭所有窗口
//        OpenWnd(wndName, bTop, paramList);//打开窗口
//    }
//    /// <summary>
//    /// 隐藏窗口
//    /// </summary>
//    /// <param name="wndName">窗口名</param>
//    public void HideWindow(string wndName)
//    {
//        WindowUGUI wnd = FindWndByName<WindowUGUI>(wndName);
//        HideWindow(wnd);
//    }
//    /// <summary>
//    /// 隐藏窗口
//    /// </summary>
//    /// <param name="wnd">窗口对象</param>
//    public void HideWindow(WindowUGUI wnd)
//    {
//        if (wnd != null)
//        {
//            wnd.OnDisable();
//            wnd.GameObject.SetActive(false);
//        }
//    }
//    /// <summary>
//    /// 调用所有界面的update方法
//    /// </summary>
//    public void OnUpdate()
//    {
//        foreach (WindowUGUI wnd in m_WindowDic.Values)
//        {
//            if(wnd!=null)
//                wnd.OnUpdate();
//        }
//    }

//    /// <summary>
//    /// 调用窗口事件
//    /// </summary>
//    /// <param name="wndName"></param>
//    /// <param name="msgId"></param>
//    /// <param name="paramList"></param>
//    /// <returns></returns>
//    public bool SendMessageToWindow(string wndName, UIMsgID msgId, params object[] paramList)
//    {
//        WindowUGUI wnd = FindWndByName<WindowUGUI>(wndName);
//        if (wnd != null)
//        {
//            return wnd.OnMessage( msgId, paramList);
//        }

//        return false;
//    }

//    /// <summary>
//    /// 显示窗口
//    /// </summary>
//    /// <param name="wndName">窗口名</param>
//    /// <param name="bTop">是否置顶</param>
//    /// <param name="paramList">参数列表</param>
//    public void ShowWindow(string wndName, bool bTop = true, params object[] paramList)
//    {
//        WindowUGUI wnd = FindWndByName<WindowUGUI>(wndName);
//        ShowWindow(wnd, bTop, paramList);
//    }

//    /// <summary>
//    /// 显示窗口
//    /// </summary>
//    /// <param name="wnd">窗口对象</param>
//    /// <param name="bTop">是否置顶</param>
//    /// <param name="">参数列表</param>
//    public void ShowWindow(WindowUGUI wnd, bool bTop = true, params object[] paramList)
//    {
//        if (wnd != null)
//        {
//            if( wnd.GameObject != null && !wnd.GameObject.activeSelf) 
//                wnd.GameObject.SetActive(true);
//            if (wnd.Transform != null && bTop)
//                wnd.Transform.SetAsLastSibling();
//            wnd.OnShow(paramList);
//        }
//    }

//}
