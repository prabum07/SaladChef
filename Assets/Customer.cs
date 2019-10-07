using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject start;
    public GameObject end;
    public bool isActive;
    public bool isAngry;
    public bool Gotten;
    public List<char> needed = new List<char>();
    public Player Assist;
    

    void Start()
    {
        Precalculate();
    }
    public float temp;
    public int TimeDecreaseRate=1;
    public void Precalculate()
    {
        needed.Clear();
        isAngry = false;
        int rand = Random.Range(1, 4);
        int count = 0;
        while(count!=rand)
        {
            int random2 = Random.Range(0, customerManager.customerManagers.totalIngredients.Count);
            if(!needed.Contains( customerManager.customerManagers.totalIngredients[random2]))
            {
                needed.Add(customerManager.customerManagers.totalIngredients[random2]);
                count++;
            }

        }
        time = needed.Count * 20;
        seventyPercentTime = time * (0.7f);
        StartCoroutine(Move());
        

    }
    public int time;
    public float seventyPercentTime;
    IEnumerator Timer()
    {
        while(time>1)
        {
            yield return new WaitForSeconds(1);
            time-=TimeDecreaseRate;
            if(Gotten)
            {
                if(time>seventyPercentTime)
                {
                    Assist.Score += 10;
                    Assist.canCaughtPowerUp = true;
                    customerManager.customerManagers.RandomSpawnPowerUp();
                    StartCoroutine(powerupTimer());
                }
                else
                {
                    Assist.Score += 5;

                }
                Assist.ChoppedVegetable.Clear();
                Assist.RefreshChoppedBtn();
                break;
            }
        }
        StartCoroutine(MoveBack());

    }

    IEnumerator powerupTimer()
    {
        customerManager.customerManagers.PowerUp.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        customerManager.customerManagers.PowerUp.gameObject.SetActive(false);

    }
    IEnumerator Move()
    {
         temp = 0f;
        while(temp<0.9f )
        {
            transform.position = Vector3.Lerp(start.transform.position, end.transform.position, temp);

            temp+=0.01f;
            yield return null;
        }
        StartCoroutine(Timer());

    }
    IEnumerator MoveBack()
    {
        if(Gotten==false)
        {
            for(int i=0;i<customerManager.customerManagers.Players.Count;i++)
            {
                customerManager.customerManagers.Players[i].Score -= 5;
            }
            for (int i = 0; i < customerManager.customerManagers.Players.Count; i++)
            {
                if(customerManager.customerManagers.Players[i].Score<=0)
                {
                    customerManager.customerManagers.Players[i].Score = 0;
                }
            }
            }
        temp = 0f;
        while (temp < 0.9f)
        {
            transform.position = Vector3.Lerp(end.transform.position, start.transform.position, temp);

            temp += 0.01f;
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
