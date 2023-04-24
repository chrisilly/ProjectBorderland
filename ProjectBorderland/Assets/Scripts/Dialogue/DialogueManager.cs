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

    private Coroutine displayLineCoroutine;

    private bool canContinue;

    private void Awake()
    {
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
        
    }
}
