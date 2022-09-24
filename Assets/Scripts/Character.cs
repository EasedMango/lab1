using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
//[RequireComponent(typeof(Camera))]
public class Character : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 1;
    float vert, hor;
    bool jump;
    public LayerMask mask;
    Vector3 origin;
    bool keycard=false;
    // Start is called before the first frame update
    void Start()
    {
        jump = false;
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Keycard"))
        {
            keycard = true;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Finish") && keycard)
        {
            print("quit");
            Application.Quit();
        }
    }
    // Update is called once per frame
    void Update()
    {
        origin = (transform.position - (transform.up * 0.5f));
        vert = Input.GetAxisRaw("Vertical");
        hor = Input.GetAxisRaw("Horizontal");
        jump = Input.GetKey(KeyCode.Space);

        

    }
    void Movement()
    {
        RaycastHit hit;
        Physics.Raycast(origin, Vector3.down, out hit, 100, mask);


        Vector3 lookDir = transform.forward;

        // transform.up = hit.normal;

        Vector3 up = hit.normal;
        Vector3 rght = Vector3.Cross(up, lookDir).normalized;
        Vector3 frwd = Vector3.Cross(rght, up);
        //frwd += up * 0.25f;


        Vector3 fNet = Vector3.zero;
        // print(vert);
        //  print(hor);
        if (vert > 0)
        {

            fNet += (new Vector3(frwd.x, 0, frwd.z).normalized);
        }
        if (vert < 0)
        {

            fNet -= (new Vector3(frwd.x, 0, frwd.z).normalized);
        }
        if (hor > 0)
        {

            fNet += (new Vector3(rght.x, 0, rght.z).normalized);
            print(fNet);
        }
        if (hor < 0)
        {

            fNet -= (new Vector3(rght.x, 0, rght.z).normalized);
        }

        Vector3 jmp = Vector3.zero;
        // Debug.DrawLine(transform.position, transform.position+ Vector3.down,Color.red);
        if (jump && hit.distance < 1.6f)
        {
            print("jump");
            jmp += Vector3.ClampMagnitude(Vector3.up * 10, 10);
            jump = false;
        }

        if (rb.velocity.magnitude < 5)
        {
            rb.velocity += Vector3.ClampMagnitude(fNet.normalized * speed, 10) + jmp;
        }
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void OnDrawGizmos()
    {

        //mesh.vertices = corners;
        //  DrawMesh();
        Vector3 headPos = transform.position;
        Vector3 lookDir = transform.forward;




        void DrawRayNormal(Vector3 pos, Vector3 dir) => Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 8f, pos, pos + dir);

        void DrawRay(Vector3 pos, Vector3 dir) => Handles.DrawAAPolyLine(EditorGUIUtility.whiteTexture, 8f, pos, dir);
        DrawRayNormal(origin, Vector3.down);
        if (Physics.Raycast(headPos - (transform.up * 0.25f), Vector3.down, out RaycastHit hit, mask))
        {


            Vector3 hitPos = hit.point;
            Vector3 up = hit.normal;
            Vector3 right = Vector3.Cross(up, lookDir).normalized;
            Vector3 forward = Vector3.Cross(right, up);
            //  forward += up * 0.25f;

            Handles.color = Color.green;
            DrawRayNormal(transform.position, up);
            Handles.color = Color.red;
            DrawRayNormal(transform.position, right);
            Handles.color = Color.cyan;
            DrawRayNormal(transform.position, forward);


        }
        else
        {
            Handles.color = Color.red;
            DrawRayNormal(headPos, lookDir);
        }

    }
}
