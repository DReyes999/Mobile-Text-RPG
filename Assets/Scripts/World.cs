using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextRPG
{
    public class World : MonoBehaviour
    {
        public Room[,] Dungeon { get; set; }
        public Vector2 Grid;

        private void Awake()
        {
            //creates a dungeon array with a size defined by the public size in Grid
            Dungeon = new Room[(int)Grid.x, (int)Grid.y];
            //Below is the way we start methods which are coroutines
            StartCoroutine(GenerateFloor());
        }

        /*In the following method we are using IEnumerator to create a hacky coroutine 
        which will allow us to pause between dungeon generation and exit generation*/
        public IEnumerator GenerateFloor()
        {
            Debug.Log("Generating Floor");
            for(int x = 0; x < Grid.x; x++)
            {
                for (int y = 0; y < Grid.y; y++)
                {
                    //This is where an actuall Room of class Room is generated for each coordinate in the grid
                    Dungeon[x, y] = new Room
                    {
                        //Sets the Room Index of each room to the coordinates of the grid
                        RoomIndex = new Vector2(x, y)
                    };
                }
            }

            //this is where the pause for our coroutine is employed
            Debug.Log("Finding Exit Solutions");
            yield return new WaitForSeconds(3);

            //We need to create an exit and assign it to a coordinate in the grid

            //pick a random coordinate in the dungeon
            Vector2 exitLocation = new Vector2((int)Random.Range(0,Grid.x),(int)Random.Range(0,Grid.y));

            //then set the room at that coordinate to be an exit
            Dungeon[(int)exitLocation.x, (int)exitLocation.y].Exit = true;
            Dungeon[(int)exitLocation.x, (int)exitLocation.y].Empty = false;
            Debug.Log("Exit is at " + exitLocation);

        }

    }
}
