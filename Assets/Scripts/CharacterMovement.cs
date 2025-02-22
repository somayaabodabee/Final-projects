using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterState stats;
    private CharacterController Controller;
    Animator anim;
    private Transform cam;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpValue = 3f;

    [Header("Gravity Settings")]
    public float gravity = 10f;

    private float verticalVelocity;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
        stats = GetComponent<CharacterState>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isSprint = Input.GetKey(KeyCode.LeftShift);
        float sprint = isSprint ? 2.7f : 1;
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");

        }

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);
        anim.SetFloat("Speed", Mathf.Clamp(moveDirection.magnitude, 0, 0.5f) + (isSprint ? 0.5f : 0));
        if (Controller.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                verticalVelocity = jumpValue;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if (moveDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        }

        moveDirection = cam.TransformDirection(moveDirection);
        moveDirection = new Vector3(moveDirection.x * speed * sprint, verticalVelocity, moveDirection.z * speed * sprint);

        Controller.Move(moveDirection * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Health"))
        {
            //Debug.Log("Health Increased!");
            GetComponent<CharacterState>().ChangeHealth(20);
            LevelManager.instance.PlaySound(LevelManager.instance.levelSounds[2], LevelManager.instance.Player.position);
            Instantiate(LevelManager.instance.particals[1], other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Item"))
        {
            LevelManager.instance.levelItem++;
            Debug.Log("Items : " + LevelManager.instance.levelItem);
            LevelManager.instance.PlaySound(LevelManager.instance.levelSounds[1], LevelManager.instance.Player.position);
            Instantiate(LevelManager.instance.particals[0], other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);

        }
    }
    public void DoAttack()
    {
        transform.Find("Collider").GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(HideCollider());

    }
    IEnumerator HideCollider()
    {
        yield return new WaitForSeconds(0.5f);
        transform.Find("Collider").GetComponent<BoxCollider>().enabled = false;
    }
}
