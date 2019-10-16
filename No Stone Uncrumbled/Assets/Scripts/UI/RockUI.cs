using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rock))]
public class RockUI : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    private Rock rock;

    void Start()
    {
        rock = GetComponent<Rock>();
    }

    void Update()
    {
        if (rock)
        {
            numberText.text = rock.number.ToString();
        }
    }
}
