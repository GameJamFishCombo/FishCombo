using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAnimation : MonoBehaviour
{
    // Start is called before the first frame update
   
    public float timer = 0.7f;
    public float time;

    public GameObject record1;
    public GameObject record2;
    public GameObject record3;

    public Transform lerpPos1;
    public Transform lerpPos2;
    public Transform lerpPos3;

    public Transform recordModel1;
    public Transform recordModel2;
    public Transform recordModel3;

    public bool lerp1 = false;
    public bool lerp2 = false;
    public bool lerp3 = false;

    void Start()
    {
        time = timer;
    }

    // Update is called once per frame
    void Update()
    {
        time  -= Time.deltaTime;

        if(time <= 0.65f){
            record1.GetComponent<Animator>().Play("Grow");
        }
        if(time <= 0.6f){
            record2.GetComponent<Animator>().Play("Grow");
        }
        if(time <= 0.55f){
            record3.GetComponent<Animator>().Play("Grow");
        }
        if(!lerp1 && time <= 0.13)
        AudioManager.PlaySound("ThrowRecord1");
        if(!lerp1 && time <= 0.1){
            AudioManager.PlaySound("ThrowRecord1");
            StartCoroutine(LerpPosition(recordModel1.transform,lerpPos1.position,0.2f));

            lerp1 = true;
        }
         if(!lerp2 && time <= 0.08)
        AudioManager.PlaySound("ThrowRecord2");
        if(!lerp2 && time <= 0.05){
            AudioManager.PlaySound("ThrowRecord2");
            StartCoroutine(LerpPosition(recordModel2.transform,lerpPos2.position,0.2f));

            lerp2 = true;
        }
        if(!lerp3 && time <= 0.03)
        AudioManager.PlaySound("ThrowRecord3");
        if(!lerp3 && time <= 0){
            lerp3 = true;
            StartCoroutine(LerpPosition(recordModel3.transform,lerpPos3.position,0.2f));
        }
    }

IEnumerator LerpPosition(Transform recordTransform, Vector3 targetPosition, float duration)
    {
        float lerptime = 0;
        Vector3 startPosition = recordTransform.position;

        while (lerptime < duration)
        {
            recordTransform.position = Vector3.Lerp(startPosition, targetPosition, lerptime / duration);
            lerptime += Time.deltaTime;
            yield return null;
        }
        recordTransform.position = targetPosition;
    }
}