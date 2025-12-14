using UnityEngine;
using System.Collections;

public class LauncherBootstrap : MonoBehaviour
{
    private StatusService statusService;
    private NewsService newsService;
    private LinksService linksService;

    private StatusPanel statusPanel;
    private NewsPanel newsPanel;

    void Start()
    {
        statusService = FindObjectOfType<StatusService>();
        newsService = FindObjectOfType<NewsService>();
        linksService = FindObjectOfType<LinksService>();

        statusPanel = FindObjectOfType<StatusPanel>();
        newsPanel = FindObjectOfType<NewsPanel>();

        if (statusService == null || newsService == null || linksService == null)
        {
            Debug.LogError("Serviços remotos não encontrados.");
            return;
        }

        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return StartCoroutine(statusService.Load());
        yield return StartCoroutine(newsService.Load());
        yield return StartCoroutine(linksService.Load());

        // Atualiza UI APÓS os dados existirem
        if (statusPanel != null) statusPanel.Refresh();
        if (newsPanel != null) newsPanel.Refresh();

        Debug.Log("Launcher inicializado com sucesso");
    }
}
