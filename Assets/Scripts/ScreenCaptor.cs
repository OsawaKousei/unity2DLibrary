using System;
using System.IO;
using UnityEngine;

/*
 * 仕様：指定カメラのスクリーンショットをPNGで保存
 * 使い方：任意の方法でcaptureScreen()を呼び出し
 * 注意事項：ファイル名は.pngまで記述
 */
public class ScreenCaptor : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private string fileName;

    public string filePath;
    
    private string stCurrentDir = Directory.GetCurrentDirectory();
    void Start()
    {
        filePath = stCurrentDir + fileName;
    }

    void Update()
    {

    }

    public void captuerScreen()
    {
        try
        {
            var rt = new RenderTexture(_camera.pixelWidth, _camera.pixelHeight, 24);
            var prev = _camera.targetTexture;
            _camera.targetTexture = rt;
            _camera.Render();
            _camera.targetTexture = prev;
            RenderTexture.active = rt;

            var screenShot = new Texture2D(
                _camera.pixelWidth,
                _camera.pixelHeight,
                TextureFormat.RGB24,
                false);
            screenShot.ReadPixels(new Rect(0, 0, screenShot.width, screenShot.height), 0, 0);
            screenShot.Apply();

            var bytes = screenShot.EncodeToPNG();
            Destroy(screenShot);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("PNGexported");
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
        
}