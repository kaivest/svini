using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class fistScript : wearponDataScript
{
    private Coroutine attackCoroutine = null;
    private Coroutine windowCoroutine = null;
    private Transform wearponPivoLocal;
    [SerializeField] private bool AttackCoroutineWorking = false;
    public float currentDamageMultiplier = 1;
    public float baseDamageMultiplier = 1.2f;
    public float crossDistance= 10;
    private bool comboWindowYielding = false;
    private bool combo1Pressed;
    private bool combo2Pressed;
    private bool combo3Pressed;
    private bool combo4Pressed;
    private bool combo5Pressed;
    private bool comboKeyPressed;
    [SerializeField] private bool comboWindowOpen;
    void Start()
    {
        comboCount = 0;
        
    }
    void Update()
    {
        if (comboWindowOpen)
        {
            if(Input.GetKeyDown(combo1)){combo1Pressed = true; comboWindowOpen = false;Debug.Log("combo key pressed: "+combo1);}
            if(Input.GetKeyDown(combo2)){combo2Pressed = true; comboWindowOpen = false;Debug.Log("combo key pressed: "+combo2);}
            if(Input.GetKeyDown(combo3)){combo3Pressed = true; comboWindowOpen = false;Debug.Log("combo key pressed: "+combo3);}
            if(Input.GetKeyDown(combo4)){combo4Pressed = true; comboWindowOpen = false;Debug.Log("combo key pressed: "+combo4);}
            if(Input.GetKeyDown(combo5)){combo5Pressed = true; comboWindowOpen = false;Debug.Log("combo key pressed: "+combo5);}
        }

        if (attackCoroutine != null) AttackCoroutineWorking = true;
        if (attackCoroutine == null) AttackCoroutineWorking = false;
    }
    private IEnumerator Combo3Coroutine()
    {
        Debug.Log("fist combo 3");
        CloseCombo();
        Collider collid = CreateCollider(wearponPivoLocal);
        PrepareCollider(ref collid,wearponPivoLocal,wearponPivoLocal.position, new Vector3(1,2,1),new Vector3(0,2,0));
        /*col.transform.position = wearponPivoLocal.transform.position;
        GameObject localPivo = new GameObject();
        localPivo.transform.position = wearponPivoLocal.transform.position;
        localPivo.transform.rotation = wearponPivoLocal.transform.rotation;
        localPivo.transform.SetParent(wearponPivoLocal.transform);
        col.transform.SetParent(localPivo.transform);
        float i = 0;
        float distance1 = 5.5f;
        float deltaAngle = speed * 0.01f*0.75f;
        float currentAngle = 0;
        localPivo.transform.localRotation *= Quaternion.Euler(30, 0, 0);
        while (i < distance1)
        {
            float localSpeed = speed;
            FistForward(ref col, ref i, localSpeed/1.7f);
            RotateWearponAxis(localPivo.transform, 100, deltaAngle, ref currentAngle);
            yield return Time.deltaTime;
        }
        Destroy(localPivo);*/
        yield return UpperCut(5.5f, speed * 0.01f * 0.75f, speed, collid);
        EndComboSeries();
        attackCoroutine = null;
        yield break;

    }
    private IEnumerator Combo2Coroutine()
    {
        Debug.Log("fist combo2");
        CloseCombo();
        Collider collid = CreateCollider(wearponPivoLocal);
        PrepareCollider(ref collid,wearponPivoLocal,wearponPivoLocal.position, new Vector3(1,1,3),  new Vector3(-3,2,0));
            yield return Cross(crossDistance, speed, collid);
            while (comboWindowYielding)
            {
                if (combo2Pressed)
                {
                    attackCoroutine = StartCoroutine(Combo3Coroutine());
                    yield break;
                }
                yield return Time.deltaTime;
            }
            if(!combo2Pressed) EndComboSeries();
        
    }
    private IEnumerator Combo1Coroutine()
    {
        Debug.Log("fist combo1"); 
        Vector3 pos =  wearponPivoLocal.transform.position;
        pos.y+= 0.7f;
        Collider collid = CreateCollider(wearponPivoLocal);
        PrepareCollider(ref collid,wearponPivoLocal, pos, new Vector3(1,1,3), new Vector3(0,2,0));
        yield return Cross(crossDistance, speed, collid);
        while (comboWindowYielding)
        {
            if (combo1Pressed)
            {
                attackCoroutine = StartCoroutine(Combo2Coroutine());
                yield break;
            }
            yield return Time.deltaTime;
        }
        if(!combo1Pressed) EndComboSeries();
    }
    internal override void  Use(ref Transform wearponPivo)
    {
        if (attackCoroutine == null)
        {
            wearponPivoLocal = wearponPivo;
            Debug.Log("fist use");
            //col.GetComponent<MeshRenderer>().enabled = false;
            attackCoroutine = StartCoroutine(Combo1Coroutine());
        }
        
    }
    internal override void  Ability(ref Transform wearponPivo)
    {
        Debug.Log("fist ability");
    }
    private void FistForward(ref Collider localCol, ref float i, float fistSpeed)
    {
        Vector3 pos = localCol.transform.localPosition;
        pos.z += 0.001f*fistSpeed;
        i += 0.001f * fistSpeed;
        localCol.transform.localPosition = pos;
        localCol.transform.rotation = wearponPivoLocal.rotation;
    }
    
    
    
    
    private IEnumerator RotateWearponAxis(Transform wearponPivo, int angleX,int angleY,int angleZ, float deltaAngle)
    {
        Vector3 delta = new Vector3(angleX, angleY, angleZ);
        delta = delta.normalized;
        delta *= 1*(float)speed/75;
        while (true)
        {
            wearponPivo.rotation *= Quaternion.Euler(delta);
            yield return Time.deltaTime;
        }
        
    }
    
    
    
    
    
    
    
    private IEnumerator OpenCloseComboWindow(float openYield, float closeYield)
    {
        yield return new WaitForSeconds(openYield);
        comboWindowOpen = true;
        Debug.Log("fist open");
        comboWindowYielding = true;
        yield return new WaitForSeconds(closeYield);
        comboWindowOpen = false;
        Debug.Log("fist close");
        comboWindowYielding = false;
    }
    private IEnumerator Cross(float distance, int fistSpeed, Collider col)
    {
        Debug.Log("fist cross");
        windowCoroutine =StartCoroutine(OpenCloseComboWindow(distance/(fistSpeed*0.001f*50)*0.05f, distance/(fistSpeed*0.001f*50)*0.2f));
        float i = 0;
        while (i < distance)
        {
            Debug.Log("fist cross yield");
                FistForward(ref col, ref i, fistSpeed);
            yield return Time.deltaTime;
        }
            Destroy(col.gameObject);
            Debug.Log("fist destroyed");

    }
    private IEnumerator UpperCut(float distance, float deltaAngle, int fistSpeed, Collider col)
    {
        GameObject localPivo = new GameObject();
        localPivo.transform.position = wearponPivoLocal.transform.position;
        localPivo.transform.rotation = wearponPivoLocal.transform.rotation;
        localPivo.transform.SetParent(wearponPivoLocal.transform);
        col.transform.SetParent(localPivo.transform);
        float i = 0;
        deltaAngle = distance/(fistSpeed)*speed*Time.deltaTime*320;
        float currentAngle = 0;
        localPivo.transform.localRotation *= Quaternion.Euler(60, 0, 0);
        windowCoroutine = StartCoroutine( OpenCloseComboWindow(distance/(fistSpeed*0.001f*50)*0.05f, distance/(fistSpeed*0.001f*50)));
        Coroutine rotate = StartCoroutine( RotateWearponAxis(localPivo.transform, -1000,0,0 ,deltaAngle));
        while (i < distance)
        {
            float localSpeed = speed;
            FistForward(ref col, ref i, localSpeed / 1.4f);
            yield return Time.deltaTime;
        }
        StopCoroutine(rotate);
        Destroy(localPivo);
    }
    private IEnumerator Double()
    {
        GameObject localPivoLeft = new GameObject();
        GameObject localPivoRight = new GameObject();
        localPivoLeft.transform.SetParent(wearponPivoLocal.transform);
        localPivoRight.transform.SetParent(wearponPivoLocal.transform);
        localPivoLeft.transform.position = wearponPivoLocal.transform.parent.transform.position;
        localPivoLeft.transform.position += Vector3.up;
        localPivoRight.transform.position =  localPivoLeft.transform.position;
        localPivoRight.transform.rotation *= Quaternion.Euler(0, 60, 0);
        localPivoLeft.transform.rotation *= Quaternion.Euler(0, -60, 0);
        Collider rightCol = CreateCollider(localPivoRight.transform);
        Collider leftCol = CreateCollider(localPivoLeft.transform);
        PrepareCollider(ref rightCol,localPivoRight.transform,localPivoRight.transform.position,new Vector3(1,3,1), new Vector3(0,0,5));
        PrepareCollider(ref leftCol,localPivoLeft.transform,localPivoLeft.transform.position,new Vector3(1,3,1), new Vector3(0,0,5));
        leftCol.transform.SetParent(localPivoLeft.transform);
        rightCol.transform.SetParent(localPivoRight.transform);
        int i = 0;
        while (i < 60)
        {
            
        }
        yield return null;
    }
    private IEnumerator FinalCombo()
    {
        yield break;
    }
    private void CloseCombo()
    {
        combo1Pressed = false;
        combo2Pressed = false;
        combo3Pressed = false;
        combo4Pressed = false;
        combo5Pressed = false;
        comboWindowOpen = false;
        if (windowCoroutine != null)
        {
            StopCoroutine(windowCoroutine);
        }
        else
        {
            Debug.Log("fist window coroutine does not exist");
        }

        windowCoroutine = null;
        Debug.Log("fist combo closed");
    }
    private void EndComboSeries()
    {
        CloseCombo();
        attackCoroutine = null;
        currentDamageMultiplier = 1;
        Debug.Log("attackCoroutine is null");
    }
    void HitSvin(Collider other)
    {
        if (other.CompareTag("svin"))
        {
            currentDamageMultiplier*=baseDamageMultiplier;
            PushSvin(this.GetComponent<Collider>(), other,1);
            this.GetComponent<Collider>().enabled = false;
            other.gameObject.GetComponent<entityData>().healthpoints-= Convert.ToInt16(damage*currentDamageMultiplier);
        }
    }
    private void PushSvin(Collider fist,Collider svinCol, int mode)
    {
        if (mode == 1)
        {
            svinCol.gameObject.GetComponent<Rigidbody>().AddForce(fist.transform.forward.normalized*impulse, ForceMode.Impulse);
        }
    }
    private Collider CreateCollider(Transform wearponPivo)
    {
        Vector3 spawnpos = wearponPivo.transform.position;
        spawnpos += Vector3.up;
        GameObject tempCol = new GameObject();
        tempCol.AddComponent<BoxCollider>();
        tempCol.AddComponent<MeshRenderer>();
        tempCol.AddComponent<MeshFilter>();
        tempCol.GetComponent<MeshFilter>().mesh = GetComponentInParent<Transform>().gameObject.GetComponentInParent<MeshFilter>().mesh;
        tempCol.GetComponent<MeshRenderer>().material =new Material(Shader.Find("Universal Render Pipeline/Lit"));
        tempCol.transform.position = spawnpos;
        tempCol.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        tempCol.transform.SetParent(wearponPivo);
        tempCol.tag = "wearpon";
        tempCol.AddComponent<customTrigger>();
        tempCol.gameObject.GetComponent<customTrigger>().OnEntered += HitSvin;
        tempCol.GetComponent<customTrigger>().Mask = wearponPivo.GetComponentInParent<Rigidbody>().gameObject.GetComponentInParent<playerMovementScript>().iCanDamage;
        tempCol.GetComponent<Collider>().isTrigger = true;
        return tempCol.GetComponent<Collider>();
    }
    void PrepareCollider(ref Collider localCollider, Transform wearponPivo, Vector3 pos, Vector3 scale, Vector3 shift)
    {
        bool svinDamaged = false;
        localCollider.enabled = true;
        localCollider.transform.position = pos;
        Vector3 position = localCollider.transform.localPosition;
        position.x = shift.x;
        position.y = shift.y;
        position.z = shift.z;
        localCollider.transform.localPosition = position;
        localCollider.transform.localScale = scale;
        localCollider.transform.SetParent(wearponPivo);
        Debug.Log("collider prepared: \n" + "name: " + localCollider.name + "\n parent: " +
                  localCollider.transform.parent.name);
        
    }
}
