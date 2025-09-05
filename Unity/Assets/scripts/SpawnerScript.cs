using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    GameObject oobPositive, oobNegative;
    public GameObject[] enemy, mine, rockCommon, rockRare;
    public GameObject firefly;

    void Awake()
    {
        oobPositive = GameObject.FindGameObjectWithTag("oobpositive");
        oobNegative = GameObject.FindGameObjectWithTag("oobnegative");
    }

    Vector2 randomPos()
    {
        float spawnX=0, spawnY=0;

        int i = Random.Range(1,5);
        
        switch(i)
        {
            case 1:
            {
                spawnX = Random.Range(oobNegative.transform.position.x,oobPositive.transform.position.x);

                spawnY = oobPositive.transform.position.y;

                break;
            }
            case 2:
            {
                spawnX = Random.Range(oobNegative.transform.position.x,oobPositive.transform.position.x);

                spawnY = oobNegative.transform.position.y;

                break;
            }
            case 3:
            {
                spawnX = oobPositive.transform.position.x;

                spawnY = Random.Range(oobNegative.transform.position.y,oobPositive.transform.position.y);

                break;
            }
            case 4:
            {
                spawnX = oobNegative.transform.position.x;

                spawnY = Random.Range(oobNegative.transform.position.y,oobPositive.transform.position.y);

                break;
            }
        }

        return new Vector2(spawnX,spawnY);
    }

    public void beat()
    {
        for(int i=0; i<Random.Range(1,4); i++)
        {
            GameObject myfirefly = Instantiate(firefly,randomPos(),Quaternion.identity);

            myfirefly.transform.parent = transform;
        }

        if(SingletonScript.instance.isPlayerAlive)
        {
            if(Random.Range(1,3)==1)
            {
                GameObject myrock = Instantiate(rockCommon[Random.Range(0,enemy.Length)],randomPos(),Quaternion.Euler(0,0,Random.Range(0,360)));

                myrock.transform.parent = transform;
            }

            if(Random.Range(1,5)==1)
            {
                GameObject myrock2 = Instantiate(rockRare[Random.Range(0,mine.Length)],randomPos(),Quaternion.Euler(0,0,Random.Range(0,360)));

                myrock2.transform.parent = transform;
            }

            if(Random.Range(1,5)==1)
            {
                GameObject myenemy = Instantiate(enemy[Random.Range(0,enemy.Length)],randomPos(),Quaternion.Euler(0,0,Random.Range(0,360)));

                myenemy.transform.parent = transform;
            }

            if(Random.Range(1,5)==1)
            {
                GameObject mymine = Instantiate(mine[Random.Range(0,mine.Length)],randomPos(),Quaternion.Euler(0,0,Random.Range(0,360)));

                mymine.transform.parent = transform;
            }
        }
    }
}
