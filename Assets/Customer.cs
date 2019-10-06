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
    public List<char> needed = new List<char>();
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
        StartCoroutine(Move());
        StartCoroutine(Timer());
        

    }
    public int time;
    IEnumerator Timer()
    {
        while(time>1)
        {
            yield return new WaitForSeconds(1);
            time-=TimeDecreaseRate;
        }
        StartCoroutine(MoveBack());

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
    }
    IEnumerator MoveBack()
    {
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
