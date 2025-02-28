using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    public List<GameObject> npcItemChoice;
    private GameObject npcItem;
    private List<Vector2> movePoints = new List<Vector2>();
    private List<GameObject> waypoints;
    private Vector2 targetPosition;
    private GameObject ItemDropPosition;
    public List<GameObject> ItemDropPositions;
    public float speed;
    public List<List<GameObject>> waypointsList;
    public List<GameObject> path1;
    public List<GameObject> path2;
    public List<GameObject> path3;
    public List<GameObject> path4;
    private Collider2D _collider2D;
    
    private bool stop;
    private bool drop;
    private bool itemdropped;
    private float timer;
    private bool collidedWithPlayer = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        npcItem = npcItemChoice[Random.Range(0, 2)];
        waypointsList = new List<List<GameObject>>() { path1, path2, path3, path4 };
        int random = Random.Range(0, 4);
        waypoints = waypointsList[random];
        ItemDropPosition = ItemDropPositions[random];
        for (int i = 0; i < waypoints.Count; i++)
        {
            movePoints.Add(new Vector2(waypoints[i].transform.position.x, waypoints[i].transform.position.y));
        }

        Vector2 backpos = movePoints[0];
        movePoints.Add(backpos);
        transform.position = backpos;
        targetPosition = backpos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetPosition = movePoints[0];
            collidedWithPlayer = true;
        }
    }

    private void DropItem()
    {
        GameObject item = Instantiate(npcItem);
        item.transform.position = ItemDropPosition.transform.position;
        itemdropped = true;
        ScoreManager.instance.RemovePoint(item.GetComponent<Item>().itemValue);
    }
    
    private void GetTargetPosition()
    {
        for (int i = 0; i < movePoints.Count; i++)
        {
            if (movePoints[i] == targetPosition)
            {
                if (collidedWithPlayer && !itemdropped)
                {
                    ScoreManager.instance.AddPoint(5);
                    Destroy(this.gameObject);
                    break;
                }
                if (!itemdropped)
                {
                    if (i == 1)
                    {
                        stop = true;
                    }

                    if (i == 2)
                    {
                        drop = true;
                    }

                    targetPosition = movePoints[i + 1];
                    break;
                }
                Destroy(this.gameObject);
                break;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop && !drop)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if ((Vector2)transform.position == targetPosition)
            {
                GetTargetPosition();
            }
        }
        else if (stop)
        {
            timer += Time.deltaTime;
            if (timer % 60 >= 5f)
            {
                stop = false;
                timer = 0f;
            }
        } else if (drop)
        {
            timer += Time.deltaTime;
            if (timer % 60 >= 1f)
            {
                DropItem();
                drop = false;
                timer = 0f;
            }
        }
    }
}
