using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public float speed;

    Animator anim;
    bool isWalking;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 targetVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            //transform.position = transform.position + Vector3.forward * Time.deltaTime * speed;
            targetVector += Vector3.forward;
        }
        if(Input.GetKey(KeyCode.D))
        {
            //transform.position = transform.position + Vector3.right * Time.deltaTime * speed;
            targetVector += Vector3.right;
        }
        if(Input.GetKey(KeyCode.S))
        {
            //transform.position = transform.position + Vector3.back * Time.deltaTime * speed;
            targetVector += Vector3.back;
        }
        if(Input.GetKey(KeyCode.A))
        {
            //transform.position = transform.position + Vector3.left * Time.deltaTime * speed;
            targetVector += Vector3.left;
        }

        if(targetVector != Vector3.zero)
        {
            transform.position = transform.position + targetVector * Time.deltaTime * speed;
            transform.LookAt(transform.position + targetVector);
            if (!isWalking)
            {
                isWalking = true;
                anim.SetBool("isWalking", true);
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                anim.SetBool("isWalking", false);
            }
            
        }

    }
}
