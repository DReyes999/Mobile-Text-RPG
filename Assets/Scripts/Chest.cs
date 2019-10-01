using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    public class Chest 
    {
        public string Item { get; set; }
        public int Gold { get; set; }
        public bool Trap { get; set; }
        public bool Heal { get; set; }
        public Enemy Enemy { get; set; }

        public Chest()
        {
            /* Using ternary operators: 
             * Allow us to assign a property or field based on a condition
             * and then decide what the value is that gets assigned
             * based on the result
             * 
             * Below, if the statement to the left of the question mark evaluates true
             * We will get a random enemy from the enemy database
             * 
             * If it is false we will complete the expression to the right of the colon
             */
            /*
            Enemy = Random.Range(0, 4) == 2 ? EnemyDatabase.Instance.GetRandomEnemy() : null; //20% chance to be an enemy
            Trap = Random.Range(0,7) == 2;  // 12.5% chance to be a Trap
            Heal = Random.Range(0, 4) == 2; // 20% chance to be a heal
            */

            //roll random numbers to determine what is in the chest
            if (Random.Range(0,7) == 2) // 12.5% chance to be a trap
            {
                Trap = true;
            }
            else if(Random.Range(0,4) == 2) //20% Chance to be a heal
            {
                Heal = true;
            }
            else if (Random.Range(0,4) == 2) // 20% Chance to be a monster
            {
                //Pull a random enemy out of EnemyDatabase and assign it to Enemy
                Enemy = EnemyDatabase.Instance.Enemies[Random.Range(0,EnemyDatabase.Instance.Enemies.Count)];
            }
            else // Else choose a random item and some gold
            {
                // create a random integer that is dynamic based on the length of ItemDatabase[]
                int itemToAdd = Random.Range(0, ItemDatabase.Instance.Items.Count);

                //Pull that item out of ItemDatabase and assign it to the "Item" variable)
                Item = ItemDatabase.Instance.Items[itemToAdd];
                Gold = Random.Range(20, 200);
            }
        }

    }
}
