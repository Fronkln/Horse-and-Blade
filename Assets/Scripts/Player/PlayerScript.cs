using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private RootScript root = null;
    [HideInInspector]public Animator anim = null;
    [HideInInspector] public Animator horseAnimator = null;

    public AudioClip crashSound = null;

    private Transform swordSwingPoint = null;
    public Transform swordRoot = null;

    public float attackRange = 0.1f;

    public bool dead = false;
    private bool deadDoOnce = false;

    [HideInInspector]public Vector3 startPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;

    [HideInInspector]public Quaternion startRot;
    
    [HideInInspector]public AudioSource horseRun;
    [HideInInspector]public AudioSource swordSlash;


    public void OnLevelStarted()
    {
        dead = false;
        deadDoOnce = false;

        transform.position = startPos;

        horseRun.Play();
        horseAnimator.Play("run", 0, 0);
    }

    public void OnLevelFinished()
    {
        horseRun.Stop();
        horseAnimator.Play("idle", 0, 0);
    }

    void Awake()
    {
        dead = false;
        deadDoOnce = false;

        startPos = transform.position;
        startRot = transform.rotation;
        targetPos = transform.position;

        anim = GetComponent<Animator>();

        swordSwingPoint = transform.Find("swordSwingPoint");

        horseRun = transform.Find("horse").GetComponent<AudioSource>();
        horseRun.Stop();

        horseAnimator = horseRun.GetComponent<Animator>();

        swordSlash = swordRoot.GetComponent<AudioSource>();
    }


    void Start()
    {
        root = RootScript.instance;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.tag == "Obstacle" || coll.transform.name.Contains("Spearman"))
        {
            print(coll.transform.name);

            print("DET");
            dead = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (dead && !deadDoOnce)
        {
            horseAnimator.Play("idle", 0, 0);
            horseRun.Stop();
            horseRun.volume = 0.6f;
            horseRun.PlayOneShot(crashSound);
            root.OnPlayerDeath();
            deadDoOnce = true;
        }


        if (!dead && !root.levelFinished && root.levelStarted) 
            transform.position += transform.forward * Time.deltaTime * 16;  /*Vector3.Lerp(transform.position, targetPos, 0.2f);*/
    }
}
