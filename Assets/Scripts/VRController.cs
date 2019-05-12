using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class VRController : MonoBehaviour
{

    public float m_Sensitivity = 0.1f;
    public float m_MaxSpeed = 1.0f;

    public SteamVR_Action_Vector2 m_MoveValue = null;
    public SteamVR_Action_Boolean jumpButton = null;
    public SteamVR_Action_Single trigger = null;

    private float m_Speed = 2.0f;

    private CharacterController m_CharacterController = null;
    private Transform m_CameraRig = null;
    private Transform m_Head = null;
    private Vector3 d = Vector3.zero;
    public float jumpSpeed = 8.0f;

    public ParticleSystem ps;

    public float gravity = 20.0f;

    public SteamVR_Behaviour_Pose leftHand;
    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_CameraRig = SteamVR_Render.Top().origin;
        m_Head = SteamVR_Render.Top().head;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHead();
        CheckForMovement();
        CheckForJetpack();
    }


    private void CheckForMovement()
    {
        if (m_CharacterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes
            Vector3 directionToMove = CalculateMovement();
            d = new Vector3(directionToMove.x, 0.0f, directionToMove.z);
            d *= m_Speed;

            if (jumpButton.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                d.y = jumpSpeed;
            }
        }

        d.y -= gravity * Time.deltaTime;

        // Move the controller
        m_CharacterController.Move(d * Time.deltaTime);
    }

    private void CheckForJetpack()
    {
        if (trigger.GetAxis(SteamVR_Input_Sources.LeftHand) >= 0.9)
        {
            ParticleSystem.EmissionModule em = ps.emission;
            em.enabled = true;
            // d = new Vector3(0, 1, 0); //Straight jump 
            d = leftHand.transform.forward; //Jetpack
            m_CharacterController.Move(d * Time.deltaTime);
        }
        else {
            ParticleSystem.EmissionModule em = ps.emission;
            em.enabled = false;

        }
    }

    private void HandleHead()
    {
        //Store current 
        Vector3 oldPosition = m_CameraRig.position;
        Quaternion oldRotation = m_CameraRig.rotation;

        //rotation 
        transform.eulerAngles = new Vector3(0.0f, m_Head.rotation.eulerAngles.y, 0.0f);

        //restore
        m_CameraRig.position = oldPosition;
        m_CameraRig.rotation = oldRotation;
    }

    private Vector3 CalculateMovement()
    {
        
        Vector3 movementDirection = Vector3.zero;

        if (m_CharacterController.isGrounded)
        {
            if (m_MoveValue.GetAxis(SteamVR_Input_Sources.LeftHand) != Vector2.zero)
            {
               // movementDirection = new Vector3(m_MoveValue.GetAxis(SteamVR_Input_Sources.LeftHand).x, 0, m_MoveValue.GetAxis(SteamVR_Input_Sources.LeftHand).y) * m_Head.transform.forward.z;
                movementDirection = new Vector3(m_Head.transform.forward.x, 0, m_Head.forward.z) * m_MoveValue.GetAxis(SteamVR_Input_Sources.LeftHand).y;
            }
        }

        return movementDirection;
    }

}
