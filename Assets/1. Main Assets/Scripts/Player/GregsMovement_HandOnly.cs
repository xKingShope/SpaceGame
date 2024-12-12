using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GregsMovement_HandOnly : MonoBehaviour
{
    Rigidbody rb;

    public int mag = 10;
    public float wait = 1000;
    private bool grab = false;

    public Camera camera;
    private Vector3 p2cam;
    private Vector3 m2cam;
    private Vector3 aim;

    public GameObject rightHand;
    public GameObject leftHandTarget;
    public GameObject armature;
    public GameObject rightHandTarget;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        leftHandTarget.transform.position = rightHand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //0-1 player vector in camera view
        p2cam = camera.WorldToViewportPoint(rb.position);
        //0-1 mouse vector in camera view
        m2cam = camera.ScreenToViewportPoint(Input.mousePosition);
        //calc angle of aim

        aim = m2cam - p2cam;

        //player's z had value so z value has to be adjusted
        aim.z = 0;

        aim.Normalize();

        wait = Mathf.Clamp(++wait, 0, 101);
        if (wait >= 100 && Input.GetMouseButton(0))
        {

            wait = 0;


            //player movement


            //gives aim a magnitude and reverses it
            Vector3 move = aim * mag * -1;

            //add force
            rb.AddRelativeForce((move));
        }

        //rotation
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(0, 0, .003f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(0, 0, -.003f);
        }


        //jumping
        if (Input.GetKeyUp(KeyCode.Space) && grab)
        {
            Vector3 jump = aim * mag;
            if (Physics.Raycast(transform.position, aim, 4f))
            {
                rb.AddRelativeForce(jump * -1);
                Debug.Log("in");
            }
            else
            {
                rb.AddRelativeForce(jump);
                Debug.Log("out");
            }
        }
        if (grab && !Input.GetMouseButton(1))
        {
            Destroy(gameObject.GetComponent<HingeJoint>());
            Destroy(leftHandTarget.GetComponent<HingeJoint>());
            grab = false;
        }

        //grabbing
        Vector3 newPoint;
        if (!grab)
        {
            newPoint.x = Mathf.MoveTowards(leftHandTarget.transform.position.x, rightHand.transform.position.x, 1f);
            newPoint.y = Mathf.MoveTowards(leftHandTarget.transform.position.y, rightHand.transform.position.y, 1f);
            newPoint.z = Mathf.MoveTowards(leftHandTarget.transform.position.z, rightHand.transform.position.z, 1f);
            leftHandTarget.transform.position = newPoint;
        }


        rightHandTarget.transform.position = rightHand.transform.position+aim;
        //mouse follow
        //as of now causes crazyness
        armature.transform.eulerAngles = new Vector3(0, 180 * (1-m2cam.x) + 270, armature.transform.eulerAngles.z);

    }
    //grabbing
    private void OnCollisionStay(Collision collision)
    {

        if (!grab && Input.GetMouseButton(1) && collision.rigidbody != null)
        {
            gameObject.AddComponent<HingeJoint>();
            gameObject.GetComponent<HingeJoint>().connectedBody = collision.rigidbody;
            leftHandTarget.transform.position = collision.GetContact(0).point;
            leftHandTarget.AddComponent<HingeJoint>();
            leftHandTarget.GetComponent<HingeJoint>().connectedBody = collision.rigidbody;
            grab = true;
        }
        
    }


    //things to fix
    //possinly get a better solution for cooldown
    //i hear states are pretty neat
    //improve grab a bit
    //that turning thing is wonky
    //i need another camera because that jumping bit gets a little wonky
    //speaking of jumping i need to add charging and recoil
}




//stuff that might be useful:

//camera.WorldToViewportPoint(body.position) position on screen 0-1
//camera.ScreenToViewportPoint(Input.mousePosition) is where mouse is on screen 0-1
//camera.WorldToScreenPoint(body.position) is position on the screen matching Input.mousePosition
