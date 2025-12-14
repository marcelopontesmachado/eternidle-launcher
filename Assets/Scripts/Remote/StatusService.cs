using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class ServerStatus
{
public string serverName;
    public string status;
    public string message;
    public string[] regions;
    public string lastUpdate;
}

public class StatusService : MonoBehaviour
{
    public static ServerStatus Current;

    private string url =
        "https://raw.githubusercontent.com/marcelopontesmachado/eternidle-launcher/main/updates/status.json";

    public IEnumerator Load()
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao carregar status");
            yield break;
        }

        Current = JsonUtility.FromJson<ServerStatus>(req.downloadHandler.text);
        Debug.Log("Status carregado: " + Current.status);
    }
}
