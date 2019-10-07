using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public int MoveAmount;
    public List<char> VegetablesInHand = new List<char>();
    public List<char> ChoppedVegetable = new List<char>();

    public List<Button> vegInHandBtn = new List<Button>();
    public List<Button> ChoppedVegBtn = new List<Button>();
    public int Score;
    public int PlayerTime = 300;
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode leftpick;
    public KeyCode rightpick;
    public KeyCode container;

    public Text PlayerTimext;
    public Text PlayerScoreTxt;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
     //   rb.AddForce(Vector2.up*MoveAmount, ForceMode2D.Force  );
        rb.velocity = Vector3.zero;
        StartCoroutine(playerTimer());

    }
    IEnumerator playerTimer()
    {
        while(PlayerTime>1)
        {
            yield return new WaitForSeconds(1f);
            PlayerTimext.text = PlayerTime.ToString();
            PlayerTime--;
        }
    }
    public GameObject Load;
    public bool NoMove;
    // Update is called once per frame
    public Vector3 dir;
    void FixedUpdate()
    {
        PlayerScoreTxt.text = Score.ToString();
        if(Input.GetKey(up))
        {
            dir = Vector2.up;

        }else
        {
            dir = Vector3.zero;
        }
         if(Input.GetKey(down))
        {
            dir = Vector2.down;

        }
        if (Input.GetKey(left))
        {
            dir = Vector2.left;

        }
        if (Input.GetKey(right))
        {
            dir = Vector2.right;
        }
        if (NoMove == false)
        {if(PlayerTime>=1)
            rb.velocity = dir * new Vector2(MoveAmount, MoveAmount);

        }
    }
    public string Name;
    void OnTriggerStay2D(Collider2D col)
    {
        char[] arr = col.gameObject.name.ToCharArray();
        Debug.Log( arr[0]);

        if (col.tag == "Trash")
        {
          //  Debug.LogError("Trash");
            if (Input.GetKeyDown(leftpick))
            {
                if (ChoppedVegetable.Count != 0)
                {
                    ChoppedVegetable.Clear();
                    RefreshChoppedBtn();
                    Score -= 5;
                    if (Score <= 0)
                        Score = 0;
                }
            }
        }
            if (col.tag == "Veg")
        {
            if (Input.GetKeyDown(leftpick))
            {
                if (VegetablesInHand.Count < 2)
                {
                    if (!VegetablesInHand.Contains(arr[0]))
                    {
                        VegetablesInHand.Add(arr[0]);
                        StartCoroutine(instruc(Name + " Took " + arr[0] + " Vegetable"));
                        TakeVegetables();
                    }
                }
            }
        }

        if (col.tag == "Chop")
        {
            if(Input.GetKeyDown(leftpick))
            {
                Debug.LogError("chop");
                if (VegetablesInHand.Count != 0)
                {
                    StartCoroutine(instruc(Name + " Chopped " + VegetablesInHand[0] + " Vegetable"));

                    ChopVegetable(0);

                }
            }
            if (Input.GetKeyDown(rightpick))
            {
                if (VegetablesInHand.Count == 2)
                {
                    StartCoroutine(instruc(Name + " Chopped " + VegetablesInHand[1] + " Vegetable"));

                    ChopVegetable(1);
                }
            }
        }
        if (col.gameObject.GetComponent<Plate>())
        {
            if (col.gameObject.GetComponent<Plate>().holdBool == false)
            {
                if (VegetablesInHand.Count!=0)
                {
                    if (Input.GetKeyDown(leftpick))
                    {
                        StartCoroutine(instruc(Name + " Kept " + VegetablesInHand[0] + " Vegetable"));

                        PlaceVegetable(0, col.gameObject.GetComponent<Plate>());
                        Debug.LogError(VegetablesInHand.Count);

                        
                    }
                    if (VegetablesInHand.Count == 2)
                    {
                        Debug.LogError(col.gameObject.name);

                        if (Input.GetKeyDown(rightpick))
                        {
                            Debug.LogError(col.gameObject.name);
                            StartCoroutine(instruc(Name + " Kept " + VegetablesInHand[1] + " Vegetable"));

                            PlaceVegetable(1, col.gameObject.GetComponent<Plate>());
                        }

                    }
                }

            }else
            {
                if (Input.GetKeyDown(container))
                {
                    StartCoroutine(instruc(Name + " Took " + col.gameObject.GetComponent<Plate>().Hold + " Vegetable"));

                    TakeVegFromPlate(col.gameObject.GetComponent<Plate>().Hold, col.gameObject.GetComponent<Plate>());
                }
                }
        }
        if(col.gameObject.GetComponent<Customer>())
        {
            col.gameObject.GetComponent<Customer>().Assist = this;
            if (Input.GetKeyDown(leftpick))
            {
                bool pass=false;
                if (ChoppedVegetable.Count == col.gameObject.GetComponent<Customer>().needed.Count)
                {
                    for(int i=0;i<ChoppedVegetable.Count;i++)
                    {
                        if(col.gameObject.GetComponent<Customer>().needed.Contains(ChoppedVegetable[i]))
                        {
                        }else
                        {
                            pass = true;
                            col.gameObject.GetComponent<Customer>().WaitMeter.color = Color.red;
                            col.gameObject.GetComponent<Customer>().isAngry = true;
                            col.gameObject.GetComponent<Customer>().TimeDecreaseRate *= 2;
                            Debug.LogError("wrong");

                            break;
                        }
                    }


                }else
                {
                    col.gameObject.GetComponent<Customer>().WaitMeter.color = Color.red;

                    col.gameObject.GetComponent<Customer>().isAngry = true;
                    col.gameObject.GetComponent<Customer>().TimeDecreaseRate *= 2;
                    pass = true;

                    Debug.LogError("wrong");

                }
                if(pass==false)
                {
                    col.gameObject.GetComponent<Customer>().Gotten = true;
                }
            }
            
        }
        if(col.tag == "powerup")
        {
            if(canCaughtPowerUp)
            {
                customerManager.customerManagers.PowerUp.gameObject.SetActive(false);
                customerManager.customerManagers.PowerUpNum = Random.Range(0, 3);
                canCaughtPowerUp = false;
                if(customerManager.customerManagers.PowerUpNum == 0)
                {
                    Debug.LogError("speed");
                    customerManager.customerManagers.PowerUp.GetComponent<SpriteRenderer>().sprite = customerManager.customerManagers.speed;
                    StartCoroutine(instruc(Name + " Gained Speed"));

                    StartCoroutine(PlayerSpeedUp());
                }else if(customerManager.customerManagers.PowerUpNum == 1)
                {
                    customerManager.customerManagers.PowerUp.GetComponent<SpriteRenderer>().sprite = customerManager.customerManagers.time;

                    Debug.LogError("time");
                    StartCoroutine(instruc(Name + " Gained Time"));

                    PlayerTime += 30;
                }else if(customerManager.customerManagers.PowerUpNum == 2)
                {
                    Debug.LogError("score");
                    customerManager.customerManagers.PowerUp.GetComponent<SpriteRenderer>().sprite = customerManager.customerManagers.score;

                    StartCoroutine(instruc(Name + " Gained Score"));

                    Score += 10;
                }
            }
        }
     }

    IEnumerator PlayerSpeedUp()
    {
        MoveAmount += 5;
        yield return new WaitForSeconds(5f);
        MoveAmount -= 5;
    }
    public bool canCaughtPowerUp;
    public IEnumerator instruc(string temp)
    {
        customerManager.customerManagers.Instructions.text = temp;
        yield return new WaitForSeconds(1f);
        customerManager.customerManagers.Instructions.text = "";
    }
    public Text ResultTxt;
    public void ChopVegetable(int index)
    {
        char temp = VegetablesInHand[index];

        if (!ChoppedVegetable.Contains(temp))
        {
            VegetablesInHand.RemoveAt(index);

            ChoppedVegetable.Add(temp);
            ButtonRefresh();
            StartCoroutine(ChopWait());
        }
        RefreshChoppedBtn();
      //  ButtonRefresh();

    }
    IEnumerator ChopWait()
    {
        Load.gameObject.SetActive(true);
        NoMove = true;
        yield return new WaitForSeconds(2.5f);
        Load.gameObject.SetActive(false);
        NoMove = false;
    }
    public void RefreshChoppedBtn()
    {
        for (int i = 0; i < 6; i++)
        {
            ChoppedVegBtn[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < ChoppedVegetable.Count; i++)
        {
            ChoppedVegBtn[i].gameObject.SetActive(true);

            ChoppedVegBtn[i].transform.GetChild(0).GetComponent<Text>().text = ChoppedVegetable[i].ToString();
        }
        for(int i=0;i<VegetablesInHand.Count;i++)
        {
            vegInHandBtn[i].gameObject.SetActive(true);
            vegInHandBtn[i].transform.GetChild(0).GetComponent<Text>().text = VegetablesInHand[i].ToString();

        }
    }

    public void ButtonRefresh()
    {
         for(int i=0;i<2;i++)
        {
            vegInHandBtn[i].gameObject.SetActive(false);
        }
         for(int i=0;i<VegetablesInHand.Count;i++)
        {
            vegInHandBtn[i].transform.GetChild(0).GetComponent<Text>().text = VegetablesInHand[i].ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.childCount>=1)
        {
            collision.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    

    public void PlaceVegetable(int index,Plate plate)
    {
        if (plate.holdBool == false)
        {
            char temp = VegetablesInHand[index];
            VegetablesInHand.RemoveAt(index);
            plate.holdBool = true;
            plate.Hold = temp;
            // vegInHandBtn[index].onClick.RemoveAllListeners();
            // vegInHandBtn[index].gameObject.SetActive(false);
            Refresh(plate);
            plate.btn.gameObject.SetActive(true);
            plate.btn.transform.GetChild(0).GetComponent<Text>().text = temp.ToString();
            plate.btn.interactable = true;
            plate.btn.onClick.AddListener(() => TakeVegFromPlate(temp, plate));
        }
    }
    public void TakeVegFromPlate(char veg,Plate plate)
    {
        if(VegetablesInHand.Count!=2)
        {
            if(!VegetablesInHand.Contains(veg))
            {
                VegetablesInHand.Add(veg);
                plate.btn.onClick.RemoveAllListeners();
                plate.btn.interactable = false;
                plate.btn.transform.GetChild(0).GetComponent<Text>().text="";
                Refresh(plate);
                plate.holdBool = false;
                plate.Hold = ' ';
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.childCount >= 1)
        {
            collision.transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    public void Refresh(Plate plate)
    {
        for (int i = 0; i < vegInHandBtn.Count; i++)
        {
            vegInHandBtn[i].gameObject.SetActive(false);
            vegInHandBtn[i].GetComponent<Button>().onClick.RemoveAllListeners();

        }
        for (int i = 0; i < VegetablesInHand.Count; i++)
        {
            vegInHandBtn[i].gameObject.SetActive(true);
            vegInHandBtn[i].transform.GetChild(0).GetComponent<Text>().text = VegetablesInHand[i].ToString();

            int k = i;
            vegInHandBtn[i].onClick.AddListener(() => PlaceVegetable(k, plate));
        }
    }

    public void TakeVegetables()
    {
        for(int i=0;i<vegInHandBtn.Count;i++)
        {
            vegInHandBtn[i].onClick.RemoveAllListeners();
            vegInHandBtn[i].gameObject.SetActive(false);
        }
        for(int i=0;i<VegetablesInHand.Count;i++)
        {
            vegInHandBtn[i].gameObject.SetActive(true);
            vegInHandBtn[i].transform.GetChild(0).GetComponent<Text>().text = VegetablesInHand[i].ToString();
        }
    }
}
