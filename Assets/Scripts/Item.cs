using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    private float timer;
    public float actionTime;
    private bool inTrigger;
    public bool collecting = false;
    public int itemValue;
    [SerializeField] private FollowObjectUI bar;
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        timer = 0f;
        bar.UpdateBar(actionTime, actionTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(timer);
        if (collecting)
        {
            timer += Time.deltaTime;
            Debug.Log("Time passed: " + timer);
            bar.UpdateBar(actionTime - timer, actionTime);
        } else if (timer > 0f)
        {
            timer = 0f;
            bar.UpdateBar(actionTime - timer, actionTime);
        }
            
        if (timer >= actionTime)
        {
            Debug.Log("Action");
            PerformAction();
            timer = 0f;
        }
    }

    private void OnDestroy()
    {
        inTrigger = false;
    }

    private void PerformAction()
    {
        //Add points
        ScoreManager.instance.AddPoint(itemValue);
        Destroy(this.gameObject);
    }
}
