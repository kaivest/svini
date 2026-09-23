using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class playerMovementScript : entityData//tower defence slasher 1st person
{
    [Header("movement")]
        public int sensivity;
        [SerializeField] internal Rigidbody playerRb;
        public int stepForce;
        public float flyForce;
        public int maxVelocity;
        public float calculatedVelocity;
        public float currentVelocity;
        public bool grounded;
        
    [Header("pivots")]
        public Transform wearponPivot;
        public Transform playerPivo;
        public Transform cameraPivo;

    
    [Header("triggers")]
        public customTrigger jumpTrigger;
        public customTrigger damageTrigger;

    [Header("masks")]
        public LayerMask whatIsGround;
        public LayerMask iCanDamage;

    [Header("keyCodes")]
        public KeyCode[] moveCodes = { KeyCode.W , KeyCode.A, KeyCode.S,  KeyCode.D,};
        
    [Header("wearpon")]
        public GameObject wearpon;
        public GameObject Fist;
        public GameObject testColumn;
    void Awake()
    {
        jumpTrigger.OnStayed += OnJumpTriggerStayed;
        jumpTrigger.OnExited += OnJumpTriggerExited;
    }

    void Start()
    {
            wearpon = Fist;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void FixedUpdate()
    {
        wearponPivot.transform.rotation =cameraPivo.transform.rotation;
        Move(stepForce, flyForce);
        playerRb.angularVelocity = Vector3.zero;
       // playerRb.AddForce(0,9.81f,0, ForceMode.Force);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        if (!grounded)
        {
        }
        currentVelocity = playerRb.linearVelocity.magnitude;
        moveCam();
        UseWearpon();
        
    }

    void OnJumpTriggerStayed(Collider other)
    {
        if ( other.gameObject.CompareTag("floor")||other.gameObject.CompareTag("svin"))
        {
            grounded = true;
        }
    }

    void OnJumpTriggerExited(Collider other)
    {
        grounded = false;
    }

    void AttemptUseWearpon()
    {
        wearpon.GetComponent<wearponDataScript>().Use(ref wearponPivot);
    }


    void moveCam()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Quaternion rot = Quaternion.Euler(0, mouseX*sensivity, 0);
        playerPivo.rotation *= rot;
        Quaternion camrot = Quaternion.Euler(-mouseY * sensivity, 0, 0);
        Quaternion testCameraRot = cameraPivo.rotation;
        cameraPivo.rotation *= camrot;
    }
    
    void Move( int speed, float flySpeed)
    {
        int[] pressed = { 0, 0, 0, 0 };
        for (int i =0; i<4;i++)
        {
            if (Input.GetKey(moveCodes[i]))
            {
                pressed[i] = 1;
            }
        }
        Vector3 Dir = Vector3.zero;
        Dir.z+=(pressed[0]-pressed[2]);
        Dir.x+=(pressed[3]-pressed[1]);
        Dir = Dir.normalized;
        Vector3 Dir2 = Vector3.zero;
        Dir2.x = Dir.x*math.cos((playerPivo.transform.rotation.eulerAngles.y)/180*math.PI)+Dir.z*math.sin((playerPivo.transform.rotation.eulerAngles.y)/180*math.PI);
        Dir2.z = -Dir.x*math.sin((playerPivo.transform.rotation.eulerAngles.y)/180*math.PI)+Dir.z*math.cos((playerPivo.transform.rotation.eulerAngles.y)/180*math.PI);
        Dir2 = Dir2.normalized;
        Vector3 pos = playerPivo.position;
        pos.y += 1;
        Ray ray = new Ray(pos, new Vector3(0,-1,0));
            RaycastHit hit = new RaycastHit();
            if (Physics.SphereCast(ray, 1.25f,  out hit, 4, whatIsGround))
            {
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Dir2 =  rot*Dir2;
                Dir2= Dir2.normalized;
            }
            Vector3 predictedVelocity = playerRb.linearVelocity +(Dir2 *  speed);
            calculatedVelocity =  predictedVelocity.magnitude;
            if (grounded &&(predictedVelocity.magnitude<maxVelocity||predictedVelocity.magnitude<playerRb.linearVelocity.magnitude))
            {
                playerRb.AddForce(Dir2 *  speed, ForceMode.VelocityChange);
            }else if (predictedVelocity.magnitude < maxVelocity ||
                      predictedVelocity.magnitude < playerRb.linearVelocity.magnitude)
            {
                playerRb.AddForce(Dir2 *  flySpeed, ForceMode.VelocityChange);
            }

 
    }
    void jump()
    {
        if (grounded)
        {
            playerRb.AddForce(0,jumpForce,0, ForceMode.Impulse);
        }
    }

    void UseWearpon()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            wearpon.GetComponent<wearponDataScript>().Use(ref wearponPivot);
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            wearpon.GetComponent<wearponDataScript>().Ability(ref wearponPivot);
        }
    }
}
