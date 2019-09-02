using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UIRegister
{
    public static void RegisterUI()
    {
        UIManager.AddUIPackage("Common");
        UIManager.Instance.Register<LoginWindow>(UIConstant.LOGINWINDOW);
    }
}
