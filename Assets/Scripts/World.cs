using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TextRPG
{
    public class World : MonoBehaviour
    {
        public Room[,] Dungeon { get; set; }
        public char[,] MapIcon { get; set; } // Unicode icons to draw our map with
        public Vector2 Grid;
        public TextMeshProUGUI mapText;
        public Vector2 PlayerPosition { get; set; }

        char square = '\u2610';
        char basicMapTile = '\u25A0';
        char player = '\u263A';

        private void Awake()
        {
            //creates a dungeon array with a size defined by the public size in Grid
            Dungeon = new Room[(int)Grid.x, (int)Grid.y];
            MapIcon = new char[(int)Grid.x, (int)Grid.y];
            //Below is the way we start methods which are coroutines
            GenerateFloor();
        }

        public void Start()
        {
            DrawMap();
        }

        /*In the following method we are using IEnumerator to create a hacky coroutine 
        which will allow us to pause between dungeon generation and exit generation*/
        public void GenerateFloor()
        {
            Debug.Log("Generating Floor");
            for(int x = 0; x < Grid.x; x++)
            {
                //mapText.text += square.ToString() + " ";
                for (int y = 0; y < Grid.y; y++)
                {
                    //mapText.text += otherX.ToString() + " ";
                    //This is where an actual Room of class Room is generated for each coordinate in the grid
                    Dungeon[x, y] = new Room
                    {
                        //Sets the Room Index of each room to the coordinates of the grid
                        RoomIndex = new Vector2(x, y)
                    };
                }

            }

            //We need to create an exit and assign it to a coordinate in the grid
            //pick a random coordinate in the dungeon
            Vector2 exitLocation = new Vector2((int)Random.Range(0,Grid.x),(int)Random.Range(0,Grid.y));

            //then set the room at that coordinate to be an exit
            Dungeon[(int)exitLocation.x, (int)exitLocation.y].Exit = true;
            Dungeon[(int)exitLocation.x, (int)exitLocation.y].Empty = false;
            Dungeon[(int)exitLocation.x, (int)exitLocation.y].Enemy = null;
            Dungeon[(int)exitLocation.x, (int)exitLocation.y].Chest = null;
            Debug.Log("Exit is at " + exitLocation);
        }

        public void DrawMap()
        {
            for (int y = 0; y < Grid.y; y++)
            {
                for (int x = 0; x < Grid.x; x++)
                {
                    MapIcon[x, y] = basicMapTile;
                    //mapText.text += MapIcon[x, y].ToString() + " ";
                }
                //mapText.text += "\n";
            }

            MapIcon[(int)PlayerPosition.x, (int)PlayerPosition.y] = player;
            mapText.text = string.Empty;

            for (int y = 0; y < Grid.y; y++)
            {
                for (int x = 0; x < Grid.x; x++)
                {
                    //MapIcon[x, y] = basicMapTile;
                    mapText.text += MapIcon[x, y].ToString() + " ";
                }
                mapText.text += "\n";
            }
        }

    }


}
