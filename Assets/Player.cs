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
    public List<Button> vegInHandBtn = new List<Button>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
     //   rb.AddForce(Vector2.up*MoveAmount, ForceMode2D.Force  );
        rb.velocity = Vector3.zero;

    }

    // Update is called once per frame
    public Vector3 dir;
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W))
        {
            dir = Vector2.up;

        }else
        {
            dir = Vector3.zero;
        }
         if(Input.GetKey(KeyCode.S))
        {
            dir = Vector2.down;

        }
        if (Input.GetKey(KeyCode.A))
        {
            dir = Vector2.left;

        }
        if (Input.GetKey(KeyCode.D))
        {
            dir = Vector2.right;
        }
        rb.velocity = dir*new Vector2(MoveAmount,MoveAmount);
    }
    void OnTriggerStay2D(Collider2D col)
    {
        char[] arr = col.gameObject.name.ToCharArray();
        Debug.Log( arr[0]);
        if (col.tag == "Veg")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (VegetablesInHand.Count < 2)
                {
                    if (!VegetablesInHand.Contains(arr[0]))
                    {
                        VegetablesInHand.Add(arr[0]);
                        TakeVegetables();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Plate>())
        {
           if(collision.gameObject.GetComponent<Plate>().holdBool==false)
            {
                for(int i=0;i<vegInHandBtn.Count;i++)
                {
                    vegInHandBtn[i].gameObject.SetActive(false);
                    vegInHandBtn[i].GetComponent<Button>().onClick.RemoveAllListeners();

                }
                for(int i=0;i<VegetablesInHand.Count;i++)
                {
                    vegInHandBtn[i].gameObject.SetActive(true);
                    int k = i;
                    vegInHandBtn[i].onClick.AddListener(() => PlaceVegetable(k, collision.gameObject.GetComponent<Plate>()));
                }
            }else
            {
                collision.gameObject.GetComponent<Plate>().btn.interactable = true;
            }

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
        if (collision.gameObject.GetComponent<Plate>())
        {
            if (collision.gameObject.GetComponent<Plate>().holdBool == true)
            {
                collision.gameObject.GetComponent<Plate>().btn.interactable = false;
            }
            for (int i = 0; i < vegInHandBtn.Count; i++)
            {
              //  vegInHandBtn[i].gameObject.SetActive(false);
                vegInHandBtn[i].GetComponent<Button>().onClick.RemoveAllListeners();

            }
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
