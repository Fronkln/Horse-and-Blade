using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for enemies, enemies can inherit this class
//
//The benefits of using zDistanceToPlayer instead of Vector3.Distance is that we avoid expensive
//square root calculations.
//
//PlayerXClose bool method returns whether the enemy is close to the player in terms of left-right position
//I ended up mostly no longer using but i still keep it just in case
//


public class BaseEnemy : MonoBehaviour
{

    [HideInInspector] public RootScript root;
    [HideInInspector] public PlayerScript player;

    public bool overrideAutoAttackCooldown = false;

    [Header("General Properties")]
    public float zDistanceToPlayer = 0;
    public float attackRange = 3;
    public float attackSpeed = 3; //seconds

    [HideInInspector] public float attackCooldown = 0;

    [Header("Moving Enemy Properties")]
    public bool movingEnemy = false;
    public float zDistanceNeededBeforeRunning = 20;
    public float moveSpeed = 1.5f;

    public bool canAttack { get { return attackCooldown == 0; } }


    [HideInInspector]public Vector3 startPos;
    [HideInInspector]public Quaternion startRot;


    [HideInInspector] public SkinnedMeshRenderer modelrenderer;


    public virtual void Start()
    {

        modelrenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        startPos = transform.localPosition;
        startRot = transform.rotation;

        root = RootScript.instance;
        player = root.player;
    }

    public bool PlayerXClose(float x)
    {
        float diff = transform.position.x - player.transform.position.x;

        if (diff < 0)
            return diff >= -x;
        else
            return diff <= x;
    }


    public void FaceThePlayerSlerp()
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0f;


        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
    }

    public void FaceThePlayerInstant()
    {
        Vector3 lookPos = player.transform.position - transform.position;
        lookPos.y = 0f;

        Quaternion rotation = Quaternion.LookRotation(lookPos);

        transform.rotation = rotation;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        if (movingEnemy && !player.dead && root.levelStarted && !root.levelFinished)
        {
            if (zDistanceToPlayer <= zDistanceNeededBeforeRunning)
            {
                //Woops, i forgot to make the enemies actually face forward so they would go backwards if i didnt use
                //-transform.forward, adjust this if you replace enemy models etc

                //This moving forward code is buggy, got no clue why...

                transform.Translate(-transform.forward * Time.deltaTime * moveSpeed );
                FaceThePlayerInstant();

            }
        }

        zDistanceToPlayer = transform.position.z - player.transform.position.z;

        if (zDistanceToPlayer < 0)
            zDistanceToPlayer = -zDistanceToPlayer;


        if (attackCooldown > 0)
        {
            if (!overrideAutoAttackCooldown)
                attackCooldown -= Time.deltaTime;

            if (attackCooldown < 0)
                attackCooldown = 0;
        }
    }
}
