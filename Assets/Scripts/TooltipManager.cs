using System.Collections;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private float hideDelay = 0.1f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
    }

    public void ShowTooltip(string text)
    {
        // Если была запущена корутина на скрытие, отменяем её
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        if (tooltipPanel != null && tooltipText != null)
        {
            tooltipText.text = text;
            tooltipPanel.SetActive(true);
        }
    }

    public void HideTooltip()
    {
        // Вместо мгновенного скрытия, запускаем корутину с задержкой
        hideCoroutine = StartCoroutine(HideTooltipRoutine());
    }

    private IEnumerator HideTooltipRoutine()
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(hideDelay);

        // Прячем панель
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }

        hideCoroutine = null;
    }
}