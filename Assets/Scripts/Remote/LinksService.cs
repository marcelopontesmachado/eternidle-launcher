using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class LinksData
{
    public string discord;
    public string website;
    public string store;
}

public class LinksService : MonoBehaviour
{
    public static LinksData Current;

    private string url =
        "https://raw.githubusercontent.com/marcelopontesmachado/eternidle-launcher/main/updates/links.json";

    public IEnumerator Load()
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao carregar links");
            yield break;
        }

        Current = JsonUtility.FromJson<LinksData>(req.downloadHandler.text);
        Debug.Log("Links carregados");
    }
}
