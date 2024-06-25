using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// �浹ó���� ���ؼ� BoxCollider ������Ʈ�� isTrigger�� üũ�ص׾��

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
        Debug.Log("AllOperatorScript �浹 ����: " + other.name);
 
        // �÷��̾�� �浹�� ��� ����
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player�� �浹: " + other.name);

            // Number �±׸� ��� ã��
            GameObject[] numberObjects = GameObject.FindGameObjectsWithTag("Number");

            // �� ������Ʈ�� Num ���� ������ ����
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

            // ������ ������ ������Ʈ ����
            gameObject.SetActive(false);
        }
    }

}
