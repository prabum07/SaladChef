﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class customerManager : MonoBehaviour
{
    public static customerManager customerManagers;
    public List<char> totalIngredients = new List<char>();
    public List<GameObject> startPoint = new List<GameObject>();
    public List<GameObject> EndPoint = new List<GameObject>();
    public List<Customer> totalCustomer = new List<Customer>();
    // Start is called before the first frame update
    private void Awake()
    {
        customerManagers = this;

    }
    void Start()
    {
    }

    public IEnumerator customerBias()
    {
        while(true)
        {
            int rand = Random.Range(0, 3);
          if(  totalCustomer[rand].isActive==false)
            {

            }

            yield return new WaitForSeconds(3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}