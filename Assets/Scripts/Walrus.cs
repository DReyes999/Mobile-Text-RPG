﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    public class Walrus : Enemy
    {

        // Use this for initialization
        void Start()
        {
            Energy = 15;
            Attack = 3;
            Defence = 5;
            Gold = 30;
            Inventory.Add("Walrus Tusk");
        }

     
    }
}