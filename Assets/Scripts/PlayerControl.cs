using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Movement movement;
    PathFinding pathFinding;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        pathFinding = GetComponent<PathFinding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            movement.JumpEvent();
        }
        movement.hMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.P))
        {
            pathFinding.GetAllNearPlat();
        }

    }
}
