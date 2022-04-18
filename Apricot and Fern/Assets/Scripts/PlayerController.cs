using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] LayerMask worldLayerMask;
    [SerializeField] LayerMask uiLayerMask;
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] DialogScreen dialogScreen;
    [SerializeField] GameObject reticle;
    [SerializeField] GameObject prompt;
    [SerializeField] CinemachineVirtualCamera floorTimeCamera;

    LayerMask activeLayerMask;
    public bool inFloorTime;

    private void Awake()
    {
        activeLayerMask = worldLayerMask;
    }

    private void Update()
    {
        if (activeLayerMask == worldLayerMask)
        {
            // Raycast and find the object here
            RaycastHit hit;
            bool didHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f, activeLayerMask);

            if (didHit && !prompt.activeInHierarchy) // Show the prompt
            {
                if (hit.collider.tag == "NPC")
                {
                    reticle.SetActive(false);
                    prompt.SetActive(true);
                }
            }
            else if (didHit && prompt.activeInHierarchy && hit.collider.tag != "NPC") // Hide the prompt
            {
                reticle.SetActive(true);
                prompt.SetActive(false);
            }
            else if (!didHit && prompt.activeInHierarchy) // Hide the prompt
            {
                reticle.SetActive(true);
                prompt.SetActive(false);
            }
        }
    }

    public void OnInteract(InputValue value)
    {
        if (!inFloorTime)
        {
            PlayerClick();
        }
        else
        {
            LeaveFloorTime();
        }
    }

    void PlayerClick()
    {
        // Raycast and find the object here
        RaycastHit hit;

        if (activeLayerMask == worldLayerMask)
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f, activeLayerMask))
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

    public void EnterFloorTime()
    {
        playerInput.SwitchCurrentActionMap("FloorTime");
        floorTimeCamera.Priority += 2;
        inFloorTime = true;
    }

    public void LeaveFloorTime()
    {
        playerInput.SwitchCurrentActionMap("Player");
        floorTimeCamera.Priority -= 2;
        inFloorTime = false;
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
        Vector3 newRotation = new Vector3(0, transform.rotation.y, 0);
        transform.rotation = Quaternion.Euler(newRotation);

        playerInput.SwitchCurrentActionMap("Player");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeLayerMask = worldLayerMask;
    }
}
