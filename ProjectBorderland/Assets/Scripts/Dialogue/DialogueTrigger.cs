using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Cisual Cue")]
    [SerializeField] private GameObject _visualCue;

    [Header("Json")]
    [SerializeField] private TextAsset _inkJson;

    [SerializeField] int level;

    private bool _playInRange;

    private static bool _isInDialogue;

    private void Awake()
    {
        _visualCue.SetActive(false);
        _playInRange = false;
    }

    private void Update()
    {

        if(level == 0)
        {
            GetDialogue(_inkJson);

        }

        else if(level == 1)
        {

        }


    }

    private void GetDialogue(TextAsset dialogue)
    {
        if(_playInRange && !_isInDialogue)
        {
            _visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                _isInDialogue = true;
            }
        }
        else
        {
            _visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _playInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playInRange = false;
    }

    public static void setBoolFalse()
    {
        _isInDialogue = false;
    }
}
