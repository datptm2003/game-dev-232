using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputFieldGrabber : MonoBehaviour
{
    private string inputText; // Store input text here

    [SerializeField] private GameObject reactionGroup;
    [SerializeField] private TMP_Text reactionTextBox;

    public void GrabFromInputField(string input)
    {
        inputText = input;
        DisplayReactionToInput();
    }
    public string GetInputText()
    {
        return inputText;
    }


    private void DisplayReactionToInput()
    {
        reactionTextBox.text = inputText;
        reactionGroup.SetActive(true);
    }
}
