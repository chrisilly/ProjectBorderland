using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideTilemapBarrier : MonoBehaviour
{
    bool tilemapIsHidden;
    // Start is called before the first frame update
    void Awake()
    {
       this.gameObject.GetComponent<TilemapRenderer>().enabled = false; 
        tilemapIsHidden = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (tilemapIsHidden)
        {
            if(Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                this.gameObject.GetComponent<TilemapRenderer>().enabled = true;
                tilemapIsHidden=false;
            }
        }
        else if (!tilemapIsHidden) 
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                this.gameObject.GetComponent<TilemapRenderer>().enabled = false;
                tilemapIsHidden = true;
            }
        }
    }
}
