using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera gameCamera;
    [SerializeField] LayerMask worldLayerMask;
    [SerializeField] LayerMask uiLayerMask;
    [SerializeField] PlayerInput playerInput;

    public void OnInteract(InputValue value)
    {
        Debug.Log("Clicked");

        PlayerClick();
    }

    void PlayerClick()
    {
        // Raycast and find the object here
        RaycastHit ray;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out ray, 10f, worldLayerMask))
        {
            if (ray.collider.tag == "NPC")
            {
                ray.collider.GetComponent<NPC>().Interacted();
            }
        }
    }

    public IEnumerator FocusOnObject(GameObject target)
    {
        playerInput.SwitchCurrentActionMap("MouseCursor");
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
}
