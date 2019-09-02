using UnityEngine;

public class LoginWindow : BaseWindow
{
    protected override void OnInit()
    {
        base.OnInit();
        SetContentSource("Login","Main");
    }

    protected override void OnBindAction()
    {
        base.OnBindAction();
        Debug.Log("OnBindAction");
    }

    protected override void OnShow(params object[] paramList)
    {
        base.OnShow(paramList);
        Debug.Log("onshow");
    }
    

    protected override void UnBindAction()
    {
        base.UnBindAction();
        Debug.Log("UnBindAction");
    }
    
    
    protected override void OnHide()
    {
        base.OnHide();
        Debug.Log("OnHide");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("OnDestroy");
    }

}
