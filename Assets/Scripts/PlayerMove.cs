using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed;
    public GameManager gameManager;

    float h;
    float v;
    bool isHorizonMove;
    Vector3 dirVec;

    Rigidbody2D rigid;
    Animator anim;
    public GameObject scanObject;

    // 모바일 할 때 쓰임
    public int up_Value;
    public int down_Value;
    public int left_Value;
    public int right_Value;
    public bool up_Down;
    public bool down_Down;
    public bool left_Down;
    public bool right_Down;
    public bool up_Up;
    public bool down_Up;
    public bool left_Up;
    public bool right_Up;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // PC or 모바일에서의 십자 이동
        h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal") + right_Value + left_Value;
        v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical") + up_Value + down_Value;

        bool hDown = gameManager.isAction ? false : Input.GetButtonDown("Horizontal") || right_Down || left_Down;
        bool vDown = gameManager.isAction ? false : Input.GetButtonDown("Vertical") || up_Down || down_Down;
        bool hUp = gameManager.isAction ? false : Input.GetButtonUp("Horizontal") || right_Up || left_Up;
        bool vUp = gameManager.isAction ? false : Input.GetButtonUp("Vertical") || up_Up || down_Up;

        if (hDown)
            isHorizonMove = true;
        else if (vDown)
            isHorizonMove = false;
        else if (hUp || vUp)
            isHorizonMove = h != 0;

        // 움직이는 애니메이션 설정
        if (anim.GetInteger("hAxisRaw") != h)
        {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);
        }
        else if (anim.GetInteger("vAxisRaw") != v)
        {  
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
        }
        else
            anim.SetBool("isChange", false);

        // 방향설정
        if (vDown && v == 1)
            dirVec = Vector3.up;
        else if (vDown && v == -1)
            dirVec = Vector3.down;
        else if (hDown && h == -1)
            dirVec = Vector3.left;
        else if (hDown && h == 1)
            dirVec = Vector3.right;

        // 스캔
        if (Input.GetButtonDown("Jump") && scanObject != null)
            gameManager.Action(scanObject);

        // Mobile은 Down, Up 변수가 로직이 끝나면 false로 초기화
        up_Down = false;
        down_Down = false;
        left_Down = false;
        right_Down = false;
        up_Up = false;
        down_Up = false;
        left_Up = false;
        right_Up = false;
    }

    void FixedUpdate()
    {   // 이동
        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * Speed;

        // 스캔
        //Debug.DrawRay(rigid.position, dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D raycastHit2D = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (raycastHit2D.collider != null)
            scanObject = raycastHit2D.collider.gameObject;
        else
            scanObject = null;
    }

    public void ButtonDown(string type)
    {
        switch(type)
        {
            case "U":
                up_Value = 1;
                up_Down = true;
                break;
            case "D":
                down_Value = -1;
                down_Down = true;
                break;
            case "L":
                left_Value = -1;
                left_Down = true;
                break;
            case "R":
                right_Value = 1;
                right_Down = true;
                break;
            case "A":
                if (scanObject != null)
                    gameManager.Action(scanObject);
                break;
            case "C":
                gameManager.Cancel();
                break;
        }
    }

    public void ButtonUp(string type)
    {
        switch (type)
        {
            case "U":
                up_Value = 0;
                up_Up = true;
                break;
            case "D":
                down_Value = 0;
                down_Up = true;
                break;
            case "L":
                left_Value = 0;
                left_Up = true;
                break;
            case "R":
                right_Value = 0;
                right_Up = true;
                break;
        }
    }
}