using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    
    public Animator anim;
    public Transform otherElevator;


    bool inElevator = false;
    Vector3 locaPos = Vector3.zero;
    Quaternion localRot = Quaternion.identity;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            inElevator = true;
            player = other.transform;
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inElevator = false;
            other.transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(inElevator)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
               

                ShutElevator();
            }
        }
    }

    public void ShutElevator()
    {
        anim.Play("close");

        StartCoroutine("Teleport");
    }

    public void OpenElevator()
    {
        anim.Play("open");
    }

   IEnumerator Teleport()
    {
        yield return new WaitForSeconds(3.0f);
        locaPos = player.localPosition;
        localRot = player.localRotation;

        player.parent = otherElevator;
        player.localPosition = locaPos;
        player.localRotation = localRot;
        otherElevator.GetComponent<ElevatorController>().OpenElevator();

    }
}
