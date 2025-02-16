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

        bool isPlayerShrouded = other.gameObject.GetComponent<CurrentPlayer>().isShrouded;

        if (other.CompareTag("Player") && !isPlayerShrouded)
        {
            var ob = GameObject.Find("Canvas").transform.Find("YouLoseText").gameObject;
            ob.SetActive(true);

            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
