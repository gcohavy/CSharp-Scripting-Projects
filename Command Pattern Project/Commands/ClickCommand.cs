using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCommand : ICommand
{
    private GameObject gameObject;
    private Color newColor;
    private Color _previousColor;

    public ClickCommand(GameObject cube, Color color)
    {
        this.gameObject = cube;
        this.newColor = color;
    }
    public void Execute()
    {
        _previousColor = gameObject.GetComponent<MeshRenderer>().material.color;
        gameObject.GetComponent<MeshRenderer>().material.color = newColor;
    }

    public void Undo()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = _previousColor;
    }
}
