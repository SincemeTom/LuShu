using FairyGame;
using FairyGUI;
using UnityEngine;

public class BaseWindow
{
    public Window _window;
    protected GComponent _contentPane;
    protected Vector2 _originPos = Vector2.zero;
    protected string[] animation = new []{"fade_in","fade_out"}; //进入动画效果 退出动画效果
    protected bool _asyncCreate = false; //是否异步创建
    public string _pkgName; //包名
    public string _windowName;//组件名
    public bool _isResident;//是否常驻内存，常驻不会销毁只会隐藏

    public void Create()
    {
        _window = new Window();
        Init();
    }
    
    /// <summary>
    /// 内部接口 导入主UI所在AB包
    /// </summary>
    public void SetContentSource(string pkgName,string windowName)
    {
        _window.AddUISource(new UISource(pkgName));
        _pkgName = pkgName;
        _windowName = windowName;
    }
    /// <summary>
    /// 内部接口 添加窗口依赖UIAB包
    /// </summary>
    /// <param name="pkgNames"></param>
    public void SetDepends(string[] pkgNames)
    {
        foreach (var pkgName in pkgNames)
        {
            Debug.Log(">>>>>>>>>>BaseWindow:SetDepends");
            _window.AddUISource(new UISource(pkgName));
        }
    }
    
    /// <summary>
    /// 内部接口，显示窗口
    /// </summary>
    public void Init()
    {
        OnInit();
        if(!string.IsNullOrEmpty(_windowName))
        {
            _window.Show();
            if (_asyncCreate)//异步加载
            {
                UIPackage.CreateObjectAsync(_pkgName,_windowName,OnInitAsyncCallBack);
            }
            else
            {
                _window.contentPane = UIPackage.CreateObject(_pkgName, _windowName).asCom;
                _contentPane = _window.contentPane;
                Init2();
                DoShowAnimation();
            }
        }
    }

    public void OnInitAsyncCallBack( GObject obj )
    {
        _window.contentPane = obj.asCom;
        _contentPane = _window.contentPane;
        Debug.Log(">>>>>CreateObjectCallback:");
        Init2();
        if (_window.isShowing)
        {
            DoShowAnimation();
        }
    }

