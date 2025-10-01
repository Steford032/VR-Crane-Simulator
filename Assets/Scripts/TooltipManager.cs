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
        // ���� ���� �������� �������� �� �������, �������� �
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
        // ������ ����������� �������, ��������� �������� � ���������
        hideCoroutine = StartCoroutine(HideTooltipRoutine());
    }

    private IEnumerator HideTooltipRoutine()
    {
        // ���� ��������� ���������� ������
        yield return new WaitForSeconds(hideDelay);

        // ������ ������
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }

        hideCoroutine = null;
    }
}