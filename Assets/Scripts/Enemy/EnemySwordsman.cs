using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordsman : BaseEnemy
{
    //Spear enemies don't have their own code, you can just enable spearmanMode to quickly turn them into a spear enemy however
    public bool spearmanMode = false;

    private Animator anim = null;
    private AudioSource swordSlashSound = null;


    public override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        swordSlashSound = GetComponent<AudioSource>();

        if (spearmanMode)
        {
            anim.Play("spearman_idle", 0, 0);
            transform.tag = "Untagged"; //Keeping tag as Enemy would mean Spearman would get killed
        }

    }

    void Update()
    {
        if (!root.levelStarted || !modelrenderer.isVisible) return;

        Vector3 dir = transform.forward * attackRange;
        Debug.DrawRay(transform.position + new Vector3(0, 2, 0), dir);

        if (PlayerXClose(5.5f))
        {
            if (zDistanceToPlayer <= attackRange)
            {
                if (!spearmanMode)
                    FaceThePlayerSlerp();
                else
                    FaceThePlayerInstant();

                if (player.dead) return;

                if (canAttack)
                {

                    attackCooldown = attackSpeed;

                    if (swordSlashSound != null && swordSlashSound.clip != null)
                        swordSlashSound.Play();

                    if (spearmanMode)
                        anim.Play("spearman_attack", 0, 0);
                    else
                        anim.Play("enemy_swing", 0, 0);

                    OnSwordsmanAttackFrame();
                }

            }

        }
    }


    //i didnt really bother checking if the player would actually get hit, 
    //if they are in attack range they would get hit anyway
    public void OnSwordsmanAttackFrame()
    {
        player.dead = true;
    }
}