    /// <summary>
    /// 内部接口,初始化
    /// </summary>
    public void Init2()
    {
        _window.SetPivot(0.5f,0.5f);
        
        //设置屏幕自适应
        _window.MakeFullScreen();
    }
    /// <summary>
    /// 内部接口 进入动画效果
    /// </summary>
    public void DoShowAnimation()
    {
        if(_contentPane == null) return;
        _originPos = _window.xy;
        string ani = animation[0];
        if (ani == null) return;
        GTweener tween = null;
        switch (ani)
        {
            case "eject"://放大
                _window.SetScale(0.9f,0.9f);
                tween = _window.TweenScale(new Vector2(1f, 1f), 0.3f);
                TweenUtils.SetEase(tween, EaseType.BackOut);
                TweenUtils.OnComplete(tween,CallOnShown);
                break;
            case "fade_in"://淡入
                _window.alpha = 0;
                tween = _window.TweenFade(1,0.3f);
                TweenUtils.SetEase(tween, EaseType.QuadOut);
                TweenUtils.OnComplete(tween,CallOnShown);
                break;
            case "move_up"://上滑进入
                _window.y = GRoot.inst.height;
                tween = _window.TweenMoveY(_originPos.y,0.3f);
                TweenUtils.SetEase(tween, EaseType.QuadOut);
                TweenUtils.OnComplete(tween,CallOnShown);
                break;
            case "move_left"://左滑动进入
                _window.x = GRoot.inst.width;
                tween = _window.TweenMoveX(_originPos.x,0.3f);
                TweenUtils.SetEase(tween, EaseType.QuadOut);
                TweenUtils.OnComplete(tween,CallOnShown);
                break;
            case "move_right"://右滑进入
                _window.x = -GRoot.inst.width - 30;
                tween = _window.TweenMoveX(_originPos.x,0.3f);
                TweenUtils.SetEase(tween, EaseType.QuadOut);
                TweenUtils.OnComplete(tween,CallOnShown);
                break;
            default:
                CallOnShown();
                break;
        }
    }
    /// <summary>
    /// 内部接口 退出动画效果
    /// </summary>
    public void DoHideAnimation()
    {
        _originPos = _window.xy;
        string ani = animation[1];
        if (ani == null) return;
        GTweener tween = null;
        switch (ani)
        {
            case "shrink"://缩小
                _window.SetScale(1,1);
                tween = _window.TweenScale(new Vector2(0.8f, 0.8f), 0.2f);
                TweenUtils.SetEase(tween, EaseType.ExpoIn);
                TweenUtils.OnComplete(tween,DoHide);
                break;
            case "fade_out"://淡出
                _window.alpha = 0;
                tween = _window.TweenFade(1,0.3f);
                TweenUtils.SetEase(tween, EaseType.BackOut);
                TweenUtils.OnComplete(tween,DoHide);
                break;
            case "move_down"://下滑退出
                tween = _window.TweenMoveY(GRoot.inst.height + 30,0.3f);
                TweenUtils.SetEase(tween, EaseType.QuadOut);
                TweenUtils.OnComplete(tween,DoHide);
                break;
            case "move_left"://左滑动退出
                tween = _window.TweenMoveX(-_window.width-30,0.3f);
                TweenUtils.SetEase(tween, EaseType.QuadOut);
                TweenUtils.OnComplete(tween,DoHide);
                break;
            case "move_right"://右滑退出
                tween = _window.TweenMoveX(-_window.width+30,0.3f);
                TweenUtils.SetEase(tween, EaseType.QuadOut);
                TweenUtils.OnComplete(tween,DoHide);
                break;
            default:
                DoHide();
                break;
        }
    }

    public void CallOnShown()
    {
        Debug.Log("BaseWindow:CallOnShown");
        _window.SetScale(1,1);
        _window.alpha = 1;
        _window.xy = _originPos;
    }
    
    public void DoHide()
    {
        Debug.Log("BaseWindow:CallOnShown");
        _window.SetScale(1,1);
        _window.alpha = 1;
        _window.xy = _originPos;
        _window.HideImmediately();
        OnHide();
    }
    
    public void ReadyToUpdate(){}
    
    /// <summary>
    /// 内部接口 打开窗口
    /// </summary>
    public void Show(params object[] paramList)
    {
        if (_contentPane == null)
        {
            Create();
        }

        if (_window.isShowing)
        {
            OnBindAction();
            _window.Show();
            OnShow(paramList);
        }
    }
    /// <summary>
    /// 内部接口 隐藏窗口
    /// </summary>
    public void Hide()
    {
        UnBindAction();
        DoHideAnimation();
    }
    
    /// <summary>
    /// 内部接口 销毁窗口
    /// </summary>
    public void Destroy()
    {
        _window.Dispose();
        _window = null;
        OnDestroy();
    }

    public override bool Equals(object obj)
    {
        BaseWindow tempObj = (BaseWindow) obj;
        return tempObj._window.name == this._window.name;
    }

    //子类复写接口，窗口初始化，这个函数调用时窗口第一次生成
    protected virtual void OnInit() {}
    //子类复写接口，窗口事件绑定，这个函数调用时窗口将要显示,一般在此绑定事件
    protected virtual void OnBindAction() {}
    //子类复写接口，窗口显示时，这个函数调用时窗口已经显示
    protected virtual void OnShow(params object[] paramList) {}
    //子类复写接口，窗口事件卸载，这个函数调用时窗口即将隐藏
    protected virtual void UnBindAction() {}
    //子类复写接口，窗口隐藏时操作，这个函数调用时窗口已经隐藏
    protected virtual void OnHide() {}
    //子类复写接口，显示对象被销毁时触发，这个函数调用时窗口已经销毁
    protected virtual void OnDestroy() {}
    
}
