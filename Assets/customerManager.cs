using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class customerManager : MonoBehaviour
{
    public static customerManager customerManagers;
    public List<char> totalIngredients = new List<char>();
    public List<GameObject> startPoint = new List<GameObject>();
    public List<GameObject> EndPoint = new List<GameObject>();
    public List<Customer> totalCustomer = new List<Customer>();

    public List<Player> Players = new List<Player>();
    public GameObject xStart;
    public GameObject xend;
    public GameObject yStart;
    public GameObject yEnd;
    public GameObject PowerUp;

    public Sprite score;
    public Sprite speed;
    public Sprite time;
    public Text Instructions;
    // Start is called before the first frame update
    private void Awake()
    {
        customerManagers = this;
        StartCoroutine(customerBias());
        //RandomSpawnPowerUp();
    }
    void Start()
    {
    }
    public int PowerUpNum;
    public void RandomSpawnPowerUp()
    {
        PowerUp.gameObject.SetActive(true);
        PowerUp.transform.position = new Vector2(Random.Range(xStart.transform.position.x, xend.transform.position.x), Random.Range(yStart.transform.position.y, yEnd.transform.position.y));
    }

    public IEnumerator customerBias()
    {
        while(true)
        {
            int rand = Random.Range(0, 3);
          if(  totalCustomer[rand].isActive==false)
            {
                totalCustomer[rand].Precalculate();
            }

            yield return new WaitForSeconds(3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
