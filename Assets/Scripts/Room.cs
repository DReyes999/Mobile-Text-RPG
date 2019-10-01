using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    public class Room 
    {
        public Chest Chest { get; set; }
        public Enemy Enemy { get; set; }
        public bool Exit { get; set; }
        public bool Empty { get; set; }
        public Vector2 RoomIndex { get; set; }

        //Overload explicit room constructor
        //Takes arguments passed in and creates a room with those parameters
        public Room(Chest chest, Enemy enemy, bool empty, bool exit)
        {
            this.Chest = chest;
            this.Enemy = enemy;
            this.Empty = empty;
            this.Exit = exit;

        }

        //random room constructor
        public Room()
        {
            int roll = Random.Range(0, 30);
            if (roll > 0 && roll < 6)
            {
                Enemy = EnemyDatabase.Instance.GetRandomEnemy();
                Enemy.RoomIndex = RoomIndex;
            }
            else if (roll > 10 && roll < 15)
            {
                Chest = new Chest();
            }
            else
            {
                Empty = true;
            }
            // Add in another range case here to determine if there are more exits than just one
        }


    }
}
