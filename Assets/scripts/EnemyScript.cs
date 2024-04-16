using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyScript : MonoBehaviour
{
    GameObject player, myhpbar, firepoint, spawner;
    public GameObject popup, hpbar, explosion, impactpurple, impactgreen, semi, laser, aim, shock, aimshock, muzzleflash, particle, gem, pickup;
    public GameObject[] firepoints;
    Rigidbody2D rb;
    float angle, angleOffset=90, healthmax, health, hpBarDistance=-1, dashDmg=25, dashForce=1250, bulletSpeed, distance, range=6;
    bool iframe, canShock, shocking, isShocked, doBeat, cleanUp;
    [HideInInspector]
    public bool isDashing;
    SpriteRenderer[] skins;
    Vector2 defScale;
    string weapon;
    BeatTriggerScript beatTrigger, beatTrigger2;

    AudioManager snd;
    public AudioClip[] snd_hit;
    public AudioClip[] snd_pew1,snd_pew2,snd_pew3,snd_pew4,snd_pew5,snd_pew6,snd_pew7,snd_pew8,snd_pew9,snd_pew10;
    int pewnum;
    public AudioClip[] snd_dash;
    public AudioClip[] snd_dashhit;
    int pewlasernum;
    public AudioClip[] snd_laser1,snd_laser2,snd_laser3,snd_laser4,snd_laser5;
    public AudioClip[] snd_aim;
    public AudioClip[] snd_shock;
    public AudioClip[] snd_shockhit;

    void Awake()
    {
        player = FindChildWithTag(SingletonScript.instance.player,"playertarget");
        rb = GetComponent<Rigidbody2D>();
        skins = GetComponentsInChildren<SpriteRenderer>();
        beatTrigger = GameObject.FindGameObjectWithTag("beattrigger").GetComponent<BeatTriggerScript>();
        beatTrigger2 = GameObject.FindGameObjectWithTag("beattrigger2").GetComponent<BeatTriggerScript>();
        spawner = GameObject.FindGameObjectWithTag("spawner");
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();
        
        healthmax = Random.Range(75f,125f);
        health = healthmax;

        defScale = transform.localScale;
        
        if(Random.Range(1,3)==1)
        {
            if(Random.Range(1,4)==1)
            {
                weapon = "laser";
                range = 7;
            }
            else if(Random.Range(1,3)==1)
            {
                weapon = "shock";
                range = 5;
            }
            else 
            {
                weapon = "shotgun";
            }
        }
        else
        {
            weapon = "semi";
        }

        pewnum = Random.Range(1,11);
        pewlasernum = Random.Range(1,6);
    }

    GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;
    
        foreach(Transform transform in parent.transform)
        {
            if(transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }
 
        return child;
    }   

    void Start()
    {
        for (int i=0; i<firepoints.Length; i++)
        {
            if(firepoints[i].activeInHierarchy)
            {
                firepoint = firepoints[i];
            }
        }
    }

    void Update()
    {
        if(beatTrigger.beating && !doBeat)
        {
            doBeat=true;

            beat();
        }
        else if(!beatTrigger.beating && doBeat)
        {
            doBeat=false;
        }

        // if(canShock && weapon=="shock")
        // {
        //     if(beatTrigger2.sustaining)
        //     {
        //         shootShock();
        //     }
        //     else
        //     {
        //         offShock();
        //     }
        // }
        
        if(!SingletonScript.instance.isPlayerAlive && !cleanUp)
        {
            cleanUp = true;

            StartCoroutine(cleanUpEnum());
        }
    }

    void LateUpdate()
    {
        if(myhpbar!=null)
        {
            myhpbar.transform.position = new Vector2(transform.position.x, transform.position.y+hpBarDistance);
        }

        if(player!=null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
        }

        if(GetComponent<DestroyOutOfCameraScript>().killed==true)
        {
            Destroy(myhpbar);
        }
    }

    public void hit(float dmg, float kbForce)
    {
        if(!iframe)
        {
            StartCoroutine(iframeCool());
            StartCoroutine(hurtColor());
            StartCoroutine(hurtAnim());

            health -= dmg;

            knockback(kbForce+Random.Range(-100f,100f));

            GameObject mypopup = Instantiate(popup, transform.position, Quaternion.identity);

            mypopup.GetComponent<TextPopupScript>().textPopup(Mathf.Round(dmg));

            if(myhpbar==null)
            {
                myhpbar = Instantiate(hpbar, transform.position, Quaternion.identity);
            }

            myhpbar.GetComponent<HPBarScript>().barColor = new Color(119,0,255);

            myhpbar.GetComponent<HPBarScript>().pivot.transform.LeanScaleX(1*(health/healthmax),.2f).setEaseInOutSine();

            if(health <= 0)
            {
                die();
            }
        }
    }

    public void die()
    {
        Destroy(myhpbar);

        Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f))));

        if(SingletonScript.instance.isPlayerAlive)
        {
            for (int i = 0; i < Random.Range(3,6); i++)
            {
                GameObject mygem = Instantiate(gem, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f))));

                mygem.GetComponent<Rigidbody2D>().AddForce(mygem.transform.up*Random.Range(250f,750f));

                mygem.transform.parent = spawner.transform;
            }

            GameObject mypickup = Instantiate(pickup,transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f))));

            mypickup.GetComponent<Rigidbody2D>().AddForce(mypickup.transform.up*Random.Range(250f,750f));

            mypickup.transform.parent = spawner.transform;

            if(weapon=="laser" || weapon=="shock" || weapon=="shotgun")
            {
                mypickup.GetComponent<PickupScript>().chooseType(weapon);
            }
            else
            {
                if(Random.Range(1,11)==1)
                {
                    mypickup.GetComponent<PickupScript>().chooseType("health");
                }
            }
        }

        Destroy(gameObject);
    }

    IEnumerator iframeCool()
    {
        iframe = true;
        yield return new WaitForSeconds(.1f);
        iframe = false;
    }

    IEnumerator hurtColor()
    {
        foreach(SpriteRenderer sr in skins)
        {
            sr.color = Color.red;
        }

        yield return new WaitForSeconds(.1f);

        foreach(SpriteRenderer sr in skins)
        {
            sr.color = Color.white;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(isDashing && (other.gameObject.layer==3 || other.gameObject.layer==11))
        {
            if(other.gameObject.layer==3 && !other.gameObject.GetComponent<PlayerScript>().isDashing)
            {   
                other.gameObject.GetComponent<PlayerScript>().hit(dashDmg+Random.Range(-5f,5f),1000);
            }
                
            Vector2 midpoint = new Vector2((transform.position.x+other.transform.position.x)/2, (transform.position.y+other.transform.position.y)/2);

            Instantiate(impactpurple, midpoint, Quaternion.Euler(new Vector3(0,0,Random.Range(1,360f))));

            rb.velocity = new Vector2(0,0);

            rb.AddForce(-transform.up*dashForce/4);

            snd.playSFX(snd_dashhit,transform,1);

            isDashing = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="playershock")
        {
            isShocked=true;

            StartCoroutine(gettingShocked());
        }
        else if(other.tag=="playerlaser")
        {
            hit(100+Random.Range(-10f,10f),2000);

            Instantiate(impactgreen, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(1,360f))));

            snd.playSFX(snd_hit,transform,1);
        }
        else if(other.tag=="explosion")
        {
            StartCoroutine(delayedHit(100+Random.Range(-10f,10f),2000));

            snd.playSFX(snd_hit,transform,1);
        }
        else if(other.tag=="minewave")
        {
            hit(50+Random.Range(-10f,10f),2000);

            snd.playSFX(snd_hit,transform,1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag=="playershock")
        {
            isShocked=false;
        }
    }

    IEnumerator gettingShocked()
    {
        while(isShocked)
        {
            hit(Random.Range(10f,15f),250);

            Instantiate(impactgreen, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(1,360f))));

            snd.playSFX(snd_shockhit,transform,1);

            yield return new WaitForSeconds(.1f);
        }
    }

    public void AddForceAtAngle(float angle, float force)
    {
        Quaternion rotation = Quaternion.Euler(0,0,angle);

        Vector3 direction = rotation*Vector3.up;

        rb.velocity = new Vector2(0,0);

        rb.AddForce(direction*force);
    }

    IEnumerator delayedHit(float _dmg, float _kbforce)
    {
        yield return new WaitForSeconds(.1f);

        hit(_dmg,_kbforce);
    }

    IEnumerator hurtAnim()
    {
        float squashvalue = .4f;

        transform.LeanScale(new Vector2(defScale.x-squashvalue,defScale.y+squashvalue),.05f).setEaseInSine();

        yield return new WaitForSeconds(.05f);

        transform.LeanScale(new Vector2(defScale.x+squashvalue,defScale.y-squashvalue),.1f).setEaseOutSine();

        yield return new WaitForSeconds(.1f);

        transform.LeanScale(defScale,.15f).setEaseInOutSine();
    }

    bool beatIt=true, busy=false;

    public void beat()
    {
        if(!busy && player!=null)
        {
            beatIt = !beatIt;
            
            if(distance<=range)
            {
                if(beatIt)
                {   
                    if(weapon=="semi")
                    {
                        StartCoroutine(shootSemi());
                        
                        switch(pewnum)
                        {
                            case 1: snd.playSFX(snd_pew1,transform,1); break;
                            case 2: snd.playSFX(snd_pew2,transform,1); break;
                            case 3: snd.playSFX(snd_pew3,transform,1); break;
                            case 4: snd.playSFX(snd_pew4,transform,1); break;
                            case 5: snd.playSFX(snd_pew5,transform,1); break;
                            case 6: snd.playSFX(snd_pew6,transform,1); break;
                            case 7: snd.playSFX(snd_pew7,transform,1); break;
                            case 8: snd.playSFX(snd_pew8,transform,1); break;
                            case 9: snd.playSFX(snd_pew9,transform,1); break;
                            case 10: snd.playSFX(snd_pew10,transform,1); break;
                        }
                    }
                    else if(weapon=="shotgun")
                    {
                        StartCoroutine(shootShotgun());

                        switch(pewnum)
                        {
                            case 1: snd.playSFX(snd_pew1,transform,1); break;
                            case 2: snd.playSFX(snd_pew2,transform,1); break;
                            case 3: snd.playSFX(snd_pew3,transform,1); break;
                            case 4: snd.playSFX(snd_pew4,transform,1); break;
                            case 5: snd.playSFX(snd_pew5,transform,1); break;
                            case 6: snd.playSFX(snd_pew6,transform,1); break;
                            case 7: snd.playSFX(snd_pew7,transform,1); break;
                            case 8: snd.playSFX(snd_pew8,transform,1); break;
                            case 9: snd.playSFX(snd_pew9,transform,1); break;
                            case 10: snd.playSFX(snd_pew10,transform,1); break;
                        }
                    }
                    else if(weapon=="laser")
                    {
                        StartCoroutine(shootLaser());
                    }
                    else if(weapon=="shock")
                    {
                        StartCoroutine(warnShock());
                    }
                }
            }
            else
            {
                StartCoroutine(dash());
            }
        }
    }

    IEnumerator dash()
    {
        if(!isDashing)
        {
            float time=Random.Range(.1f,.2f);

            aimRandom(time);

            yield return new WaitForSeconds(time);

            rb.velocity = Vector2.zero;

            rb.AddForce(transform.up*(dashForce+Random.Range(-250f,250f)));

            StartCoroutine(dashAnim());

            StartCoroutine(emitParticles());

            snd.playSFX(snd_dash,transform,1);
        }
    }

    IEnumerator dashAnim()
    {
        isDashing = true;

        float squashvalue = .4f;

        transform.LeanScale(new Vector2(defScale.x+squashvalue,defScale.y-squashvalue),.05f).setEaseInSine();

        yield return new WaitForSeconds(.05f);

        transform.LeanScale(new Vector2(defScale.x-squashvalue,defScale.y+squashvalue),.1f).setEaseOutSine();

        yield return new WaitForSeconds(.1f);

        transform.LeanScale(defScale,.15f).setEaseInOutSine();

        isDashing = false;
    }

    void knockback(float force)
    {
        rb.velocity = new Vector2(0,0);

        rb.AddForce(-transform.up*(force));
    }

    IEnumerator emitParticles()
    {
        while(isDashing)
        {
            GameObject myparticles = Instantiate(particle, transform.position, Quaternion.identity);

            myparticles.transform.parent = spawner.transform;

            yield return new WaitForSeconds(.01f);
        }
    }

    void aimRandom(float time)
    {
        float angleRandom = Random.Range(-90f,90f);

        if(player!=null)
        {
            angle = Mathf.Atan2(transform.position.y-player.transform.position.y, transform.position.x-player.transform.position.x) * Mathf.Rad2Deg + angleOffset + angleRandom;
        }

        transform.LeanRotate(new Vector3(0,0,angle),time).setEaseInOutSine();
    }

    void aimPlayer(float time)
    {
        if(player!=null)
        {
            angle = Mathf.Atan2(transform.position.y-player.transform.position.y, transform.position.x-player.transform.position.x) * Mathf.Rad2Deg + angleOffset;
        }

        transform.LeanRotate(new Vector3(0,0,angle),time).setEaseInOutSine();
    }

    IEnumerator shootSemi()
    {
        float time=Random.Range(.1f,.2f);

        aimPlayer(time);

        yield return new WaitForSeconds(time);

        bulletSpeed = Random.Range(250f,500f);

        GameObject mysemi = Instantiate(semi,firepoint.transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

        mysemi.GetComponent<EnemyBulletScript>().AddForceAtAngle(angle, bulletSpeed);

        mysemi.transform.parent = spawner.transform;

        knockback(500);

        StartCoroutine(shootAnim());
    }

    IEnumerator shootShotgun()
    {   
        float time=Random.Range(.1f,.2f);

        aimPlayer(time);

        yield return new WaitForSeconds(time);

        bulletSpeed = Random.Range(250f,500f);

        int shotgunPellets = 3;

        for(int i=0; i<shotgunPellets; i++)
        {
            float inaccuracy = Random.Range((shotgunPellets-1)*-12,(shotgunPellets-1)*12);
            
            GameObject mysemi = Instantiate(semi,firepoint.transform.position,Quaternion.Euler(new Vector3(0,0,angle+inaccuracy)));

            mysemi.GetComponent<EnemyBulletScript>().AddForceAtAngle(angle+inaccuracy, bulletSpeed);

            mysemi.transform.parent = spawner.transform;
        }

        knockback(750);

        StartCoroutine(shootAnim());
    }

    IEnumerator shootLaser()
    {   
        busy=true;

        float time=Random.Range(.1f,.2f);

        aimPlayer(time);

        yield return new WaitForSeconds(time);

        GameObject myaim = Instantiate(aim,transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

        myaim.transform.parent = transform;

        snd.playSFX(snd_aim,transform,1);

        yield return new WaitForSeconds(.75f);

        GameObject mylaser = Instantiate(laser,transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

        mylaser.transform.parent = transform;

        knockback(1000);
    
        StartCoroutine(shootAnim());

        switch(pewlasernum)
        {
            case 1: snd.playSFX(snd_laser1,transform,1); break;
            case 2: snd.playSFX(snd_laser2,transform,1); break;
            case 3: snd.playSFX(snd_laser3,transform,1); break;
            case 4: snd.playSFX(snd_laser4,transform,1); break;
            case 5: snd.playSFX(snd_laser5,transform,1); break;
        }

        yield return new WaitForSeconds(.3f);

        busy=false;
    }

    GameObject myshock;
    bool aimShock;

    IEnumerator warnShock()
    {
        if(!shocking)
        {
            busy=true;

            float time=Random.Range(.1f,.2f);

            aimPlayer(time);

            yield return new WaitForSeconds(time);

            GameObject myaim = Instantiate(aimshock,transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

            myaim.transform.parent = transform;

            aimShock=true;

            StartCoroutine(aimingShock());

            snd.playSFX(snd_aim,transform,1);

            yield return new WaitForSeconds(.75f);

            //canShock = true;
            shootShock();

            yield return new WaitForSeconds(Random.Range(1f,2f));

            //canShock = false;
            offShock();
            
            aimShock=false;

            busy = false;
        }
    }

    IEnumerator aimingShock()
    {
        while(aimShock)
        {
            float time=Random.Range(.2f,.4f);

            aimPlayer(time);
            
            yield return new WaitForSeconds(time);
        }
    }

    void shootShock()
    {
        if(weapon=="shock" && !shocking)
        {
            shocking=true;

            StartCoroutine(shootingShock());

            StartCoroutine(shockSound());

            myshock = Instantiate(shock,transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

            myshock.transform.parent = transform;
        }
    }

    IEnumerator shootingShock()
    {
        while(shocking)
        {
            yield return new WaitForSeconds(.1f);

            flashMuzzle();
        }
    }

    IEnumerator shockSound()
    {
        while(shocking)
        {
            yield return new WaitForSeconds(.25f);

            snd.playSFX(snd_shock,transform,1);
        }
    }

    void offShock()
    {
        if(shocking)
        {
            shocking=false;

            myshock.GetComponent<EnemyShockScript>().offShock();
        }
    }

    IEnumerator shootAnim()
    {
        flashMuzzle();

        float squashvalue = .4f;

        transform.LeanScale(new Vector2(defScale.x+squashvalue,defScale.y-squashvalue),.1f).setEaseInSine();

        yield return new WaitForSeconds(.1f);

        transform.LeanScale(defScale,.2f).setEaseInOutSine();
    }

    void flashMuzzle()
    {
        GameObject mymuzzle = Instantiate(muzzleflash,firepoint.transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

        mymuzzle.transform.parent = transform;
    }

    IEnumerator cleanUpEnum()
    {
        yield return new WaitForSeconds(Random.Range(.5f,2f));

        die();
    }
}
