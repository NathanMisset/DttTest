using UnityEngine;

public class Maze : MonoBehaviour
{
    public GameObject wall;
    public float wallLenght = 1.0f;
    public int xSize = 5;
    public int ySize = 5;
    private Vector3 initialPos;
    private GameObject WallHolder;
    // Start is called before the first frame update
    void Start()
    {
        CreateWalls();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateWalls()
    {
        WallHolder = new GameObject();
        WallHolder.name = "Maze";
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
                tempWall.transform.parent = WallHolder.transform;
            }
        }

        //For y Axis
        for (int i = 0; i <= ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLenght), 0, initialPos.z + (i * wallLenght) - wallLenght);
                tempWall = Instantiate(wall, myPos, Quaternion.Euler(0, 90.0f, 0)) as GameObject;
                tempWall.transform.parent = WallHolder.transform;
            }
        }
    }
}
