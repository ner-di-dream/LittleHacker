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
                // 조건을 충족하면 Gate를 비활성화 (통과 가능)
                SetGateState(false);
            }
            else
            {
                // 조건을 충족하지 않으면 Gate를 활성화 (벽)
                SetGateState(true);
            }
        }
    }

    void SetGateState(bool isActive)
    {
        if (isActive)
        {
            // Gate를 활성화 (벽)
            gateRenderer.color = new Color(gateRenderer.color.r, gateRenderer.color.g, gateRenderer.color.b, 1f);
            gameObject.tag = "Wall";
            gameObject.layer = LayerMask.NameToLayer("Wall"); // layer를 Wall로 설정
        }
        else
        {
            // Gate를 비활성화 (통과 가능)
            gateRenderer.color = new Color(gateRenderer.color.r, gateRenderer.color.g, gateRenderer.color.b, 0.5f);
            gameObject.tag = "Gate";
            gameObject.layer = LayerMask.NameToLayer("Gate"); // layer를 Gate로 설정
        }
    }
}