using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlagueDoctorController : NPC
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;

    bool running;

    // Start is called before the first frame update
    void Start()
    {
        running = true;

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);

        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interacted()
    {
        StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

        transform.LookAt(player.transform);
    }
}
