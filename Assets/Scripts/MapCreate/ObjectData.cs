using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectData : MonoBehaviour
{
    public TileData tileData;

    SpriteRenderer sR;
    TMP_Text textUI;

    public bool boxTrigger = false;
    public bool boxStop = false;
    public Vector2 boxMoveDir = new Vector2(0, 0);
    private float boxMoveSpeed = 5;

    private int tmpTurn = 0;

    public void Start()
    {
        sR = GetComponent<SpriteRenderer>();

        if (transform.childCount > 0)
        {
            textUI = transform.GetChild(0).GetComponent<TextMeshPro>();
        }

        tmpTurn = GameManager.playerTurn;

        switch(tileData.type)
        {
            case TileType.Background:
                tag = "Untagged";
                gameObject.layer = 0; // Default
                sR.sprite = null;
                sR.sortingOrder = -2;
                GetComponent<BoxCollider2D>().enabled = false;
                break;

            case TileType.Wall:
                tag = "Wall";
                gameObject.layer = 7; // Wall
                sR.sprite = Resources.Load<Sprite>("Sprites/Background_block");
                sR.sortingOrder = 0;
                break;

            case TileType.Player:
                tag = "Player";
                gameObject.layer = 6; // Player
                sR.sprite = Resources.Load<Sprite>("Sprites/MainCharacter");
                sR.sortingOrder = 0;
                break;

            case TileType.Number:
                tag = "Number";
                gameObject.layer = 8; // Item
                sR.sprite = Resources.Load<Sprite>("Sprites/number_block");
                sR.sortingOrder = -1;
                textUI.text = tileData.value.ToString();
                break;

            case TileType.Operator:
                tag = "Operator";
                gameObject.layer = 8; // Item
                sR.sprite = Resources.Load<Sprite>("Sprites/Operator");
                sR.sortingOrder = -1;
                textUI.text = tileData.oper;
                break;

            case TileType.Door:
                tag = "Door";
                gameObject.layer = 9; // Door
                sR.sprite = Resources.Load<Sprite>("Sprites/endNumber_block");
                sR.sortingOrder = 0;
                textUI.text = tileData.value.ToString();
                break;

            case TileType.Box:
                tag = "Box";
                gameObject.layer = 10; // Trigger
                sR.sprite = Resources.Load<Sprite>("Sprites/Box");
                sR.sortingOrder = 0;
                break;
            
            case TileType.AllOper:
                tag = "AllOperator";
                gameObject.layer = 8; // Item (¿ø·¡ TrapÀÎµ¥ °íÃÆÀ½)
                sR.sprite = Resources.Load<Sprite>("Sprites/AllOperator");
                sR.sortingOrder = -1;
                textUI.text = tileData.value.ToString();
                break;

            case TileType.Trap:
                tag = "Trap";
                gameObject.layer = 12; // Trap (¿ø·¡ GateÀÎµ¥ °íÃÆÀ½)
                sR.sprite = Resources.Load<Sprite>("Sprites/Trap");
                sR.sortingOrder = -1;
                textUI.text = tileData.value.ToString();
                break;

            case TileType.Gate:
                tag = "Gate";
                gameObject.layer = 10; // Trigger
                sR.sprite = Resources.Load<Sprite>("Sprites/Gate");
                sR.sortingOrder = -1;
                textUI.text = tileData.value.ToString();
                break;

            
        }
    }

    public void Update()
    {
        // ¸®¼Â
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
            // ¹Ú½º°¡ º®°ú ºÎµúÃÆÀ» °æ¿ì
            transform.position = new Vector2(hitWall.transform.position.x - boxMoveDir.x, hitWall.transform.position.y - boxMoveDir.y);
            boxStop = true;
            boxTrigger = false;
            boxMoveDir = Vector2.zero;
        }
    }
}
