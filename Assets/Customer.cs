using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text NeedTxt;
    public Image WaitMeter;
    void Start()
    {
       // Precalculate();
    }
    public float temp;
    public int TimeDecreaseRate=1;
    public void Precalculate()
    {
        needed.Clear();
        isAngry = false;
        isActive = true;
        Gotten = false;
        Assist = null;
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
    public float time;
    public float seventyPercentTime;
    IEnumerator Timer()
    {
        while(time>1)
        {
            yield return new WaitForSeconds(1);
            time-=TimeDecreaseRate;
            WaitMeter.fillAmount = (float)(time * 1/((needed.Count*20)));
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
        NeedTxt.gameObject.SetActive(true);

        WaitMeter.transform.parent.gameObject.SetActive(true);
        WaitMeter.fillAmount = 1;
         temp = 0f;
        while(temp<0.9f )
        {
            transform.position = Vector3.Lerp(start.transform.position, end.transform.position, temp);

            temp+=0.01f;
            yield return null;
        }
        StartCoroutine(Timer());
        NeededShowUI();
    }
    public void NeededShowUI()
    {
        NeedTxt.gameObject.SetActive(true);
        NeedTxt.text = "";
        for(int i=0;i<needed.Count;i++)
        {
            NeedTxt.text = NeedTxt.text + needed[i] ;
            if(i!=needed.Count-1)
            {
                NeedTxt.text = NeedTxt.text + ",";
            }
        }
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
        NeedTxt.gameObject.SetActive(false);
        WaitMeter.transform.parent.gameObject.SetActive(false);
        WaitMeter.fillAmount = 1;
        WaitMeter.color = Color.green;
        isActive = false;

    }
    // Update is called once per frame
    void Update()
    {

    }
}
