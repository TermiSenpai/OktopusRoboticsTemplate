using UnityEngine;


public class ScreenshotManager : MonoBehaviour
{
    [SerializeField, Tooltip("Tecla modificable para sacar captura de pantalla")] KeyCode screenshotKey;

    private void Update()
    {
        if (Input.GetKeyDown(screenshotKey))        
            TakeScreenshot();
        
    }

    public void TakeScreenshot()
    {
        string desktopFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string baseName = "CapturaDePantalla";
        string extension = ".png";
        string timeData = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");

        // Construir el nombre de archivo único
        string fileName = baseName + "_" + timeData + extension;
        string screenshotPath = System.IO.Path.Combine(desktopFolder, fileName);

        // Capturar la pantalla y guardar la imagen en un archivo
        ScreenCapture.CaptureScreenshot(screenshotPath);
    }
}
