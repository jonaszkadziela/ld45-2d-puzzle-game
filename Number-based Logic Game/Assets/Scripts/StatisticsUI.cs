using UnityEngine;
using TMPro;

public class StatisticsUI : MonoBehaviour
{
    public TextMeshProUGUI energyValueText;
    public TextMeshProUGUI moneyValueText;
    public TextMeshProUGUI timeValueText;

    void Update()
    {
        energyValueText.text = ((int)PlayerController.Instance.energy).ToString();
        moneyValueText.text = PlayerController.Instance.money.ToString();
        timeValueText.text = "0:00"; // TODO: Implement time display
    }
}
