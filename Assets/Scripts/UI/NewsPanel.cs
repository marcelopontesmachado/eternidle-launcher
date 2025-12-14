using UnityEngine;
using TMPro;
using System.Text;

public class NewsPanel : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI contentText;

    public void Refresh()
    {
        if (NewsService.Current == null) return;

        titleText.text = NewsService.Current.title;
        dateText.text = NewsService.Current.date;

        StringBuilder sb = new StringBuilder();
        foreach (var item in NewsService.Current.items)
            sb.AppendLine("• " + item);

        contentText.text = sb.ToString();
    }
}
