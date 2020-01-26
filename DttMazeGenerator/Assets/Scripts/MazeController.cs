using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MazeController : MonoBehaviour
{

    public Button generateButtone;
    public GameObject maze;

    // Start is called before the first frame update
    void Start()
    {
        //maze = GameObject.Find("Maze Generator");
        //generateButtone = GameObject.Find("Generate Button");

        generateButtone.onClick.AddListener(CallMaze);
    }

    void CallMaze(){
      //  Debug.Log("Generate");
       // maze.GetComponent<Maze>().CreateMaze();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
