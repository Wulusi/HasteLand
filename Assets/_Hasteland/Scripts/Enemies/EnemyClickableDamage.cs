﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClickableDamage : MonoBehaviour
{
    [SerializeField]
    private Health health;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {

    }


    public void takeItems()
    {

    }

    //AutoSerialization
#if UNITY_EDITOR

    void GetRef()
    {
        if (!Application.isEditor)
        {
            return;
        }
        else
        {
            health = GetComponent<Health>();
        }
    }

#endif
}