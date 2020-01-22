using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [System.Serializable]
    public class Cell   
    {
        public bool visited;
        public GameObject north;
        public GameObject east;
        public GameObject west;
        public GameObject south;
    }
    public GameObject wall;
    public float wallLenght = 1.0f;
    public int xSize = 5;
    public int ySize = 5;
    private Vector3 initialPos;
    private GameObject wallHolder;
    private Cell[] cells;

    // Start is called before the first frame update
    void Start()
    {
        CreateWalls();
    }

    void CreateWalls()
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";
        initialPos = new Vector3((-xSize / 2) + wallLenght / 2, 0, ((ySize / 2) + wallLenght / 2));
        Vector3 myPos = initialPos;
        GameObject tempWall;
        //For x Axis
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j <= xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLenght) - wallLenght / 2, 0, initialPos.z + (i * wallLenght) - wallLenght / 2);
                tempWall = Instantiate(wall, myPos, Quaternion.identity) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }

        //For y Axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLenght), 0, initialPos.z + (i * wallLenght) - wallLenght);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0, 90.0f, 0)) as GameObject;
                tempWall.transform.parent = wallHolder.transform;
            }
        }
        CreateCells();
    }
    
    void CreateCells()
    {
        GameObject[] allWalls;
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        cells = new Cell[xSize * ySize];
        int eastWestProcess = 0;
        int childProcess = 0;
        int termCount = 0;

        //Gets All the children
        for (int i = 0; i < children; i++)
        {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        //Assigns walls to the cells
        for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++)
        {
            cells[cellprocess] = new Cell();
            cells[cellprocess].east = allWalls[eastWestProcess];
            cells[cellprocess].south = allWalls[childProcess + (xSize+1) * ySize];

            if (termCount == xSize)
            {
                eastWestProcess += 2;
                termCount = 0;
            }
            else
            {
                eastWestProcess++;
            }
            termCount++;
            childProcess++;

            cells[cellprocess].west = allWalls[eastWestProcess];
            cells[cellprocess].north = allWalls[(childProcess + (xSize + 1) * ySize) + xSize - 1];
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
