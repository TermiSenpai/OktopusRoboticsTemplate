using UnityEngine;


public class ScreenshotManager : MonoBehaviour
{
    // Tecla para realizar una captura de pantalla
    [SerializeField, Tooltip("Tecla modificable para sacar captura de pantalla")] KeyCode screenshotKey;
    // Nombre del archivo
    [SerializeField, Tooltip("Nombre del archivo. Este nombre será modificado con la fecha para que las capturas no se sobreescriban")] string baseName = "CapturaDePantalla";
    string extension = ".png";
    // busca el escritorio
    string desktopFolder;

    private void Start()
    {        
        //Busca la ruta al escritorio (Debug)
        desktopFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    }

    private void Update()
    {
        // Cada frame comprueba si se ha pulsado la tecla configurada
        if (Input.GetKeyDown(screenshotKey))
            TakeScreenshot();
    }


    // Realiza la captura de pantalla
    public void TakeScreenshot()
    {
        // Se añade el tiempo para que el nombre del archivo siempre sea diferente ( Año-mes-dia-horas-minutos-segundos)
        string timeData = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        // Construir el nombre de archivo único
        string fileName = $"{baseName}-{timeData}{extension}";

        // Se combina la ruta con el nombre
        string screenshotPath = System.IO.Path.Combine(desktopFolder, fileName);

        // Capturar la pantalla y guardar la imagen en un archivo
        ScreenCapture.CaptureScreenshot(screenshotPath);
    }
}
