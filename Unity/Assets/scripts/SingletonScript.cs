using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using Unity.Mathematics;

public class SingletonScript : MonoBehaviour
{
    public static SingletonScript instance;

    public int gems=0, highscore=0;
    public bool isPlayerAlive=false;
    public GameObject teleport, player;
    public PlayerScript playerScript;
    public Vector2 lastPlayerPos;
    SpriteRenderer dmgvig;

    AudioManager snd;
    public AudioClip[] snd_teleport, snd_tween;

    public TextMeshProUGUI tmp_health, tmp_ammo, tmp_score, tmp_highscore;
    public GameObject hudHealth, hudAmmo, iconShotgun, iconShock, iconRailgun, hudScore;
    public GameObject playbtn, title, settingsbtn, exitbtn, tutorial;
    Vector2 playbtnpos, titlepos, settingsbtnpos, exitbtnpos;

    public Animator transition;

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        dmgvig = GameObject.FindGameObjectWithTag("dmgvig").GetComponent<SpriteRenderer>();
        snd = GameObject.FindGameObjectWithTag("audiomanager").GetComponent<AudioManager>();

        tmp_health.text = tmp_ammo.text = tmp_score.text = tmp_highscore.text = "";

        playbtnpos = playbtn.transform.position;
        titlepos = title.transform.position;
        settingsbtnpos = settingsbtn.transform.position;
        exitbtnpos = exitbtn.transform.position;

        transition.SetTrigger("in");

        snd.playSFX(snd_tween,transform,1);

        StartCoroutine(mainMenu());
    }

    void Update()
    {
        if(gems>=highscore)
        {
            highscore=gems;
        }

        if(isPlayerAlive)
        {
            hudHealth.SetActive(true);
            tmp_health.text = Mathf.Round(playerScript.health/10).ToString();

            if(playerScript.ammo>0)
            {
                hudAmmo.SetActive(true);

                tmp_ammo.text = Mathf.Round(playerScript.ammo).ToString();

                if(playerScript.weapon=="semi")
                {
                    iconShotgun.SetActive(true);
                }
                else
                {
                    iconShotgun.SetActive(false);
                }

                if(playerScript.weapon=="shock")
                {
                    iconShock.SetActive(true);
                }
                else
                {
                    iconShock.SetActive(false);
                }

                if(playerScript.weapon=="laser")
                {
                    iconRailgun.SetActive(true);
                }
                else
                {
                    iconRailgun.SetActive(false);
                }
            }
            else
            {
                hudAmmo.SetActive(false);
            }

            hudScore.SetActive(true);

            tmp_score.text = gems.ToString();
            tmp_highscore.text = highscore.ToString();
        }
        else
        {
            hudHealth.SetActive(false);
            hudAmmo.SetActive(false);
            hudScore.SetActive(false);
        }
    }

    public void viewTutorial()
    {
        title.transform.LeanMoveLocalY(titlepos.y+8,Random.Range(.25f,.75f)).setEaseInSine();
        playbtn.transform.LeanMoveLocalY(playbtnpos.y-8,Random.Range(.25f,.75f)).setEaseInSine();

        tutorial.SetActive(true);
        tutorial.LeanScale(Vector3.zero,0);
        tutorial.LeanScale(Vector3.one,2).setEaseOutSine();

        snd.playSFX(snd_tween,transform,1);
    }

    public void spawnPlayer()
    {
        if(!isPlayerAlive)
        {
            StartCoroutine(spawnPlayerenum());
        }
        else
            Debug.Log("Player already spawned!");
    }

    IEnumerator spawnPlayerenum()
    {
        tutorial.LeanScale(Vector3.zero,1).setEaseInSine();

        snd.playSFX(snd_tween,transform,1);

        yield return new WaitForSeconds(1);

        tutorial.SetActive(false);

        Instantiate(teleport, lastPlayerPos, Quaternion.identity);

        snd.playSFX(snd_teleport,transform,1);
    }

    public void _mainMenu()
    {
        StartCoroutine(mainMenu());
    }

    IEnumerator mainMenu()
    {
        if(!isPlayerAlive)
        {
            title.transform.LeanMoveLocalY(titlepos.y+8,0);
            playbtn.transform.LeanMoveLocalY(playbtnpos.y-8,0);
            settingsbtn.transform.LeanMoveLocalY(settingsbtnpos.y+6,0);
            exitbtn.transform.LeanMoveLocalY(exitbtnpos.y+6,0);

            yield return new WaitForSeconds(1);
            
            title.transform.LeanMoveLocalY(titlepos.y,Random.Range(1f,2f)).setEaseOutSine();
            playbtn.transform.LeanMoveLocalY(playbtnpos.y,Random.Range(1,2f)).setEaseOutSine();
            settingsbtn.transform.LeanMoveLocalY(settingsbtnpos.y,Random.Range(1f,2f)).setEaseOutSine();
            exitbtn.transform.LeanMoveLocalY(exitbtnpos.y,Random.Range(1f,2f)).setEaseOutSine();

            snd.playSFX(snd_tween,transform,1);
        }
    }

    public void dmgvigshow()
    {
        StartCoroutine(dmgviganim());
    }

    IEnumerator dmgviganim()
    {
        LeanTween.value(dmgvig.color.a,1f,.1f).setEaseInOutSine().setOnUpdate(dmgvigSetAlpha);

        yield return new WaitForSeconds(.1f);

        LeanTween.value(dmgvig.color.a,0,.5f).setEaseInOutSine().setOnUpdate(dmgvigSetAlpha);
    }
    
    public void dmgvigSetAlpha(float value)
    {
        dmgvig.color = new Color(dmgvig.color.r,dmgvig.color.g,dmgvig.color.b, value);
    }
}
