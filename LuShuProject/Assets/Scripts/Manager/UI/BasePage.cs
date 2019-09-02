using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

//静态页面基类 固定的页面，如开始登陆、主UI等非显示对象类
public class BasePage
{
    private GObject _mainCom;
    private bool _isResident;
    private string _pkgPath;
    private string _mainPath;
    /// <summary>
    /// ctor方法，设置基本UI显示对象参数
    /// </summary>
    /// <param name="pkgPath">FGUI包名</param>
    /// <param name="mainPath">FGUI包内组件名</param>
    /// <param name="isResident">UI是否常驻（常驻不销毁只隐藏）</param>
    public BasePage(string pkgPath, string mainPath, bool isResident = true)
    {
        _isResident = isResident;
        _pkgPath = pkgPath;
        _mainPath = mainPath;
        Debug.Log(">>BasePage:ctor..pkg:"+_pkgPath+">>mainpath:"+_mainPath);
    }

    /// <summary>
    /// 内部接口 创建显示对象设置自适应
    /// </summary>
    public void Create()
    {
        _mainCom = UIPackage.CreateObject(_pkgPath, _mainPath);//创建窗体
        //设置屏幕自适应
        _mainCom.SetSize(GRoot.inst.width,GRoot.inst.height);
        _mainCom.AddRelation(GRoot.inst,RelationType.Size);
        GRoot.inst.AddChild(_mainCom);
        OnInit();
    }
    /// <summary>
    /// 内部接口，显示显示对象
    /// </summary>
    public void Show()
    {
        if (_mainCom == null)
        {
            Create();
        }
        OnBindAction();
        _mainCom.visible = true;
        OnShow();
    }

    /// <summary>
    /// 内部接口，隐藏显示对象
    /// </summary>
    public void Hide()
    {
        UnBindAction();
        OnHide();
        if (_isResident)
        {
            _mainCom.visible = false;
        }
        else
        {
            Destroy();
        }
    }

    /// <summary>
    /// 内部接口 销毁显示对象
    /// </summary>
    public void Destroy()
    {
        _mainCom.Dispose();
        _mainCom = null;
        OnDestroy();
    }

    //子类复写接口，显示对象初始化，这个函数调用时显示对象第一次生成
    private void OnInit() {}
    //子类复写接口，显示对象事件绑定，这个函数调用时显示对象将要显示,一般在此绑定事件
    private void OnBindAction() {}
    //子类复写接口，显示对象显示时，这个函数调用时显示对象已经显示
    private void OnShow() {}
    //子类复写接口，显示对象事件卸载，这个函数调用时显示对象即将隐藏
    private void UnBindAction() {}
    //子类复写接口，显示对象隐藏时操作，这个函数调用时显示对象已经隐藏
    private void OnHide() {}
    //子类复写接口，显示对象被销毁时触发，这个函数调用时显示对象已经销毁
    private void OnDestroy() {}
    
}
