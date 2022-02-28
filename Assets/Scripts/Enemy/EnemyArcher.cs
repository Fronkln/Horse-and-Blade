using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : BaseEnemy
{
    private Animator anim;

    private GameObject arrowPrefab;
    private Transform arrowShootPoint;

    private AudioSource bowShoot;


    public void Awake()
    {
        anim = GetComponent<Animator>();
        anim.Play("archer_idle", 0, 0);

        arrowPrefab = Resources.Load<GameObject>("arrow");
        arrowShootPoint = transform.Find("shootPoint");

        overrideAutoAttackCooldown = true;

        bowShoot = GetComponent<AudioSource>();

    }

    public override void Start()
    {
        base.Start();
    }

    public void Reset() => attackRange = 45;

    // Update is called once per frame
    void Update()
    {
        if (player.dead || !root.levelStarted || root.levelFinished)
            return;

        if(zDistanceToPlayer <= attackRange)
        {
            //don't shoot if we are running enemy and we are not in running distance
            if (movingEnemy && zDistanceToPlayer > zDistanceNeededBeforeRunning) return;

            if(canAttack)
            {
                attackCooldown = attackSpeed;

                anim.Play("archer_shoot", 0, 0);

                GameObject arrow = GameObject.Instantiate(arrowPrefab);
                arrow.transform.position = arrowShootPoint.transform.position;
                arrow.transform.rotation = arrowShootPoint.transform.rotation;

                bowShoot.Play();
                
            }
            else
            {
                //since the archer has overrideAttackCoodlown true we gotta handle it here

                attackCooldown -= Time.deltaTime;
                if(attackCooldown < 0) attackCooldown = 0;
            }
        }
    }
}
