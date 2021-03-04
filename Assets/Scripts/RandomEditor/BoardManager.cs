using UnityEngine;
using System;
using System.Collections.Generic;         //Allows us to use Lists.
using Random = UnityEngine.Random;         //Tells Random to use the Unity Engine random number generator.
using TMPro;

namespace Completed

{

    public class BoardManager : MonoBehaviour
    {
        // Using Serializable allows us to embed a class with sub properties in the inspector.
        [Serializable]
        public class Count
        {
            public int minimum;             //Minimum value for our Count class.
            public int maximum;             //Maximum value for our Count class.


            //Assignment constructor.
            public Count(int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }


        public float columns = 8;                                         //Number of columns in our game board.
        public float rows = 8;                                            //Number of rows in our game board.
        public Count wallCount = new Count(5, 9);                        //Lower and upper limit for our random number of walls per level.
        public Count weaponCount = new Count(1, 5);                        //Lower and upper limit for our random number of food items per level.
        public Count spawnerCount = new Count(1, 5);                        //Lower and upper limit for our random number of food items per level.
        public GameObject exit;                                            //Prefab to spawn for exit.
        public GameObject[] floorTiles;                                    //Array of floor prefabs.
        public GameObject[] wallTiles;                                    //Array of wall prefabs.
        public GameObject[] weaponTiles;                                    //Array of food prefabs.
        public GameObject[] spawnerTiles;                                    //Array of enemy prefabs.
        public GameObject[] outerWallTiles;                                //Array of outer tile prefabs.
        public Grille grille;

        private GameObject t_perso;
        private int m_MinColumns = -9;
        private int m_MinRows = -5;

        private Transform boardHolder;                                    //A variable to store a reference to the transform of our Board object.
        private List<Vector3> gridPositions = new List<Vector3>();    //A list of possible locations to place tiles.

        private Camera m_Camera;


        //Clears our list gridPositions and prepares it to generate a new board.
        void InitialiseList()
        {
            //Clear our list gridPositions.
            gridPositions.Clear();

            //Loop through x axis (columns).
            for (int x = m_MinColumns; x < columns - 1; x++)
            {
                //Within each column, loop through y axis (rows).
                for (int y = m_MinRows; y < rows - 1; y++)
                {
                    //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }

        private void Update()
        {
            

        }
        //Sets up the outer walls and floor (background) of the game board.
        void BoardSetup()
        {
            //Instantiate Board and set boardHolder to its transform.
            boardHolder = new GameObject("Board").transform;

            //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
            for (int x = m_MinColumns; x < columns + 1; x++)
            {
                //Loop along y axis, starting from -1 to place floor or outerwall tiles.
                for (int y = m_MinRows; y < rows + 1; y++)
                {
                    //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                    GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    Tuile t_tuile = toInstantiate.GetComponent<Tuile>();
                    t_tuile.x = (uint)(x + Math.Abs(m_MinColumns));
                    t_tuile.y = (uint)(y + Math.Abs(m_MinRows));

                    //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                    if (x == m_MinColumns || x == columns && y > m_MinRows || y > rows)
                        toInstantiate = outerWallTiles[0];
                    if (x < m_MinColumns || x < columns && y == m_MinRows || y == rows)
                        toInstantiate = outerWallTiles[1];
                    if(x == m_MinColumns && y == rows)
                        toInstantiate = outerWallTiles[3];
                    if(x == m_MinColumns && y == m_MinRows)
                        toInstantiate = outerWallTiles[5];
                    if(x == columns && y == rows)
                        toInstantiate = outerWallTiles[2];
                    if(x==columns && y == m_MinRows)
                        toInstantiate = outerWallTiles[4];


                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance = Instantiate(toInstantiate, grille.GridToWorld(new Vector2Int((int)t_tuile.x, (int)t_tuile.y)), Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent(boardHolder);
                }
            }
        }


        //RandomPosition returns a random position from our list gridPositions.
        Vector3 RandomPosition()
        {
            //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            int randomIndex = Random.Range(0, gridPositions.Count);

            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            Vector3 randomPosition = gridPositions[randomIndex];
            randomPosition.z = -1;

            //Remove the entry at randomIndex from the list so that it can't be re-used.
            gridPositions.RemoveAt(randomIndex);

            //Return the randomly selected Vector3 position.
            return randomPosition;
        }


        //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
        {
            //Choose a random number of objects to instantiate within the minimum and maximum limits
            int objectCount = Random.Range(minimum, maximum + 1);

            //Instantiate objects until the randomly chosen limit objectCount is reached
            for (int i = 0; i < objectCount; i++)
            {
                //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                Vector3 t_randomPosition = RandomPosition();
                Vector2Int gridPos = grille.WorldToGrid(t_randomPosition);

                //Choose a random tile from tileArray and assign it to tileChoice
                GameObject t_tileChoice = tileArray[Random.Range(0, tileArray.Length)];
                List<EnnemySpawner> t_spawner;
                /*foreach (GameObject[] item in tileArray)
                {
                    if (item == spawnerTiles)
                    {
                        t_spawner = FindObjectOfType<GameHandler>().allspawners;
                        t_spawner.Add(item);
                    }
                }*/
                

                //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                GameObject g =  Instantiate(t_tileChoice,grille.GridToWorld(gridPos), Quaternion.identity);

                g.transform.parent = boardHolder;

                Tuile t_Tile = g.GetComponent<Tuile>();
                if (t_Tile)
                {
                    t_Tile.x = (uint)gridPos.x;
                    t_Tile.y = (uint)gridPos.y;
                }

            }
        }


        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene(int a_level)
        {

            Camera m_Camera = FindObjectOfType<Camera>();
            //Creates the outer walls and floor.
            BoardSetup();

            //Reset our list of gridpositions.
            InitialiseList();

            //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

            //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(weaponTiles, weaponCount.minimum, weaponCount.maximum);

            //Determine number of enemies based on current level number, based on a logarithmic progression
            //int t_spawnerCount = (int)Mathf.Log(a_level, 6f);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(spawnerTiles, spawnerCount.minimum, spawnerCount.maximum);

            //Instantiate the exit tile in the upper right hand corner of our game board
            t_perso = Instantiate(exit, new Vector3(m_MinColumns + 1, m_MinRows + 1, 0f), Quaternion.identity);
            t_perso.GetComponent<PersoController>().Cam = m_Camera;
            m_Camera.GetComponent<FollowTargetRandom>().Target = t_perso.transform;
            PersoController t_PersoController = t_perso.GetComponent<PersoController>();
            t_PersoController.ScoreText = FindObjectOfType<Canvas>().transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            t_PersoController.VieText = FindObjectOfType<Canvas>().transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            grille.AppellerTuiles(boardHolder.transform);
            t_PersoController.PlayerLastTuile = grille.GetTuile(grille.WorldToGrid(t_perso.transform.position));
            t_PersoController.EndPanel = FindObjectOfType<Canvas>().transform.GetChild(5).gameObject;
            t_perso.GetComponent<PersoInteract>().Interact = FindObjectOfType<Canvas>().transform.GetChild(3).transform.GetChild(0).gameObject;
            t_perso.GetComponent<Shooting>().MunitionsText = FindObjectOfType<Canvas>().transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        }
    }
}
