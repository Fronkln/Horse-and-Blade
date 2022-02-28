using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrapScript : MonoBehaviour
{
    private RootScript root;
    private Animator anim;

    private bool playerTrapped = false;
    private bool playerWasDead = false;

    public void Start() => root = RootScript.instance;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void TrapKill()
    {
        root.player.dead = true;
        anim.Play("beartrap_close", 0, 0);
    }


    void Update()
    {
        if (root.player.dead)
            playerWasDead = true;

        if (playerWasDead)
            if (!root.player.dead)
            {
                anim.Play("beartrap_idle", 0, 0);
                playerTrapped = false;
                playerWasDead = false;
            }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !playerTrapped)
        {
            playerTrapped = true;
            Invoke("TrapKill", 0.3f);
        }

    }
}
