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
    public int currentCell = 0;
    private int totalCells;

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
        CreateMaze();
    }

    void CreateMaze()
    {
        GiveMeNeightbour();
    }
    void GiveMeNeightbour()
    {
        totalCells = xSize * ySize;
        int lenght = 0;
        int[] neightbours = new int[4];
        int check = 0;

        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;

        //West
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells [currentCell + 1].visited == false)
            {
                neightbours[lenght] = currentCell + 1;
                lenght++;
            }
        }

        //East
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neightbours[lenght] = currentCell - 1;
                lenght++;
            }
        }

        //North
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neightbours[lenght] = currentCell + xSize;
                lenght++;
            }
        }

        //South
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neightbours[lenght] = currentCell - xSize;
                lenght++;
            }
        }

        for (int i = 0; i < lenght; i++)
        {
            Debug.Log(neightbours[i]);
        }
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}
