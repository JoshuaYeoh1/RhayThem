using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerScript : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject semi, shock, laser, firepoint, muzzleflash, impactgreen, impactpurple, popup, explosion, particle, shield;
    GameObject spawner;
    Vector2 mousePos, defaultScale;
    Camera cam;
    WiggleScript cameraShake;
    float angle, angleOffset=90, healthmax=1000, dashForce=3000, dashDmg=25, bulletSpeed=700;
    public float health, ammo;
    public string weapon = "semi";
    int shotgunPellets=4;
    bool shocking, iframe, isShocked;
    [HideInInspector]
    public bool isDashing;
    public SpriteRenderer sr;

    AudioManager snd;
    public AudioClip[] snd_pew1,snd_pew2,snd_pew3,snd_pew4,snd_pew5,snd_pew6,snd_pew7,snd_pew8,snd_pew9,snd_pew10;
    int pewnum;
    public AudioClip[] snd_dash;
    public AudioClip[] snd_dashhit;
    public AudioClip[] snd_loot;
    public AudioClip[] snd_lootlife;
    public AudioClip[] snd_lootweapon;
    public AudioClip[] snd_lootgem;
    int pewlasernum;
    public AudioClip[] snd_laser1,snd_laser2,snd_laser3,snd_laser4,snd_laser5;
    public AudioClip[] snd_die;
    public AudioClip[] snd_hit;
    public AudioClip[] snd_shieldon;
    public AudioClip[] snd_shieldoff;
    public AudioClip[] snd_shock;
    public AudioClip[] snd_shockhit;
    public AudioClip[] snd_lose;
    public AudioClip[] snd_subwoofer;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cameraShake = GameObject.FindGameObjectWithTag("camshaker").GetComponent<WiggleScript>();
        spawner = GameObject.FindGameObjectWithTag("spawner");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();

        defaultScale = transform.localScale;

        health = healthmax;

        StartCoroutine(hpRegen());
        
        SingletonScript.instance.lastPlayerPos = Vector2.zero;

        pewnum = Random.Range(1,11);
    }

    public void spawn()
    {
        StartCoroutine(spawnenum());
    }

    IEnumerator spawnenum()
    {
        iframe = true;

        SingletonScript.instance.isPlayerAlive = true;

        GameObject myshield = Instantiate(shield, transform.position, Quaternion.identity);

        myshield.transform.parent = transform;

        transform.LeanScale(Vector2.zero,0);

        myshield.transform.LeanScale(Vector2.zero,0);

        transform.LeanScale(defaultScale,.5f).setEaseInOutSine();

        myshield.transform.LeanScale(Vector2.one,.75f).setEaseInOutSine();

        snd.playSFX(snd_shieldon,transform,1);

        yield return new WaitForSeconds(5);

        iframe = false;

        myshield.GetComponentInChildren<Animator>().SetTrigger("off");

        snd.playSFX(snd_shieldoff,transform,1);
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        angle = Mathf.Atan2(transform.position.y-mousePos.y, transform.position.x-mousePos.x) * Mathf.Rad2Deg + angleOffset;

        transform.rotation = Quaternion.Euler(0,0,angle);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(isDashing && (other.gameObject.layer==10 || other.gameObject.layer==11))
        {
            if(other.gameObject.layer==10)
            {   
                other.gameObject.GetComponent<EnemyScript>().hit(dashDmg+Random.Range(-5f,5f),1000);
            }
                
            Vector2 midpoint = new Vector2((transform.position.x+other.transform.position.x)/2, (transform.position.y+other.transform.position.y)/2);

            Instantiate(impactgreen, midpoint, Quaternion.Euler(new Vector3(0,0,Random.Range(1,360f))));

            rb.velocity = new Vector2(0,0);

            rb.AddForce(-transform.up*dashForce/4);

            snd.playSFX(snd_dashhit,transform,1);

            isDashing = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="enemyshock")
        {
            isShocked=true;

            StartCoroutine(gettingShocked());
        }
        
        if(other.tag=="enemylaser")
        {
            hit(100+Random.Range(-10f,10f),2000);

            Instantiate(impactpurple, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(1,360f))));
        }
        
        if(other.tag=="explosion")
        {
            hit(100+Random.Range(-10f,10f),2000);
        }
        
        if(other.tag=="minewave")
        {
            hit(50+Random.Range(-10f,10f),2000);
        }

        if(other.tag=="gem")
        {
            int score = Random.Range(1,6);

            SingletonScript.instance.gems += score;

            GameObject mypopup = Instantiate(popup, transform.position, Quaternion.identity);

            //mypopup.transform.localScale = new Vector2(mypopup.transform.localScale.x*.75f,mypopup.transform.localScale.y*.75f);

            mypopup.GetComponent<TextPopupScript>().TextpopupTMP.text = Mathf.Round(score).ToString();

            mypopup.GetComponent<TextPopupScript>().TextpopupTMP.color = new Color(1,.5f,0);

            mypopup.transform.parent = spawner.transform;

            snd.playSFX(snd_lootgem,transform,1);

            StartCoroutine(orangeColor());

            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag=="enemyshock")
        {
            isShocked=false;

            StopCoroutine(gettingShocked());
        }
    }

    IEnumerator gettingShocked()
    {
        while(isShocked)
        {
            hit(Random.Range(10f,15f),250);

            Instantiate(impactpurple, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(1,360f))));

            snd.playSFX(snd_shockhit,transform,1);

            yield return new WaitForSeconds(.1f);
        }
    }

    public void dash()
    {
        if(!isDashing)
        {
            StartCoroutine(dashAnim());

            StartCoroutine(emitParticles());

            rb.velocity = new Vector2(0,0);

            rb.AddForce(transform.up*dashForce);

            snd.playSFX(snd_dash,transform,1);
        }
    }

    IEnumerator dashAnim()
    {
        isDashing = true;

        float squashvalue = .4f;

        transform.LeanScale(new Vector2(defaultScale.x+squashvalue,defaultScale.y-squashvalue),.05f).setEaseInSine();

        yield return new WaitForSeconds(.05f);

        transform.LeanScale(new Vector2(defaultScale.x-squashvalue,defaultScale.y+squashvalue),.1f).setEaseOutSine();

        yield return new WaitForSeconds(.1f);

        transform.LeanScale(defaultScale,.15f).setEaseInOutSine();

        isDashing = false;
    }

    IEnumerator emitParticles()
    {
        while(isDashing)
        {
            GameObject myparticle = Instantiate(particle, transform.position, Quaternion.identity);

            myparticle.transform.parent = spawner.transform;

            yield return new WaitForSeconds(.01f);
        }
    }

    public void shoot()
    {   
        if(weapon!="shock")
        {
            if(weapon=="laser")
            {
                if(ammo>0)
                {
                    ammo--;

                    GameObject mylaser = Instantiate(laser,transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

                    mylaser.transform.parent = transform;
                    
                    knockback(1000);

                    switch(pewlasernum)
                    {
                        case 1: snd.playSFX(snd_laser1,transform,1); break;
                        case 2: snd.playSFX(snd_laser2,transform,1); break;
                        case 3: snd.playSFX(snd_laser3,transform,1); break;
                        case 4: snd.playSFX(snd_laser4,transform,1); break;
                        case 5: snd.playSFX(snd_laser5,transform,1); break;
                    }
                }
                else
                {
                    weapon="semi";

                    pewnum = Random.Range(1,11);

                    snd.playSFX(snd_lootweapon,transform,1);
                }   
            }
            
            if(weapon=="semi")
            {   
                if(ammo>0)
                {   
                    ammo--;

                    for(int i=0; i<shotgunPellets; i++)
                    {
                        float inaccuracy = Random.Range((shotgunPellets-1)*-12,(shotgunPellets-1)*12);
                        
                        GameObject mysemi = Instantiate(semi,firepoint.transform.position,Quaternion.Euler(new Vector3(0,0,angle+inaccuracy)));

                        mysemi.GetComponent<PlayerBulletScript>().AddForceAtAngle(angle+inaccuracy, bulletSpeed);

                        mysemi.transform.parent = spawner.transform;
                    }

                    knockback(750);

                    if(ammo==1)
                    {
                        snd.playSFX(snd_lootweapon,transform,1);
                    }
                }
                else
                {
                    GameObject mysemi = Instantiate(semi,firepoint.transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

                    mysemi.GetComponent<PlayerBulletScript>().AddForceAtAngle(angle, bulletSpeed);

                    mysemi.transform.parent = spawner.transform;

                    knockback(500);
                }

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

            StartCoroutine(shootAnim());            
        }
    }

    void knockback(float force)
    {
        rb.velocity = new Vector2(0,0);

        rb.AddForce(-transform.up*(force));
    }

    IEnumerator shootAnim()
    {
        flashMuzzle();

        float squashvalue = .4f;

        transform.LeanScale(new Vector2(defaultScale.x+squashvalue,defaultScale.y-squashvalue),.1f).setEaseInSine();

        yield return new WaitForSeconds(.1f);

        transform.LeanScale(defaultScale,.2f).setEaseInOutSine();

        yield return new WaitForSeconds(.2f);
    }

    void flashMuzzle()
    {
        GameObject mymuzzle = Instantiate(muzzleflash,transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

        mymuzzle.transform.parent = transform;
    }

    GameObject myshock;

    public void shootShock()
    {
        if(weapon=="shock" && ammo>0 && !shocking)
        {
            shocking=true;

            StartCoroutine(shockAmmo());

            StartCoroutine(shockSound());

            myshock = Instantiate(shock,transform.position,Quaternion.Euler(new Vector3(0,0,angle)));

            myshock.transform.parent = transform;
        }
    }

    public void offShock()
    {
        if(myshock!=null && shocking)
        {
            shocking=false;

            myshock.GetComponent<PlayerShockScript>().offShock();
        }
    }

    IEnumerator shockAmmo()
    {
        while(shocking)
        {
            if(ammo>0)
            {
                yield return new WaitForSeconds(.1f);

                flashMuzzle();

                ammo-=.1f;

                if(ammo<=0)
                {
                    weapon="semi";

                    pewnum = Random.Range(1,11);

                    snd.playSFX(snd_lootweapon,transform,1);

                    offShock();
                }
            }
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

    public void hit(float dmg, float kbForce)
    {
        if(!iframe && !isDashing)
        {
            StartCoroutine(iframeCool());
            StartCoroutine(hurtColor());
            StartCoroutine(hurtAnim());

            health -= dmg;

            AddForceAtAngle(Random.Range(0f,360f), kbForce+Random.Range(-100f,100f));

            GameObject mypopup = Instantiate(popup, transform.position, Quaternion.identity);

            mypopup.GetComponent<TextPopupScript>().textPopup(Mathf.Round(dmg/10+1));

            mypopup.transform.parent = spawner.transform;

            cameraShake.shake();

            SingletonScript.instance.dmgvigshow();

            snd.playSFX(snd_hit,transform,1);
            snd.playSFX(snd_subwoofer,transform,1);

            if(health <= 0)
            {
                die();
            }

            // if(myhpbar==null)
            // {
            //     myhpbar = Instantiate(hpbar, transform.position, Quaternion.identity);
            // }
            //myhpbar.GetComponent<HPBarScript>().barColor = new Color(119,0,255);
            //myhpbar.GetComponent<HPBarScript>().pivot.transform.LeanScaleX(1*(health/healthmax),.2f).setEaseInOutSine();
        }
    }

    void die()
    {
        if(myshock!=null)
        {
            Destroy(myshock);
        }

        StopAllCoroutines();

        Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(0f,360f))));

        SingletonScript.instance.isPlayerAlive = false;

        SingletonScript.instance.lastPlayerPos = transform.position;

        snd.playSFX(snd_die,transform,1);
        snd.playSFX(snd_lose,transform,1);

        GameObject mypopup = Instantiate(popup, transform.position, Quaternion.identity);

        mypopup.GetComponent<TextPopupScript>().textPopup("You Died!");

        mypopup.transform.parent = spawner.transform;

        SingletonScript.instance.gems=0;

        SingletonScript.instance._mainMenu();

        Destroy(gameObject);
    }

    IEnumerator iframeCool()
    {
        iframe = true;
        yield return new WaitForSeconds(.1f);
        iframe = false;
    }

    IEnumerator hurtAnim()
    {
        float squashvalue = .1f;

        transform.LeanScale(new Vector2(defaultScale.x-squashvalue,defaultScale.y+squashvalue),.05f).setEaseInSine();

        yield return new WaitForSeconds(.05f);

        transform.LeanScale(new Vector2(defaultScale.x+squashvalue,defaultScale.y-squashvalue),.1f).setEaseOutSine();

        yield return new WaitForSeconds(.1f);

        transform.LeanScale(defaultScale,.15f).setEaseInOutSine();
    }

    IEnumerator hurtColor()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(.1f);

        sr.color = Color.white;
    }
    
    IEnumerator greenColor()
    {
        sr.color = new Color(.5f,1,0);

        yield return new WaitForSeconds(.1f);

        sr.color = Color.white;
    }
    
    IEnumerator orangeColor()
    {
        sr.color = new Color(1,.5f,0);

        yield return new WaitForSeconds(.1f);

        sr.color = Color.white;
    }

    IEnumerator hpRegen()
    {
        while(health<healthmax && SingletonScript.instance.isPlayerAlive)
        {
            health++;

            yield return new WaitForSeconds(1);
        }
    }

    public void AddForceAtAngle(float angle, float force)
    {
        Quaternion rotation = Quaternion.Euler(0,0,angle);

        Vector3 direction = rotation*Vector3.up;

        rb.velocity = new Vector2(0,0);

        rb.AddForce(direction*force);
    }

    public void pickup(string type)
    {
        GameObject mypopup = Instantiate(popup, transform.position, Quaternion.identity);

        mypopup.GetComponent<TextPopupScript>().TextpopupTMP.color = new Color(.5f,1,0);

        mypopup.transform.parent = spawner.transform;

        StartCoroutine(greenColor());

        snd.playSFX(snd_loot,transform,1);

        if(type=="health")
        {
            float heal = (healthmax-health)*.8f;

            mypopup.GetComponent<TextPopupScript>().TextpopupTMP.text = Mathf.Round(heal/10).ToString()+" HP";

            health+=heal;

            snd.playSFX(snd_lootlife,transform,1);
        }
        else if(type=="laser")
        {
            if(weapon!="laser")
            {
                weapon="laser";
                ammo=20;
                mypopup.GetComponent<TextPopupScript>().TextpopupTMP.text = "Railgun";
                pewlasernum = Random.Range(1,6);
            }
            else
            {
                ammo+=20;
                mypopup.GetComponent<TextPopupScript>().TextpopupTMP.text = "Ammo";
            }

            snd.playSFX(snd_lootweapon,transform,1);
        }
        else if(type=="shock")
        {
            if(weapon!="shock")
            {
                weapon="shock";
                ammo=10;
                mypopup.GetComponent<TextPopupScript>().TextpopupTMP.text = "Zapper";
            }
            else
            {
                ammo+=10;
                mypopup.GetComponent<TextPopupScript>().TextpopupTMP.text = "Ammo";
            }

            snd.playSFX(snd_lootweapon,transform,1);
        }
        else if(type=="shotgun")
        {
            if(weapon!="semi")
            {
                weapon="semi";
                ammo=20;
                mypopup.GetComponent<TextPopupScript>().TextpopupTMP.text = "Shotgun";
                pewnum = Random.Range(1,11);
            }
            else
            {
                ammo+=20;
                mypopup.GetComponent<TextPopupScript>().TextpopupTMP.text = "Ammo";
            }

            snd.playSFX(snd_lootweapon,transform,1);
        }
    }
}   
