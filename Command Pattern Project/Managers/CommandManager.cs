using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandManager : MonoBehaviour
{
    private static CommandManager _instance;
    public static CommandManager Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("CommandManager is NULL");
            return _instance;
        }
    }

    private List<ICommand> _commandBuffer = new List<ICommand>();

    private void Awake()
    {
        _instance = this;
    }

    //Create a method to add a command to the buffer
    public void AddToBuffer(ICommand command)
    {
        _commandBuffer.Add(command);
    }

    //Create a play routine triggered by a play method that's going to play back all the commands
    //1 Second delay
    public void Play()
    {
        StartCoroutine(PlayRoutine());
    } 
    IEnumerator PlayRoutine()
    {
        Debug.Log("Running Play Routine...");

        foreach(ICommand command in _commandBuffer)
        {
            command.Execute();
            yield return new WaitForSeconds(1.0f);
        }

        Debug.Log("Finished.");
    }

    //Create a Rewind routine triggered by a rewind method that is going to play in reverse with a 1 second delay
    public void Reverse()
    {
        StartCoroutine(ReverseRoutine());
    }
    IEnumerator ReverseRoutine()
    {
        foreach(ICommand command in Enumerable.Reverse(_commandBuffer))
        {
            command.Undo();
            yield return new WaitForSeconds(1.0f);
        }
    }

    //Done = Finished with changing colors. Turn them all white
    public void Done()
    {
        var cubes = GameObject.FindGameObjectsWithTag("Cube");
        foreach(var cube in cubes)
        {
            cube.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        
    }

    //Reset = Clear the command buffer
    public void ClearBuffer()
    {
        _commandBuffer.Clear();
    }
}
