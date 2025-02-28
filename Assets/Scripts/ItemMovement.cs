using System.Collections.Generic;
using UnityEngine;

public class ItemMovement : MonoBehaviour
{
    public List<Vector2> movePoints; 
    public List<List<GameObject>> ItemPaths;
    public List<GameObject> path1;
    public List<GameObject> path2;
    public List<GameObject> path3;
    private List<GameObject> objectPoints;
    
    public Vector2 targetPosition;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemPaths = new List<List<GameObject>>() {path1, path2, path3};
        objectPoints = ItemPaths[Random.Range(0, 3)];
        for (int i = 0; i < objectPoints.Count; i++)
        {
            movePoints.Add(new Vector2(objectPoints[i].transform.position.x, objectPoints[i].transform.position.y));
        } 
        targetPosition = movePoints[0];
    }

    private void GetTargetPosition()
    {
        for (int i = 0; i < movePoints.Count; i++)
        {
            if (movePoints[i] == targetPosition)
            {
                if (i == movePoints.Count - 1)
                {
                    Destroy(this.gameObject);
                    break;
                }
                targetPosition = movePoints[i + 1];
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if ((Vector2)transform.position == targetPosition)
        {
            GetTargetPosition();
        }
    }
}
