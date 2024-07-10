using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//조력자가 먹어야하는 조건
//    1. player가 움직이고 있을때 -> 연산자와 숫자를 동시에 만났을때 경우를 생각. player 우선
//    2. player가 움직이고 있지 않을때 -> player가 item을 먹는 알고리즘과 동일하게 제작

public class Helper : MonoBehaviour
{
    public Player player;
    public Vector2 moveDir;
    private Vector2 tmpMoveDir;
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        moveDir = player.moveDir;
    }

    void HelperMove()
    {
        
    }
}
