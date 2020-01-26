using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;

public class Maze : MonoBehaviour
{
    [System.Serializable]
    public class Cell   
    {
        public bool visited;
        public GameObject north;    //1
        public GameObject east;     //2
        public GameObject west;     //3
        public GameObject south;    //4
    }
    public GameObject wall;
    private float wallLenght = 1.0f;
    public int xSize = 5;
    public int ySize = 5;
    private Vector3 initialPos;
    private GameObject wallHolder;
    private Cell[] cells;
    public int currentCell = 0;
    private int totalCells;
    private int visitedCells = 0;
    private bool startedBuilding = false;
    private int currentNeighbor = 0;
    private List<int> lastCells;
    private int backingUp = 0;
    private int wallToBreak = 0;
    private Slider WidthSilder;
    private Slider HeightSlider;
    private GameObject camera; 

    // Start is called before the first frame update
    void Start()
    {
        WidthSilder = GameObject.Find("Width Slider").GetComponent<Slider>();
        HeightSlider = GameObject.Find("Height Silder").GetComponent<Slider>();
        camera = GameObject.Find("Main Camera");
    }

    public void Generate(){
        Destroy(wallHolder);
        currentCell = 0;
        visitedCells = 0;
        startedBuilding = false;
        currentNeighbor = 0;
        backingUp = 0;
        wallToBreak = 0;
        CreateWalls();
    	camera.transform.position = new Vector3(camera.transform.position.x, 9 + 1 * ySize, camera.transform.position.z);
    }

    public void ChangeWidth(){
        xSize = (int)WidthSilder.value;
    }
    public void ChangeHeight(){
        ySize = (int)HeightSlider.value;
    }
    

    public void CreateWalls()
    {
        wallHolder = new GameObject();
        wallHolder.name = "Maze";
        initialPos = new Vector3((-xSize / 2) + wallLenght / 2, 0, (-ySize / 2) + wallLenght / 2);
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
        if(lastCells != null){
            lastCells.Clear();
        }
        lastCells = new List<int>();
        totalCells = xSize * ySize;

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
        if (visitedCells < totalCells)
        {
            if (startedBuilding)
            {
                GiveMeNeightbour();
                if (cells[currentNeighbor].visited == false && cells[currentCell].visited == true)
                {
                    BreakWall();
                    cells[currentNeighbor].visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbor;
                    if (lastCells.Count > 0)
                    {
                        backingUp = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = UnityEngine.Random.Range(0, totalCells);
                cells[currentCell].visited = true;
                visitedCells++;
                startedBuilding = true;
            }

            Invoke("CreateMaze", 0);
        }
    }

    void BreakWall()
    {
        Debug.Log(wallToBreak);
        switch (wallToBreak)
        {
            
            case 1:
                Destroy(cells[currentCell].north);
                Debug.Log(cells[currentCell] + "  North" );
                break;
            case 2:
                Destroy(cells[currentCell].east);
                Debug.Log(cells[currentCell] + "  East");
                break;
            case 3:
                Destroy(cells[currentCell].west);
                Debug.Log(cells[currentCell] + "  West");
                break;
            case 4:
                Destroy(cells[currentCell].south);
                Debug.Log(cells[currentCell] + "  South");
                break;
        }
    }

    void GiveMeNeightbour()
    {
        int lenght = 0;
        int[] neightbours = new int[4];
        int[] connectingWall = new int[4];
        int check = 0;

        check = ((currentCell + 1) / xSize);
        check -= 1;
        check *= xSize;
        check += xSize;

        //West
        if (currentCell + 1 < totalCells && (currentCell + 1) != check)
        {
            if (cells[currentCell + 1].visited == false)
            {
                neightbours[lenght] = currentCell + 1;
                connectingWall[lenght] = 3;
                lenght++;
            }
        }

        //East
        if (currentCell - 1 >= 0 && currentCell != check)
        {
            if (cells[currentCell - 1].visited == false)
            {
                neightbours[lenght] = currentCell - 1;
                connectingWall[lenght] = 2;
                lenght++;
            }
        }

        //North
        if (currentCell + xSize < totalCells)
        {
            if (cells[currentCell + xSize].visited == false)
            {
                neightbours[lenght] = currentCell + xSize;
                connectingWall[lenght] = 1;
                lenght++;
            }
        }

        //South
        if (currentCell - xSize >= 0)
        {
            if (cells[currentCell - xSize].visited == false)
            {
                neightbours[lenght] = currentCell - xSize;
                connectingWall[lenght] = 4;
                lenght++;
            }
        }

        if (lenght != 0)
        {
            int theChoseOne = UnityEngine.Random.Range(0, lenght);
            currentNeighbor = neightbours[theChoseOne];
            wallToBreak = connectingWall[theChoseOne];
        }
        else
        {
            if (backingUp > 0)
            {
                currentCell = lastCells[backingUp];
                backingUp--;
            }
        }
    }
}
