using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ڰ� �Ծ���ϴ� ����
//    1. player�� �����̰� ������ -> �����ڿ� ���ڸ� ���ÿ� �������� ��츦 ����. player �켱
//    2. player�� �����̰� ���� ������ -> player�� item�� �Դ� �˰���� �����ϰ� ����

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
