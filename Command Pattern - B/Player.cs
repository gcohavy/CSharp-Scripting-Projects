using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    ICommand moveUp,moveDown,moveLeft,moveRight;

    [SerializeField]
    private float _speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            //Move up command
            moveUp = new MoveUpCommand(this.transform, _speed);
            moveUp.Execute();
            CommandManager.Instance.AddCommand(moveUp);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            //Move down command
            moveDown = new MoveDownCommand(this.transform, _speed);
            moveDown.Execute();
            CommandManager.Instance.AddCommand(moveDown);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            //Move left command
            moveLeft = new MoveLeftCommand(this.transform, _speed);
            moveLeft.Execute();
            CommandManager.Instance.AddCommand(moveLeft);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            //Move right command
            moveRight = new MoveRightCommand(this.transform, _speed);
            moveRight.Execute();
            CommandManager.Instance.AddCommand(moveRight);
        }
    }
}
