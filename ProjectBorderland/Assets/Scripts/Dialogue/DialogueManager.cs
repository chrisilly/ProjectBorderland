using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _continueButton;

    private Story _currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    [SerializeField] float _typingSpeed;
    [SerializeField] float _defualtSpeed;

    private Coroutine displayLineCoroutine;

    private bool canContinue;

    private void Awake()
    {
        _typingSpeed = _defualtSpeed;
        _dialoguePanel.SetActive(false);
        canContinue = false;
        _continueButton.SetActive(false);
        if(instance != null)
        {
            Debug.Log("Found more than one dialogue manager in the scence");
        }

        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            DialogueTrigger.setBoolFalse();
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canContinue)
            {
                ContinueStory();
            }
            else
            {
                _typingSpeed = float.MinValue;
            }
        }

        if (dialogueIsPlaying)
        {
            if (canContinue)
            {
                _continueButton.SetActive(true);
            }
            else
            {
                _continueButton.SetActive(false);
            }
        }

    }

    IEnumerator DisplayLine(string line)
    {
        _dialogueText.text = "";
        canContinue = false;
        foreach(char c in line.ToCharArray())
        {
            _dialogueText.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }

        canContinue = true;
    }

    public void EnterDialogueMode(TextAsset inkJson)
    {
        _currentStory = new Story(inkJson.text);
        dialogueIsPlaying = true;
        _dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        _dialoguePanel.SetActive(false);
        canContinue = false;
        _continueButton.SetActive(false);
        _dialogueText.text = "";
    }

    private void ContinueStory()
    {
        _typingSpeed = _defualtSpeed;

        if (_currentStory.canContinue)
        {
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(_currentStory.Continue()));
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
