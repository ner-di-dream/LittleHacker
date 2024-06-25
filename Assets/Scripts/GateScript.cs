using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    private ObjectData gateData;
    private SpriteRenderer gateRenderer;
    private Collider2D gateCollider;

    void Start()
    {
        gateData = GetComponent<ObjectData>();
        gateRenderer = GetComponent<SpriteRenderer>();
        gateCollider = GetComponent<Collider2D>();
        SetGateState(true);
    }

    void Update()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        Player player = playerObj.GetComponent<Player>();

        if (player != null && gateData != null)
        {
            if (player.formulaCount == 1 && player.formulaTotalNum == gateData.num)
            {
                // ������ �����ϸ� Gate�� ��Ȱ��ȭ (��� ����)
                SetGateState(false);
            }
            else
            {
                // ������ �������� ������ Gate�� Ȱ��ȭ (��)
                SetGateState(true);
            }
        }
    }

    void SetGateState(bool isActive)
    {
        if (isActive)
        {
            // Gate�� Ȱ��ȭ (��)
            gateRenderer.color = new Color(gateRenderer.color.r, gateRenderer.color.g, gateRenderer.color.b, 1f);
            gameObject.tag = "Wall";
            gameObject.layer = LayerMask.NameToLayer("Wall"); // layer�� Wall�� ����
        }
        else
        {
            // Gate�� ��Ȱ��ȭ (��� ����)
            gateRenderer.color = new Color(gateRenderer.color.r, gateRenderer.color.g, gateRenderer.color.b, 0.5f);
            gameObject.tag = "Gate";
            gameObject.layer = LayerMask.NameToLayer("Gate"); // layer�� Gate�� ����
        }
    }
}