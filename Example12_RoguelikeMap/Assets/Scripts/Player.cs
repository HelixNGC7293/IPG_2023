using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField]
    GameObject game_Password;
    [SerializeField]
    TextMeshPro tX_PasswordContent;
    [SerializeField]
    Animator anim;

    string currentDoorPassword = "";
    int currentDoorWallID;
    Room currentRoom;

    Vector2 moveDirection;
    Rigidbody2D rB;

    Transform mainCamera;
    Vector3 cameraOffset;

	private void Start()
	{
        rB = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main.transform;
        cameraOffset = mainCamera.position - transform.position;
    }

	// Update is called once per frame
	void Update()
    {
        mainCamera.position = transform.position + cameraOffset;
        if (currentDoorPassword != "")
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyUp(vKey))
                {
                    if (moveDirection == Vector2.zero)
                    {
                        if (tX_PasswordContent.text == "_")
                        {
                            tX_PasswordContent.text = "";
                        }
                        tX_PasswordContent.text += vKey.ToString().ToUpper();
                        //print(tX_PasswordContent.text + "  " + currentDoorPassword);
                        if (tX_PasswordContent.text == currentDoorPassword)
                        {
                            //Password correct!
                            currentRoom.UnlockDoor(currentDoorWallID);
                            DisplayPasswordEnd();
                        }
                        else if (tX_PasswordContent.text.Length >= 5)
                        {
                            tX_PasswordContent.text = "_";
                        }
                    }
                }
            }
        }
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Item"))
		{
            Item item = other.transform.parent.GetComponent<Item>();
            item.Pickup();

        }
        else if (other.tag.Contains("Door"))
        {
            Room room = other.GetComponentInParent<Room>();
            game_Password.SetActive(true);
            game_Password.transform.position = transform.position;
            tX_PasswordContent.text = "_";

            if (other.tag.Contains("DoorL"))
            {
                currentDoorWallID = 0;
            }
            else if (other.tag.Contains("DoorR"))
            {
                currentDoorWallID = 1;
            }
            else if (other.tag.Contains("DoorU"))
            {
                currentDoorWallID = 2;
            }
            else if (other.tag.Contains("DoorD"))
            {
                currentDoorWallID = 3;
            }
            currentDoorPassword = room.GetPassword(currentDoorWallID);

            UIManager.instance.DisplayPassword(currentDoorPassword);
            currentRoom = room;
        }
        else if (other.CompareTag("Exit"))
		{
            UIManager.instance.Escape();
        }
    }

	private void OnTriggerExit2D(Collider2D other)
	{
        if (other.tag.Contains("Door"))
        {
            DisplayPasswordEnd();
        }
    }

    void DisplayPasswordEnd()
    {
        game_Password.SetActive(false);
        tX_PasswordContent.text = "_";
        currentDoorPassword = "";
        UIManager.instance.DisplayPasswordEnd();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();

        rB.velocity = moveDirection * 3.5f;

        if(moveDirection != Vector2.zero)
		{
            anim.SetBool("running", true);
            if(moveDirection.x < 0)
			{
                transform.localScale = new Vector2(-1, 1);
			}
            else if (moveDirection.x > 0)
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
        else
		{
            anim.SetBool("running", false);

        }
    }

}
