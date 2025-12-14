using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class LauncherWindow : MonoBehaviour
{
    public int width = 1280;
    public int height = 720;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        uint uFlags
    );

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    const uint SWP_NOZORDER = 0x0004;
    const uint SWP_SHOWWINDOW = 0x0040;
#endif

    void Awake()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(width, height, false);
    }

    void Start()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        IntPtr hwnd = GetActiveWindow();

        int posX = (Display.main.systemWidth - width) / 2;
        int posY = (Display.main.systemHeight - height) / 2;

        SetWindowPos(
            hwnd,
            IntPtr.Zero,
            posX,
            posY,
            width,
            height,
            SWP_NOZORDER | SWP_SHOWWINDOW
        );
#endif
    }
}
