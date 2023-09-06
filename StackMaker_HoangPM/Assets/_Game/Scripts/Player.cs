using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direct
{
    Up,
    Down,
    Right,
    Left,
    None
}
public class Player : MonoBehaviour
{
    private Vector2 startPos;

    public Direct GetDirect(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        Direct draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? Direct.Right : Direct.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? Direct.Up : Direct.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }

    public void Oninit()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 endPos = Input.mousePosition;

            Vector2 dragVector = endPos - startPos;

            GetDirect(dragVector);
        }

    }

    public void Takebrick()
    {

    }

    public void Control()
    {
        
    }
}
