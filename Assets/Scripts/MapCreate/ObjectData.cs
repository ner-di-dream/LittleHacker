using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public int num;
    public int doorNum;
    public string oper;

    public bool boxTrigger = false;
    public bool boxStop = false;
    public Vector2 boxMoveDir = new Vector2(0, 0);
    private float boxMoveSpeed = 5;

    private int tmpTurn = 0;

    public void Start()
    {
        tmpTurn = GameManager.playerTurn;
    }

    public void Update()
    {
        // 리셋
        if (tmpTurn != GameManager.playerTurn && !boxTrigger)
        {
            ResetStart();
            tmpTurn = GameManager.playerTurn;
        }
    }

    public void FixedUpdate()
    {
        if (boxTrigger)
        {
            BoxMove();
        }
    }

    void ResetStart()
    {
        boxStop = false;
        boxTrigger = false;
    }

    public void BoxMove()
    {
        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, boxMoveDir, 0.6f, LayerMask.GetMask("Wall"));

        transform.Translate(boxMoveDir * boxMoveSpeed * Time.deltaTime);
        if (hitWall)
        {
            Debug.Log(hitWall);
            // 박스가 벽과 부딪쳤을 경우
            transform.position = new Vector2(hitWall.transform.position.x - boxMoveDir.x, hitWall.transform.position.y - boxMoveDir.y);
            boxStop = true;
            boxTrigger = false;
            boxMoveDir = Vector2.zero;
        }
    }
}
