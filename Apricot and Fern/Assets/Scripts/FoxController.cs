using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : NPC
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;

    [SerializeField] DialogObject openingLine;

    public override void Interacted()
    {
        StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

        DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();
        dialogScreen.StartConversation(openingLine, "Tae", animator);
    }
}
