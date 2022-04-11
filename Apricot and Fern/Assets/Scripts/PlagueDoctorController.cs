using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlagueDoctorController : NPC
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;

    [SerializeField] DialogObject[] dialogOpeners;

    bool running;

    // Start is called before the first frame update
    //void Start()
    //{
    //    running = true;

    //    //StartCoroutine(Wait());
    //}

    //IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(10f);

    //    running = false;
    //}

    public override void Interacted()
    {
        StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

        transform.LookAt(player.transform);

        int opener = (int)(Random.value * dialogOpeners.Length);
        if (opener >= dialogOpeners.Length)
            opener--;

        DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();
        dialogScreen.StartConversation(dialogOpeners[opener], "Dandelion", animator);
    }
}
