using UnityEngine;
using TMPro;
using System.IO;

public class LauncherController : MonoBehaviour
{
    public TextMeshProUGUI localVersionText;

    private string versionFilePath;

    void Start()
    {
        versionFilePath = Path.Combine(Application.streamingAssetsPath, "version.txt");
        LoadLocalVersion();
    }

    void LoadLocalVersion()
    {
        if (File.Exists(versionFilePath))
        {
            string version = File.ReadAllText(versionFilePath).Trim();
            localVersionText.text = "Versão instalada: " + version;
        }
        else
        {
            localVersionText.text = "Versão instalada: desconhecida";
        }
    }

    public void OnPlayClick()
    {
        Debug.Log("Jogar clicado → (Ainda não inicia o jogo)");
        // Nas próximas etapas vamos iniciar o executável do jogo aqui
    }

    public void OnUpdateClick()
    {
        Debug.Log("Atualizar clicado → (Ainda não baixa nada)");
        // Nas próximas etapas implementaremos o sistema real de update
    }
}
