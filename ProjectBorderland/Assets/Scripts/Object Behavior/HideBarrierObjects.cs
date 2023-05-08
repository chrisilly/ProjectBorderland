using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideBarrierObjects : MonoBehaviour
{
    bool objectIsHidden;
    // Start is called before the first frame update
    void Awake()
    {
        if (TryGetComponent<TilemapRenderer>(out TilemapRenderer tilemapRenderer))
        {
            tilemapRenderer.enabled = false;
            objectIsHidden = true;
        }
        else if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            spriteRenderer.enabled = false;
            objectIsHidden = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (objectIsHidden)
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus) && TryGetComponent<TilemapRenderer>(out TilemapRenderer tilemapRenderer))
            {
                tilemapRenderer.enabled = true;
                objectIsHidden = false;
            }
            else if (Input.GetKeyDown(KeyCode.KeypadPlus) && TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.enabled = true;
                objectIsHidden = false;
            }
        }
        else if (!objectIsHidden)
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus) && TryGetComponent<TilemapRenderer>(out TilemapRenderer tilemapRenderer))
            {
                tilemapRenderer.enabled = false;
                objectIsHidden = true;
            }
            else if (Input.GetKeyDown(KeyCode.KeypadPlus) && TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.enabled = false;
                objectIsHidden = true;
            }
        }
    }
}
