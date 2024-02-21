using UnityEngine;
using SFB;
using TMPro;
using System.Collections;
using System.IO;


public class ScreenshotManager : MonoBehaviour
{
    // Tecla para realizar una captura de pantalla
    [SerializeField, Tooltip("Tecla modificable para sacar captura de pantalla")] KeyCode screenshotKey;
    // Nombre del archivo
    [SerializeField, Tooltip("Nombre del archivo. Este nombre será modificado con la fecha para que las capturas no se sobreescriban")] string baseName = "CapturaDePantalla";

    // Placeholder
    [SerializeField] private TextMeshProUGUI pathPlaceholder;
    // User input
    [SerializeField] private TMP_InputField pathInput;

    // Extensión de archivo
    const string extension = ".png";
    // busca el escritorio
    string pathFolder;
    // Referencia al canvas
    GameObject CanvasObj;

    private void Start()
    {
        // Referencia al canvas
        CanvasObj = GameObject.FindGameObjectWithTag("UI");
        //Carga la ultima ruta usada, si no existe, añade el escritorio
        pathFolder = PlayerPrefs.GetString("Path", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
        // La ruta del escritorio se guarda
        pathPlaceholder.text = pathFolder;
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
        DisableCanvas();

        // Se añade el tiempo para que el nombre del archivo siempre sea diferente ( Año-mes-dia-horas-minutos-segundos)
        string timeData = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        // Construir el nombre de archivo único
        string fileName = $"{baseName}-{timeData}{extension}";

        // Se combina la ruta con el nombre
        string screenshotPath = Path.Combine(pathFolder, fileName);

        // Comprueba que exista la carpeta, en caso contrario, se crea
        if (!Directory.Exists(pathFolder))
            Directory.CreateDirectory(pathFolder);


        // Capturar la pantalla y guardar la imagen en un archivo
        ScreenCapture.CaptureScreenshot(screenshotPath);
        // Espera a realizar la captura antes de habilitar la UI de nuevo
        StartCoroutine(WaitToEnable());
    }

    public void OnBrowseBtn()
    {
        // Abre el cuadro de diálogo de selección de carpeta.
        string[] paths = StandaloneFileBrowser.OpenFolderPanel("Seleccionar Carpeta", "", false);

        // Verifica si se seleccionó una carpeta antes de actualizar la interfaz de usuario.
        if (paths.Length > 0)
        {
            // Toma la carpeta seleccionada
            pathFolder = paths[0];
            // Añade la ruta al placeholder de la UI
            pathPlaceholder.text = pathFolder;
            // Guarda la ruta por defecto
            SavePath(pathFolder);
        }
    }

    // Posibilidad de escribir la ruta en vez de buscarse
    public void OnInputFieldEndEdit(string txt)
    {
        // Guarda la ruta escrita por el usuario
        pathFolder = txt;
        // Lo añade al placeholder
        pathPlaceholder.text = txt;
        // Se elimina lo escrito para dejar a la vista el placeholder
        pathInput.text = string.Empty;
        // Guarda la ruta por defecto
        SavePath(pathFolder);
    }

    // Desactiva el canvas
    void DisableCanvas() => CanvasObj.SetActive(false);
    // Activa el canvas
    void EnableCanvas() => CanvasObj.SetActive(true);

    // Espera a que la captura finalice al final del frame
    private IEnumerator WaitToEnable()
    {
        yield return new WaitForEndOfFrame();
        EnableCanvas();
    }
    // Guarda la ruta por defecto
    private void SavePath(string save) => PlayerPrefs.SetString("Path", save);

}
