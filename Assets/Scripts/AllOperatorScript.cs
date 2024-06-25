using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 충돌처리를 위해서 BoxCollider 컴포넌트에 isTrigger를 체크해뒀어요

public class AllOperatorScript : MonoBehaviour
{
    private TMP_Text textComponent;
    private string text;
    private char oper;
    private int num;

    private void Start()
    {
        textComponent = GetComponentInChildren<TMP_Text>();
        text = textComponent.text;
        oper = text[0];
        num = int.Parse(text.Substring(1));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("AllOperatorScript 충돌 감지: " + other.name);
 
        // 플레이어와 충돌한 경우 실행
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player와 충돌: " + other.name);

            // Number 태그를 모두 찾기
            GameObject[] numberObjects = GameObject.FindGameObjectsWithTag("Number");

            // 각 오브젝트의 Num 값에 연산을 진행
            foreach (GameObject obj in numberObjects)
            {
                ObjectData data = obj.GetComponent<ObjectData>();
                TMP_Text dataText = data.GetComponentInChildren<TMP_Text>();
                if (data != null)
                {
                    if (oper == '+')
                    {
                        data.num += num;
                    }
                    else if (oper == '-')
                    {
                        data.num -= num;
                    }
                    else if (oper == '*')
                    {
                        data.num *= num;
                    }
                    else
                    {
                        data.num /= num;
                    }
                    dataText.text = data.num.ToString();
                }
            }

            // 연산이 끝나면 오브젝트 끄기
            gameObject.SetActive(false);
        }
    }

}
