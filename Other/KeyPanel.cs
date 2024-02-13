using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPanel : MonoBehaviour
{
    [Header("The Animator of the Door will be unlocked")]
    [SerializeField] Animator _door;
    public bool isAnimated = false;
    public string Description = "Press E to unlock the door.";
    public string passcode;
    [Header("Animation Camera")]
    [SerializeField] GameObject cam;
    public void UnlockDoor()
    {
        cam.SetActive(true);
        _door.SetBool("OpenDoor", true);
        _door.transform.GetComponent<Collider>().enabled = false;
        Description = "Door is unlocked.";
        StartCoroutine("DisableCam");
    }
    IEnumerator DisableCam()
    {
        yield return new WaitForSeconds(1.6f);
        cam.SetActive(false);
        isAnimated = true;
    }
    public bool CheckForPassword(string passcode)
    {
        if (this.passcode == passcode)
        {
            UnlockDoor();
            return true;
        }
        else
            return false;
    }
}
