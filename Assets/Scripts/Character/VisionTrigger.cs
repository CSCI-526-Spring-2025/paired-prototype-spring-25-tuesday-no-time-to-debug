using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public class VisionTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CurrentPlayer player = other.GetComponent<CurrentPlayer>();
            if (!player.isShrouded)
            {
                var ob = GameObject.Find("Canvas").transform.Find("YouLoseText").gameObject;
                ob.SetActive(true);

                Time.timeScale = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}