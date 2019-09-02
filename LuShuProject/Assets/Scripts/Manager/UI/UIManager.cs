using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class UIManager: Singleton<UIManager>
{
//    private Dictionary<string,BasePage> _pageDic = new Dictionary<string, BasePage>();//固定的页面，如开始登陆、主UI等非窗口类
    private Dictionary<string,BaseWindow> _windowDic = new Dictionary<string, BaseWindow>();//窗口类
//    private Dictionary<string,Type> _RegisterPageDic = new Dictionary<string, Type>();//注册的显示对象名对应的窗口类型
    private Dictionary<string,Type> _RegisterWindowDic = new Dictionary<string, Type>();//注册的窗口名对应的窗口类型
    private Stack<BaseWindow> _showWindowStack = new Stack<BaseWindow>();//显示UI栈

    public void Init()
    {
        //设置全局自适应
        GRoot.inst.SetContentScaleFactor(1136,640,UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
    }
    /// <summary>
    /// 窗口注册方法
    /// </summary>
    /// <typeparam name="T">窗口泛型类</typeparam>
    /// <param name="name">窗口名</param>
    public void Register<T>(string name) where T : BaseWindow
    {
        _RegisterWindowDic[name] = typeof(T);
    }
    
    /// <summary>
    /// 根据窗口名获得窗口对象
    /// </summary>
    /// <typeparam name="T">窗口类型</typeparam>
    /// <param name="name">窗口名</param>
    /// <returns></returns>
    public T FindWndByName<T>(string name)where T: BaseWindow
    {
        BaseWindow wnd = null;
        if (_windowDic.TryGetValue(name, out wnd) && wnd != null)
        {
            return (T)wnd;
        }

        return null;
    }
    
    /// <summary>
    /// 打开指定窗口
    /// </summary>
    /// <param name="wndName"></param>
    /// <param name="bTop"></param>
    /// <param name="paramList"></param>
    /// <returns></returns>
    public BaseWindow ShowWindow(string wndName, params object[] paramList)
    {
        BaseWindow wnd = FindWndByName<BaseWindow>(wndName);
        if (wnd == null)
        {
            Type type = null;
            if (_RegisterWindowDic.TryGetValue(wndName, out type) && type != null)
            {
                wnd = System.Activator.CreateInstance(type) as BaseWindow;
            }
            else
            {
                Debug.Log("找不到对应的窗口脚本，窗口名为：" + wndName);
            }
            if(wnd == null)
                return null;
            if (!_windowDic.ContainsKey(wndName))
                _windowDic.Add(wndName,wnd);
            //ShowUI
            wnd.Show(paramList);
        }
        else
        {
            wnd.Show(paramList);
        }

        if (!wnd._window.isShowing)
        {
            Debug.Log("窗口未准备好！"+wndName);
            return null;
        }
        if (!_showWindowStack.Contains(wnd))
        {
            _showWindowStack.Push(wnd);
        }
        return wnd;
    }
    /// <summary>
    /// 隐藏栈顶窗口
    /// </summary>
    /// <param name="wndName"></param>
    public void HideWindow(string wndName)
    {
        if (string.IsNullOrEmpty(wndName))
        {
            Debug.Log("UI窗口名不能为空");
            return;
        }
        BaseWindow wnd = FindWndByName<BaseWindow>(wndName);
        HideWindow(wnd);
    }
    
    public void HideWindow(BaseWindow wnd)
    {
        if (wnd != null)
        {
            wnd.Hide();
            RemoveWindowByStack(wnd);
        }
    }
    /// <summary>
    /// 关闭栈顶窗口
    /// </summary>
    public void CloseWindow(string wndName)
    {
        if (string.IsNullOrEmpty(wndName))
        {
            Debug.Log("UI窗口名不能为空");
            return;
        }
        BaseWindow wnd = FindWndByName<BaseWindow>(wndName);
        CloseWindow(wnd);
    }
    /// <summary>
    /// 关闭窗口
    /// </summary>
    public void CloseWindow(BaseWindow wnd)
    {
        if (wnd != null)
        {
            wnd.Hide();
            if (!wnd._isResident)
            {
                wnd.Destroy();
                ResourceManger.Instance.ReleaseResouce(wnd._pkgName.ToLower());//引用计数减少
            }
            RemoveWindowByStack(wnd);
        }
    }

    public void RemoveWindowByStack(BaseWindow wnd)
    {
        List<BaseWindow> wndList = new List<BaseWindow>();
        while (_showWindowStack.Count > 0)
        {
            BaseWindow topWnd = _showWindowStack.Pop();
            if (wnd.Equals(topWnd))
                break;
            else
                wndList.Add(topWnd);
        }

        foreach (var tempWnd in wndList)
        {
            _showWindowStack.Push(tempWnd);
        }
        wndList.Clear();
        wndList = null;
    }
    
    /// <summary>
    /// 关闭所有窗口
    /// </summary>
    public void CloseAllWindow()
    {
        while (_showWindowStack.Count > 0)
        {
            BaseWindow wnd =_showWindowStack.Peek();
            CloseWindow(wnd);   
        }
    }
    /// <summary>
    /// 切换到唯一窗口
    /// </summary>
    public void SwitchStateByName(string wndName)
    {
        CloseAllWindow();
        ShowWindow(wndName);
    }

    public static void AddUIPackage(string pkgName)
    {
        ResourceItem item = ResourceManger.Instance.LoadResourceUI(pkgName);
        UIPackage pkg = null;
#if UNITY_EDITOR
        if(!ResourceManger.Instance.m_LoadFromAssetBundle)
            pkg = UIPackage.AddPackage(item.m_AssetName);
#endif
        if (pkg == null)
        {
            UIPackage.AddPackage(item.m_AssetBundle);
        }
    }
}

