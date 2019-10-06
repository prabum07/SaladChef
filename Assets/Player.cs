using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public int MoveAmount;
    public List<char> VegetablesInHand = new List<char>();
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
                    }
                }
            }
        }
    }
}
