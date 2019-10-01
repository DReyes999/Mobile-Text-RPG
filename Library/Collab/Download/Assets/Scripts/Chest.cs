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
            //roll random numbers to determine what is in the chest
            if(Random.Range(0,7) == 2)
            {
                Trap = true;
            }
            else if(Random.Range(0,5) == 2)
            {
                Heal = true;
            }
            else if (Random.Range(0,5) == 2)
            {
                //Pull a random enemy out of EnemyDatabase and assign it to Enemy
                Enemy = EnemyDatabase.Instance.Enemies[Random.Range(0,EnemyDatabase.Instance.Enemies.Count)];
            }
            else
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
