using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroll : MonoBehaviour
{
    // Start is called before the first frame update
   
    public float xPos;
    public float ScrollY = 0.5f;
    public float OffsetY;
    public float timer = 6;
    public float time;
    public float scrollTime = 2f;

    void Start(){
        time = timer;
    }
    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        Scroll();
        //float OffsetY = Time.time * ScrollY;
        //GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xPos, OffsetY);
    }

    void Scroll(){
        if(time < scrollTime){
            OffsetY += Time.deltaTime * ScrollY;
            GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xPos, OffsetY);
        }
        if(time < 0){
            time = timer;
        }
    }
}