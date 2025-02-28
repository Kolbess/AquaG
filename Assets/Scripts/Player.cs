using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    public Animator anim;
    private Vector2 lastMoveDirection;
    public Item currentItem;
    private List<Collider2D> triggeredItems = new List<Collider2D>();
    private float maxstamina = 100;
    private float currentstamina = 100;
    public AudioMangaer audiomanager;

    private void Start()
    {
        audiomanager = FindObjectOfType<AudioMangaer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            triggeredItems.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            if (currentItem == other.GetComponent<Item>())
            {
                currentItem.collecting = false;
                currentItem = null;
            }
            triggeredItems.Remove(other);
        }
    }

    void Update()
    {
        ProcessInputs();
        Animate();
        if (moveDirection.magnitude > 0.1f)
        {
            //moving
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                audiomanager.Play("walking-on-leaves-260279");
            } else if (SceneManager.GetActiveScene().name == "Level2")
            {
                audiomanager.Play("walking-in-water-199418");
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                audiomanager.Stop("walking-on-leaves-260279");
            } else if (SceneManager.GetActiveScene().name == "Level2")
            {
                audiomanager.Stop("walking-in-water-199418");
            }
        }
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && moveDirection.x != 0 || moveDirection.y != 0)
        {
            lastMoveDirection = moveDirection;
        }
        
        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKey(KeyCode.Space))
        {
            if (!currentItem && triggeredItems.Count > 0)
            {
                currentItem = triggeredItems[0].GetComponent<Item>();
            }
            if (currentItem)
            {
                currentItem.collecting = true;
            }
        } else if (currentItem)
        {
            currentItem.collecting = false;
            currentItem = null;
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void Animate()
    {
        anim.SetFloat("AnimMoveX", moveDirection.x);
        anim.SetFloat("AnimMoveY", moveDirection.y);
        anim.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);
        anim.SetFloat("AnimLastMoveX", lastMoveDirection.x);
        anim.SetFloat("AnimLastMoveY", lastMoveDirection.y);
    }
}
