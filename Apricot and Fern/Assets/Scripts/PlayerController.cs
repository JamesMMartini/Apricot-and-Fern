using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] LayerMask worldLayerMask;
    [SerializeField] LayerMask uiLayerMask;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] DialogScreen dialogScreen;

    LayerMask activeLayerMask;

    private void Awake()
    {
        activeLayerMask = worldLayerMask;
    }

    public void OnInteract(InputValue value)
    {
        PlayerClick();
    }

    void PlayerClick()
    {
        // Raycast and find the object here
        RaycastHit hit;

        if (activeLayerMask == worldLayerMask)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f, activeLayerMask))
            {
                if (hit.collider.tag == "NPC")
                {
                    hit.collider.GetComponent<NPC>().Interacted();
                }
            }
        }
        else
        {
            dialogScreen.UIClick();
        }
    }

    public IEnumerator FocusOnObject(GameObject target)
    {
        playerInput.SwitchCurrentActionMap("MouseCursor");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        activeLayerMask = uiLayerMask;
        transform.LookAt(target.transform);
        yield return null;

        //Vector3 current = transform.rotation.eulerAngles;
        //Vector3 newRotation = (target.transform.position - transform.position).normalized;

        //float t = 0;
        //float speed = 1f;
        //while (t < 1f)
        //{
        //    t += Time.deltaTime * speed;
        //    transform.rotation = Quaternion.Euler(Vector3.Lerp(current, newRotation, t));
        //    yield return null;
        //}
    }

    public void Unfocus()
    {
        playerInput.SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeLayerMask = worldLayerMask;
    }
}
