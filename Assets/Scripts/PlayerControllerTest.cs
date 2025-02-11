using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldMemory;

public class PlayerControllerTest : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool[] previousInputs = { false, false, false }; // Left, Right, Up;

    public float speed = 5f;
    public Vector2 jumpForce = new Vector2(0, 5f);
    public GameManager GameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private bool ShouldLogMemory()
    {
        bool halfSecondPassed = Math.Floor(2 * GameManager.CurrentTime) != Math.Floor(2 * (GameManager.CurrentTime - Time.deltaTime));

        bool inputChanged = Input.GetKey(KeyCode.LeftArrow) != previousInputs[0] ||
                            Input.GetKey(KeyCode.RightArrow) != previousInputs[1] ||
                            Input.GetKey(KeyCode.UpArrow) != previousInputs[2];

        previousInputs[0] = Input.GetKey(KeyCode.LeftArrow);
        previousInputs[1] = Input.GetKey(KeyCode.RightArrow);
        previousInputs[2] = Input.GetKey(KeyCode.UpArrow);

        return halfSecondPassed || inputChanged;
    }

    private void performKeyboardBasedActions()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(horizontal * speed * Time.deltaTime, 0, 0);

        if (isGrounded && vertical != 0)
        {
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (ShouldLogMemory())
        {

            List<KeyCode> pressedKeys = new List<KeyCode>();

            if (Input.GetKey(KeyCode.LeftArrow))
                pressedKeys.Add(KeyCode.LeftArrow);
            if (Input.GetKey(KeyCode.RightArrow))
                pressedKeys.Add(KeyCode.RightArrow);
            if (Input.GetKey(KeyCode.UpArrow))
                pressedKeys.Add(KeyCode.UpArrow);

            GameManager.WorldMemory.AddLog(new PlayerMemoryWithKeyLog()
            {
                TimeStamp = GameManager.CurrentTime,
                OwnerName = gameObject.name,
                Position = transform.position,
                KeysPressed = pressedKeys.ToArray(),
                IsAlive = true
            });
        }
    }

    private bool isVisible()
    {
        return gameObject.GetComponent<SpriteRenderer>().enabled;
    }

    private void disappear()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    private void appear()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    private void performMemoryBasedActions()
    {
        PlayerMemoryWithKeyLog memoryLog = GameManager.GetNextMemoryLogFor(gameObject.name) as PlayerMemoryWithKeyLog;

        if (memoryLog == null || memoryLog.IsAlive == false)
        {
            if (isVisible())
            {
                disappear();
            }
            return;
        }
        else
        {
            if (!isVisible())
            {
                appear();
                transform.position = new Vector3(memoryLog.Position[0], memoryLog.Position[1], 0);
            }

            KeyCode[] keysPressed = memoryLog.KeysPressed;

            bool isLeftPressed = previousInputs[0];
            bool isRightPressed = previousInputs[1];
            bool isUpPressed = previousInputs[2];

            if (memoryLog.TimeStamp < GameManager.CurrentTime)
            {
                if (keysPressed != null)
                {
                    previousInputs[0] = isLeftPressed = keysPressed.Contains(KeyCode.LeftArrow) ? true : false;
                    previousInputs[1] = isRightPressed = keysPressed.Contains(KeyCode.RightArrow) ? true : false;
                    previousInputs[2] = isUpPressed = keysPressed.Contains(KeyCode.UpArrow) ? true : false;
                }
            }


            if (isLeftPressed)
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            else if (isRightPressed)
                transform.Translate(speed * Time.deltaTime, 0, 0);
            else if (isUpPressed && isGrounded)
            {
                rb.AddForce(jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "Player")
            performKeyboardBasedActions();
        else
        {
            performMemoryBasedActions();
        }
    }

    public void OnEnterPortal()
    {
        Debug.Log("Enter");
    }
}
