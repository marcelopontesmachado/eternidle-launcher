using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;

public class LauncherController : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI localVersionText;
    public UnityEngine.UI.Button playButton;
    public UnityEngine.UI.Button updateButton;

    private string versionFilePath;
    private string gameDir;
    private string tempDir;

    private string manifestUrl =
        "https://raw.githubusercontent.com/marcelopontesmachado/eternidle-launcher/main/updates/version.json";

    void Start()
    {
        versionFilePath = Path.Combine(Application.streamingAssetsPath, "version.txt");
        gameDir = Path.Combine(Application.dataPath, "..", "Game");
        tempDir = Path.Combine(Application.dataPath, "..", "temp");

        playButton.interactable = false;
        updateButton.interactable = false;

        StartCoroutine(CheckForUpdates());
    }

    IEnumerator CheckForUpdates()
    {
        statusText.text = "Verificando atualizações...";

        UnityWebRequest req = UnityWebRequest.Get(manifestUrl);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            statusText.text = "Erro ao verificar versão.";
            yield break;
        }

        UpdateManifest manifest =
            JsonUtility.FromJson<UpdateManifest>(req.downloadHandler.text);

        string localVersion = LoadLocalVersion();
        localVersionText.text = "Versão instalada: " + localVersion;

        if (localVersion != manifest.latestVersion)
        {
            statusText.text = "Atualização disponível: " + manifest.latestVersion;
            updateButton.interactable = true;
            updateButton.onClick.AddListener(() =>
                StartCoroutine(DownloadAndInstall(manifest))
            );
        }
        else
        {
            statusText.text = "Jogo atualizado.";
            playButton.interactable = true;
        }
    }

    IEnumerator DownloadAndInstall(UpdateManifest manifest)
    {
        updateButton.interactable = false;
        statusText.text = "Baixando atualização...";

        Directory.CreateDirectory(tempDir);

        string zipPath = Path.Combine(tempDir, "client.zip");

        UnityWebRequest download = UnityWebRequest.Get(manifest.windows.url);
        download.redirectLimit = 10;
        download.downloadHandler = new DownloadHandlerFile(zipPath);
        yield return download.SendWebRequest();

        if (download.result != UnityWebRequest.Result.Success)
        {
            statusText.text = "Erro no download.";
            yield break;
        }

        if (!ValidateSHA256(zipPath, manifest.windows.sha256))
        {
            statusText.text = "Arquivo corrompido (hash inválido).";
            yield break;
        }

        FileInfo fileInfo = new FileInfo(zipPath);
        if (fileInfo.Length < 1000000) // menos de 1 MB
        {
            statusText.text = "Download inválido (arquivo muito pequeno).";
            yield break;
        }

        statusText.text = "Instalando...";

        if (Directory.Exists(gameDir))
            Directory.Delete(gameDir, true);

        ZipFile.ExtractToDirectory(zipPath, gameDir);

        SaveLocalVersion(manifest.latestVersion);

        statusText.text = "Atualização concluída!";
        playButton.interactable = true;
    }



    string LoadLocalVersion()
    {
        if (!File.Exists(versionFilePath))
            return "0.0.0";

        return File.ReadAllText(versionFilePath).Trim();
    }

    void SaveLocalVersion(string version)
    {
        File.WriteAllText(versionFilePath, version);
    }

    bool ValidateSHA256(string filePath, string expectedHash)
    {
        using (var sha = SHA256.Create())
        using (var stream = File.OpenRead(filePath))
        {
            byte[] hash = sha.ComputeHash(stream);
            string fileHash = System.BitConverter.ToString(hash).Replace("-", "");

            return fileHash.Equals(expectedHash, System.StringComparison.OrdinalIgnoreCase);
        }
    }

    public void OnPlayClick()
    {
        string exePath = Path.Combine(gameDir, "EternidleClient", "EternidleClient.exe");

        if (File.Exists(exePath))
        {
            System.Diagnostics.Process.Start(exePath);
            Application.Quit();
        }
        else
        {
            statusText.text = "Executável não encontrado.";
        }
    }
}
