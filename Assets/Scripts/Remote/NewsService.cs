using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class NewsData
{
    public NewsEntry latest;
}

[System.Serializable]
public class NewsEntry
{
    public string title;
    public string date;
    public string[] items;
}

public class NewsService : MonoBehaviour
{
    public static NewsEntry Current;

    private string url =
        "https://raw.githubusercontent.com/marcelopontesmachado/eternidle-launcher/main/updates/news.json";

    public IEnumerator Load()
    {
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao carregar news");
            yield break;
        }

        NewsData data = JsonUtility.FromJson<NewsData>(req.downloadHandler.text);
        Current = data.latest;

        Debug.Log("News carregada: " + Current.title);
    }
}
