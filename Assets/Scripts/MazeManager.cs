using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject corner;
    public GameObject hallway;
    public GameObject inter;
    public GameObject keycard;
    public GameObject player;
    public GameObject border;
    public GameObject escape;
    public GameObject top, fps;
    Dictionary<int, GameObject> tiles = new Dictionary<int, GameObject>();


    public int size;
    void Start()
    {
        size = 10;
        tiles.Add(0, corner);
        tiles.Add(1, hallway);
        tiles.Add(2, inter);
        Instantiate(border, transform);
        top = GameObject.Find("TOPCamera");

        TopCam();
    }
    bool RecheckTile(int posx, int posz)
    {
        int one = 0;
        int two = 0;
        int three = 0;
        int four = 0;
        List<RaycastHit> hits = new List<RaycastHit>();
        if (Physics.Raycast(new Vector3(posx, 1, posz), Vector3.left, out RaycastHit hit1, 5.25f))
        {
            hits.Add(hit1);
            one = 1;
        }

        if (Physics.Raycast(new Vector3(posx, 1, posz), Vector3.back, out RaycastHit hit2, 5.25f))
        {
            hits.Add(hit2);
            two = 1;
        }


        if (Physics.Raycast(new Vector3(posx, 1, posz), Vector3.right, out RaycastHit hit3, 5.25f))
        {
            hits.Add(hit3);
            three = 1;
        }

        if (Physics.Raycast(new Vector3(posx, 1, posz), Vector3.forward, out RaycastHit hit4, 5.25f))
        {
            hits.Add(hit4);
            four = 1;
        }
        int count = one + two + three + four;
        if (count > 2)
        {
            Destroy(hits[0].transform.gameObject);
            if (count >= 4)
            {
                Destroy(hits[1].transform.gameObject);
            }
            print("destroying");
            return true;
        }

        return true;
    }
    bool CheckTile(int posx, int posz)
    {
        int one = 0;
        int two = 0;
        int three = 0;
        int four = 0;
        List<RaycastHit> hits = new List<RaycastHit>();
        if (Physics.Raycast(new Vector3(posx, 1, posz), Vector3.left, out RaycastHit hit1, 5.25f))
        {
            if (hit1.transform.tag == "Maze")
            {
                hits.Add(hit1);
                one = 1;
            }
        }

        if (Physics.Raycast(new Vector3(posx, 1, posz), Vector3.back, out RaycastHit hit2, 5.25f))
        {
            if (hit2.transform.tag == "Maze")
            {
                hits.Add(hit2);
                two = 1;
            }
        }


        if (Physics.Raycast(new Vector3(posx, 1, posz), Vector3.right, out RaycastHit hit3, 5.25f))
        {
            if (hit3.transform.tag == "Maze")
            {
                hits.Add(hit3);
                three = 1;
            }
        }

        if (Physics.Raycast(new Vector3(posx, 1, posz), Vector3.forward, out RaycastHit hit4, 5.25f))
        {
            if (hit4.transform.tag == "Maze")
            {
                hits.Add(hit4);
                four = 1;
            }
        }
        int count = one + two + three + four;
        if (count > 2)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    void Reset()
    {
        foreach (GameObject g in GetComponentInChildren<Transform>())
        {
            Destroy(g.gameObject);
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int tile = Random.Range(0, 3);
                print(tile);
                GameObject temp = Instantiate(tiles[tile], new Vector3(i * 10, 0, j * 10), Quaternion.identity, transform);
                while (!CheckTile(i, j))
                {
                    Destroy(temp);
                    temp = Instantiate(tiles[tile], new Vector3(i * 10, 0, j * 10), Quaternion.identity, transform);
                }
            }
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {

                //Physics.Raycast(new Vector3(i * 10, 0, j * 10), Vector3.down, out RaycastHit hit);
                // GameObject temp = hit.transform.gameObject;
                print("testing");
                while (!RecheckTile(i * 10, j * 10))
                {
                    print("Checking");
                    //Destroy(temp);
                    //temp = Instantiate(tiles[tile], new Vector3(i * 10, 0, j * 10), Quaternion.identity, transform);
                }
            }
        }
        int x = Random.Range(0, size);
        int y = Random.Range(0, size);
        while (!CheckTile(x, y))
        {
            x = Random.Range(0, size);
            y = Random.Range(0, size);
        }
        Instantiate(keycard, new Vector3(x * 10, 0.5f, y * 10), Quaternion.identity, transform);
    }
    int i = 0, j = 0, k = 0, l = 0;
    bool pass1 = false, pass2 = false;
    // Update is called once per frame
    float timer = 0;
    bool playerCheck = false;
    bool card = false;
    bool escapeCheck = false;
    bool start = false;
    void ResetUpdate()
    {
        print("Break");
        i = 0; j = 0; k = 0; l = 0;
        pass1 = false; pass2 = false;
        timer = 0;
        playerCheck = false;
        card = false;
        escapeCheck = false;
        start = false;
        foreach (Transform g in GetComponentInChildren<Transform>())
        {

            Destroy(g.gameObject);
        }

        Instantiate(border, transform);

    }
    void TopCam()
    {
        top.SetActive(true);
        if (fps != null)
            fps.SetActive(false);
    }
    void FpsCam()
    {
        top.SetActive(false);
        fps.SetActive(true);
    }
    //float startTimer
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }




        if (timer >= 4.0f && !pass2)
        {
            print("timeout");
            ResetUpdate();

        }

        //Physics.Raycast(new Vector3(i * 10, 0, j * 10), Vector3.down, out RaycastHit hit);
        // GameObject temp = hit.transform.gameObject;
        timer += Time.deltaTime;
        if (!pass2)
        {

            if (i < size)
            {
                int tile = Random.Range(0, 3);
                //  print(tile);
                GameObject temp = Instantiate(tiles[tile], new Vector3(i * 10, 0, j * 10), Quaternion.identity, transform);
                if (!CheckTile(i, j))
                {
                    Destroy(temp);
                    return;
                    //  temp = Instantiate(tiles[tile], new Vector3(i * 10, 0, j * 10), Quaternion.identity, transform);
                }
                print("i: " + i + " j: " + j);
                if (i >= 6 && j >= 0)
                {
                    print("i59");
                }
                if (j >= 9)
                {
                    i++;
                    j = 0;
                }
                else
                    j++;


            }
            else
                pass1 = true;
            if (pass1 && !pass2)
            {
                //   print("testing");
                while (!RecheckTile(k * 10, l * 10))
                {
                    return;
                    //    print("Checking");
                    //Destroy(temp);
                    //temp = Instantiate(tiles[tile], new Vector3(i * 10, 0, j * 10), Quaternion.identity, transform);
                }

                if (k < size)
                {
                    print("k: " + k + " l: " + l);
                    if (l >= 10)
                    {
                        k++;
                        l = 0;
                    }
                    else
                        l++;

                }
                else
                {
                    pass2 = true;
                }
            }
        }
        else if (!playerCheck)
        {

            int x = Random.Range(0, size);
            int y = Random.Range(0, size);
            //while (!CheckTile(x, y))
            //{
            //    x = Random.Range(0, size);
            //    y = Random.Range(0, size);
            //}
            Instantiate(player, new Vector3(x * 10, 1.5f, y * 10), Quaternion.identity, transform);
            fps = GameObject.Find("Main Camera");
            playerCheck = true;
        }
        else if (!card)
        {
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);
            //while (!CheckTile(x, y))
            //{
            //    x = Random.Range(0, size);
            //    y = Random.Range(0, size);
            //}
            Instantiate(keycard, new Vector3(x * 10, 1.5f, y * 10), Quaternion.identity, transform);
            card = true;
        }
        else if (!escapeCheck)
        {
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);
            //while (!CheckTile(x, y))
            //{
            //    x = Random.Range(0, size);
            //    y = Random.Range(0, size);
            //}
            Instantiate(escape, new Vector3(x * 10, 1.5f, y * 10), Quaternion.identity, transform);
            escapeCheck = true;
        }

        if (timer > 7)
        {
            if (Input.GetMouseButton(0))
            {
                TopCam();
            }
            else
                FpsCam();
        };
    }
}
