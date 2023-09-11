using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum Direct
{
    Up,
    Down,
    Right,
    Left,
    None
}
public class Player : GameManager
{
    private Rigidbody rb;
    private Vector3 startPos;
    public float speed;

    public float rotateSpeed = 100f;

    public Stack<GameObject> bricks = new Stack<GameObject>();

    public GameObject bridge;

    public GameObject brickPrefab;

    public GameObject startPrefab;

    private Vector3 direction;

    private bool isMoving = false;

    private bool hasBridgeBrick = false;

    private Vector3 mouseStart;

    private Vector3 mouseEnd;

    private Vector3 mouseDirection;
    public Direct GetDirect(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        Direct draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? Direct.Right : Direct.Left;
            direction = new Vector3(Mathf.Sign(mouseDirection.x), 0, 0);
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? Direct.Up : Direct.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }



    public override void Oninit()
    {
        base.Oninit();
        rb = GetComponent<Rigidbody>();
        startPos = startPrefab.transform.position + new Vector3(0, 1, 0);
        ResetPosition();
    }

    void Update()
    {
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            mouseStart = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseEnd = Input.mousePosition;
            mouseDirection = mouseEnd - mouseStart;
            GetDirect(mouseDirection);
            isMoving = true;         
        }

        if (Mathf.Abs(mouseDirection.x) > Mathf.Abs(mouseDirection.y))
        {
            direction = new Vector3(Mathf.Sign(mouseDirection.x), 0, 0);
        }
        else
        {
            direction = new Vector3(0, 0, Mathf.Sign(mouseDirection.y));
        }

        if (isMoving)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            transform.position += direction * speed * Time.deltaTime;
            Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward * 0.5f, 0.1f);
            // Nếu có thì gọi hàm OnTriggerEnter để xử lý va chạm
            foreach (Collider collider in colliders)
            {
                OnTriggerEnter(collider);
            }
        }

        Debug.DrawRay(transform.position , (transform.forward) * 0.5f, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(transform.position , (transform.forward ), out hit, 0.5f))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);
            
            if (hit.collider.tag == "Wall")
            {
                isMoving = false;
            }
        }
    }

    void ResetPosition()
    {
        transform.position = startPos; 
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject body = transform.GetChild(0).gameObject;

        // Nếu gameobject là "BridgeBrick" thì bỏ qua không xử lý
        if (other.gameObject.tag == "BridgeBrick")
        {
            // Đặt trạng thái của bridge là đã có bridgebrick
            hasBridgeBrick = true;
            return;
        }
        if (other.gameObject.tag == "Brick")
        {
            bricks.Push(other.gameObject);
            Transform pbrick = Instantiate(brickPrefab, transform.position + new Vector3(0, bricks.Count * 0.2f, 0), Quaternion.identity).transform;
            pbrick.SetParent(transform);
            Destroy(other.gameObject);
            body.transform.position = new Vector3(body.transform.position.x, transform.position.y + pbrick.transform.position.y, body.transform.position.z);
        }
        if (other.gameObject.tag == "Bridge")
        {
            hasBridgeBrick = false;
            // Kiểm tra xem nhân vật có viên gạch nào trên người hay không
            if (bricks.Count > 0)
            {
                if (!hasBridgeBrick)
                {
                    // Lấy ra viên gạch trên cùng của ngăn xếp
                    GameObject brick = bricks.Pop();
                    // Tạo một viên gạch mới trên thay thế bridge
                    Transform bridgeBrick = Instantiate(brickPrefab, other.transform.position, other.transform.rotation).transform;
                    Destroy(other.gameObject);
                    // Thay đổi tag của viên gạch mới thành "BridgeBrick"
                    bridgeBrick.tag = "BridgeBrick";
                    Destroy(gameObject.transform.GetChild(gameObject.transform.childCount - 1).gameObject);
                    body.transform.position = new Vector3(body.transform.position.x, body.transform.position.y - 0.2f, body.transform.position.z);
                }
            }
        }
    }
}
