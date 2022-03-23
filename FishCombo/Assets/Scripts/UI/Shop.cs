using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public int itemNum = 1;
    public bool cutscene = true;
    public bool itemSelected = false;
    public float timer = 4.5f;
    public float time = 0f;

    public float timer2 = 3f;
    public float time2 = 3f;

    public GameObject poofParticles;
    public Transform takenItem;

    public AudioSource hoverSound;
    public AudioSource selectSound;

    [Header("UI")]
    public GameObject FadeIn;
    [Header("Health Object Spawn")]
    public GameObject medsPrefab;
    public GameObject meds;
    public Transform medsSpawn;
    public Animator medsAnim;
    public Transform medsLerp;
    
    [Header("Damage Object Spawn")]
    public GameObject syringePrefab;
    public GameObject syringe;
    public Transform syringeSpawn;
    public Animator syringeAnim;
    public Transform syringeLerp;

    public GameObject playerPrefab;
    public Player player;

    public AudioSource music;
    // public string levelToLoad1;
    // public string levelToLoad2;

    static int numVisited = 0;

    void Start()
    {
        time = timer;
        time2 = timer2;
    }

    void SpawnItems(){
        meds = Instantiate(medsPrefab,medsSpawn);
        medsAnim = meds.GetComponentInChildren<Animator>();
        syringe = Instantiate(syringePrefab,syringeSpawn);
        syringeAnim = syringe.GetComponentInChildren<Animator>();
        Instantiate(poofParticles,meds.transform.position,Quaternion.identity);
        Instantiate(poofParticles,syringe.transform.position,Quaternion.identity);
        player = playerPrefab.GetComponent<Player>();
    }

    void Update()
    {
        if(cutscene)
        time -= Time.deltaTime;

        if(itemSelected)
        time2 -= Time.deltaTime;

        if(time <= 1.5 && meds == null){
            SpawnItems();
        }

        if(time <= 0 && cutscene){

            cutscene = false;
            medsAnim.Play("Idle");
            syringeAnim.Play("Idle");
            HoverItemOne();
        }

        if(time2 <= 2.4){
            FadeIn.SetActive(true);
            StartCoroutine(FadeMusic.StartFade(music,2f,0f));
        }

        if(time2 <= 0){
            Debug.Log("Shop Visits: " + numVisited);
            if(numVisited == 0) {
                numVisited++;
                SceneManager.LoadScene("Subway");
            } else if(numVisited == 1) {
                numVisited++;
                SceneManager.LoadScene("CutScene2");
            }
        }


        if(!cutscene && !itemSelected){
            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
                if(itemNum == 1){
                    HoverItemTwo();
                    return;
                }
                if(itemNum == 2){
                    HoverItemOne();
                    return;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            if(!itemSelected)
                SelectItem();
        }
    }

    void HoverItemOne(){
        hoverSound.Play();
        itemNum = 1;
        StartCoroutine(LerpPosition(meds,medsLerp.position,0.2f));
        StartCoroutine(LerpPosition(syringe,syringeSpawn.position,0.2f));
        syringeAnim.SetBool("Selected",false);
        medsAnim.SetBool("Selected",true);
    }
    
    void HoverItemTwo(){
        hoverSound.Play();
        itemNum = 2;
        StartCoroutine(LerpPosition(meds,medsSpawn.position,0.2f));
        StartCoroutine(LerpPosition(syringe,syringeLerp.position,0.2f));
        syringeAnim.SetBool("Selected",true);
        medsAnim.SetBool("Selected",false);
    }

    void SelectItem(){
        itemSelected = true;
        selectSound.Play();
        if(itemNum == 1){
            StartCoroutine(LerpPosition(meds,takenItem.position,0.2f));
            player.IncreaseMaxHP(); 
        }  

        if(itemNum == 2){
            StartCoroutine(LerpPosition(syringe,takenItem.position,0.2f));
            player.IncreaseATK(); 
        }
    }

    IEnumerator LerpPosition(GameObject targetObject, Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = targetObject.transform.position;

        while (time < duration)
        {
            targetObject.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        targetObject.transform.position = targetPosition;
    }
}
