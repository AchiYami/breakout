using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisableButtonIfEmpty : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;

    [SerializeField] private Button button;


    //Disables button on empty input field.
    private void Update()
    {
        button.interactable = input.text.Length > 0;
    }
}