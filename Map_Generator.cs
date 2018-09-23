using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Map_Generator : MonoBehaviour {

    public int length;
    public int rotation;
    public GameObject turtle;
    public BoxCollider col;
    public Rigidbody rig;
    public GameObject prev;
    public GameObject prevInvert;
    public Vector3 ObjScale = new Vector3(1, 1, 1);
    public Vector3 rescale = new Vector3(1.1f, 1.1f, 1.1f);
    public string cube_tag = "wall";
    public int index = 1;
    public Material mat;
    public Material mat_des;

    GameGod GG;

	// Use this for initialization
	void Start () {
        GG = GameObject.Find("GameGod").GetComponent<GameGod>();
        turtle = this.gameObject;
        turtle.transform.tag = "turtle";
        Vector3 rot = new Vector3(0f, (float)rotation, 0f);
        turtle.transform.Rotate(rot, Space.Self);
        col = (BoxCollider)this.gameObject.AddComponent(typeof(BoxCollider));
        rig = this.gameObject.AddComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ;
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        turtle.transform.Translate(1 + 0.1f, 0, 0);
        //Debug.Log("split!");
    }
	
	// Update is called once per frame
	void Update () {
        move();
	}

    void move() {
        if (index>length) {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            if (index>4 && index<10)
            {
                obj.transform.tag = cube_tag;
                obj.GetComponent<MeshRenderer>().material = mat_des;
                obj.AddComponent<wallscript>();
                GG.Destructible.Add(obj);
            }
            else
            {
                obj.transform.tag = "cube";
                obj.GetComponent<MeshRenderer>().material = mat;
            }

            Destroy(obj.GetComponent<BoxCollider>());
            obj.transform.localScale = ObjScale;
            obj.AddComponent<Box_collision>();

            obj.transform.position = new Vector3(turtle.transform.position.x, 0.5f, turtle.transform.position.z);
            obj.transform.Rotate(turtle.transform.eulerAngles.x, turtle.transform.eulerAngles.y, turtle.transform.eulerAngles.z);

            obj.AddComponent <NavMeshObstacle>();
            prev.transform.localScale = rescale;
            prev = obj;

            GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

            if (index > 4 && index < 10)
            {
                obj2.transform.tag = cube_tag;
                obj2.GetComponent<MeshRenderer>().material = mat_des;
                obj2.AddComponent<wallscript>();
                GG.Destructible.Add(obj2);
            }
            else
            {
                obj2.transform.tag = "cube";
                obj2.GetComponent<MeshRenderer>().material = mat;

            }

            Destroy(obj2.GetComponent<BoxCollider>());
            obj2.transform.localScale = ObjScale;
            obj2.AddComponent<Box_collision>();

            obj2.transform.position = new Vector3(-turtle.transform.position.x, 0.5f, -turtle.transform.position.z);
            Vector3 rot = new Vector3(0f, (float)rotation, 0f);
            obj2.transform.Rotate(rot, Space.Self);

            NavMeshObstacle NMO = obj2.AddComponent<NavMeshObstacle>();
            NMO.carving = true;
            prevInvert.transform.localScale = rescale;
            prevInvert = obj2;


            int lengthNew = (int)Random.Range(15f, 20f);
            int rotationNew = (int)Random.Range(20f, 50f);
            GameObject branch1 = new GameObject();
            branch1.transform.Translate(turtle.transform.position.x, turtle.transform.position.y, turtle.transform.position.z);
            GameObject branch2 = new GameObject();
            branch2.transform.Translate(turtle.transform.position.x, turtle.transform.position.y+rotationNew, turtle.transform.position.z);

            Map_Generator b1 = branch1.AddComponent<Map_Generator>();
            Map_Generator b2 = branch2.AddComponent<Map_Generator>();
            b1.length = lengthNew;
            b2.length = lengthNew;
            b1.rotation =rotation+rotationNew;
            b2.rotation =rotation -rotationNew;
            b1.mat = mat;
            b2.mat = mat;
            b1.mat_des = mat_des;
            b2.mat_des = mat_des;
            b1.prev = obj;
            b1.prevInvert = obj2;
            b2.prev = obj;
            b2.prevInvert = obj2;
            b1.rescale = rescale;
            b2.rescale = rescale;
            Destroy(this.gameObject);

        }

        else
        {
            float step = 1;
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            if (index > 4 && index < 10)
            {
                obj.transform.tag = cube_tag;
                obj.GetComponent<MeshRenderer>().material = mat_des;
                obj.AddComponent<wallscript>();
                GG.Destructible.Add(obj);
            }
            else
            {
                obj.transform.tag = "cube";
                obj.GetComponent<MeshRenderer>().material = mat;
            }

            Destroy(obj.GetComponent<BoxCollider>());
            obj.transform.localScale = ObjScale;
            obj.AddComponent<Box_collision>();
            NavMeshObstacle NMO = obj.AddComponent<NavMeshObstacle>();
            NMO.carving = true;

            obj.transform.position = new Vector3(turtle.transform.position.x,0.5f, turtle.transform.position.z);
            obj.transform.Rotate(turtle.transform.eulerAngles.x, turtle.transform.eulerAngles.y, turtle.transform.eulerAngles.z);
            prev.transform.localScale = rescale;
            prev = obj;

            GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

            if (index > 4 && index < 10)
            {
                obj2.transform.tag = cube_tag;
                obj2.GetComponent<MeshRenderer>().material = mat_des;
                obj2.AddComponent<wallscript>();
                GG.Destructible.Add(obj2);
            }
            else
            {
                obj2.transform.tag = "cube";
                obj2.GetComponent<MeshRenderer>().material = mat;

            }

            Destroy(obj2.GetComponent<BoxCollider>());
            obj2.transform.localScale = ObjScale;
            obj2.AddComponent<Box_collision>();
            NavMeshObstacle NMO2 = obj2.AddComponent<NavMeshObstacle>();
            NMO2.carving = true;

            obj2.transform.position = new Vector3(-turtle.transform.position.x, 0.5f, -turtle.transform.position.z);
            Vector3 rot = new Vector3(0f, (float)rotation, 0f);
            obj2.transform.Rotate(rot,Space.Self);
            prevInvert.transform.localScale = rescale;
            prevInvert = obj2;

            turtle.transform.Translate(step+0.05f, 0, 0);
            turtle.transform.position = new Vector3(turtle.transform.position.x, 0.6f, turtle.transform.position.z);
            prev.AddComponent<BoxCollider>();
            prevInvert.AddComponent<BoxCollider>();
            index++;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.tag.Equals("turtle"))
        { return; }
        else
        {
            //Debug.Log("Destroy");
            prev.transform.localScale = rescale;
            prevInvert.transform.localScale = rescale;
            Destroy(this.gameObject);
        }
    }


}
