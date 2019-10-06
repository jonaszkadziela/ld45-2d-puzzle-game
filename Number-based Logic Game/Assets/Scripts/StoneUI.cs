using UnityEngine;
using TMPro;

[RequireComponent(typeof(Stone))]
public class StoneUI : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    private Stone stone;

    void Start()
    {
        stone = GetComponent<Stone>();
    }

    void Update()
    {
        if (stone)
        {
            numberText.text = stone.number.ToString();
        }
    }
}
