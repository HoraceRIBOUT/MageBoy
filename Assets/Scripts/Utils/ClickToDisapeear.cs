﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ClickToDisapeear : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool active = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            active = !active;
            this.GetComponent<SpriteRenderer>().enabled = active;
        }

        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.x, this.transform.localScale.x);
    }
}
