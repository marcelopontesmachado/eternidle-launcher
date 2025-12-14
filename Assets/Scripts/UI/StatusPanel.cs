using UnityEngine;
using TMPro;
using System.Text;

public class StatusPanel : MonoBehaviour
{
    public TextMeshProUGUI serverName;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI regionsText; // opcional
    public TextMeshProUGUI lastUpdateText; // opcional

    public void Refresh()
    {
        if (StatusService.Current == null) return;

        serverName.text = StatusService.Current.serverName;
        statusText.text = StatusService.Current.status;
        messageText.text = StatusService.Current.message;

        if (regionsText != null && StatusService.Current.regions != null)
        {
            regionsText.text = string.Join(" / ", StatusService.Current.regions);
        }

        if (lastUpdateText != null)
        {
            lastUpdateText.text = "Atualizado em: " + StatusService.Current.lastUpdate;
        }
    }
}
