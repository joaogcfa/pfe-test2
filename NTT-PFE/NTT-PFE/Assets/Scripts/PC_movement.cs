using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PC_movement : MonoBehaviour
{
    private Vector3 rawInputMovement;
    public Vector2 inputMovement;
    public Pc_player_input pc_input;

    void Start()
    {
        //pc_input = new Pc_player_input();
        rawInputMovement = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // inputx = Input.GetAxisRaw("Horizontal");
        // inputy = Input.GetAxisRaw("Vertical");

        // mov = new Vector3(inputx,0,inputy);

        // this.transform.position += mov * Time.deltaTime * 3f;

        this.transform.position += rawInputMovement * Time.deltaTime * 3f;
    }

    public void Move(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }
}
