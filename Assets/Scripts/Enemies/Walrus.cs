using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    public class Walrus : Enemy
    {

        // Use this for initialization
        void Start()
        {
            // TODO: Assign random stats when created
            // TODO: Assign random Loot from a loot table
            // TODO: Assign Random Gold
            // TODO: Assign some kind of dynamic description

            Energy = 15;
            MaxEnergy = 15;
            Attack = 3;
            Dice = 20;
            Defence = 10;
            Gold = 30;
            Description = "Walrus";
            Inventory.Add("Walrus Tusk");
        }

     
    }
}