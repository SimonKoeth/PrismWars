using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public Text resourcecount, curSupply, MaxSupply;
    public GameObject Targetobj;
    Camera GameCamera;
    public Camera Rendercam;
    List<SelectController> Unitlist;
    GameGod GG;
    RaycastHit hit;
    public Button[] UIBuildingButtons = new Button[8];
    public Button[] UIUnitButtons = new Button[8];
    GameObject tempFocus;

    public GameObject UIName, UIDescription, UISlider, UIhp;
    Text TName, TDesc, THP;
    Slider HPPercent;

    public GameObject BuildingPanel, BuildBuildingPanel;

    public int Resource = 100;

    Vector3 Startpoint = new Vector3();
    Vector3 Endpoint = new Vector3();

    // Use this for initialization
    void Start () {

        GG = this.gameObject.GetComponent<GameGod>();
        Unitlist = new List<SelectController>();
        GameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Rendercam.enabled = false;

        TName = UIName.GetComponent<Text>();
        TDesc = UIDescription.GetComponent<Text>();
        THP = UIhp.GetComponent<Text>();

        HPPercent = UISlider.GetComponent<Slider>();
    }

    void MovementFunction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (hit.transform.gameObject.tag.Equals("GameGod"))
            {
                tempFocus = null;
                Targetobj.transform.position = new Vector3(hit.point.x, 0f, hit.point.z); //Set position of the new destination
            }
            else if (hit.transform.gameObject.tag.Equals("enemyBuilding") || hit.transform.gameObject.tag.Equals("enemy"))
            {
                //Focusfire auf gebäude
                tempFocus = hit.transform.gameObject;
            }

            foreach (SelectController unit in Unitlist)
            {
                
                //Iterate through all units and set their individual destination
                if (unit.isSelected)
                {
                    if (hit.transform.gameObject.tag.Equals("GameGod") && tempFocus == null)
                    {
                        unit.FocusEnemy = null;
                        unit.Targetdest = Targetobj.transform.position;
                        unit.followobject = null;
                    }
                    else if (tempFocus != null) {
                        unit.FocusEnemy = hit.transform.gameObject;
                    }

                    else {
                        //Follow whatever was clicked
                        unit.FocusEnemy = null;
                        unit.followobject = hit.transform.gameObject;
                    }
                }
            }
        }
    }

    void Singleselectfunction() {
        Ray ray = GameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (hit.transform.gameObject.tag.Equals("Unit"))
                {
                    GG.IsBuilding = false;
                    //Debug.Log("A unit was selected! " + hit.transform.gameObject);
                    // A unit is being selected
                    SelectController Sel;
                    Sel = hit.transform.gameObject.GetComponent<SelectController>();
                    Sel.isSelected = true;
                    //Debug.Log(Sel);
                    Unitlist.Add(Sel); //Add to selected units

                    foreach (SelectController unit in Unitlist)
                    {
                        //Unselect all units that weren't clicked
                        if (unit != hit.transform.gameObject.GetComponent<SelectController>())
                        {
                            Unitlist.Remove(unit);
                            unit.isSelected = false;
                        }
                    }
                }
                else if (hit.transform.gameObject.tag.Equals("GameGod"))
                {
                    //GG.IsBuilding = false;
                    //Debug.Log(hit.point);
                    Unitlist.Clear();
                    foreach (SelectController unit in Unitlist)
                    {
                        unit.isSelected = false;
                        //Unitlist.Remove(unit);
                    }

                }
                else if (hit.transform.gameObject.tag.Equals("Building")) {
                    BuildingScript tempBuilding = hit.transform.gameObject.GetComponent<BuildingScript>();

                    TName.text = tempBuilding.building.name;
                    TDesc.text = tempBuilding.building.description;
                    THP.text = tempBuilding.hp + " / " + tempBuilding.building.hp;

                    HPPercent.value = (tempBuilding.hp*100 / tempBuilding.building.hp*100) / 100;
                    Debug.Log((tempBuilding.hp*100 / tempBuilding.building.hp*100) / 100);

                    //If a building was selected
                    if (tempBuilding.buildTime >= tempBuilding.maxBuildTime)
                    {   
                        GG.IsBuilding = true;
                        GG.SelBuilding = hit.transform.gameObject;
                    }

                    //Enable the buttons of the units

                }
            }
        }
    }

    void Multiselectfunction()
    {
        Ray ray = GameCamera.ScreenPointToRay(Input.mousePosition);
        

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Startpoint = hit.point;
            }
            if (Input.GetButtonUp("Fire1")) {
                if (!hit.transform.gameObject.tag.Equals("Building"))
                {
                    //GG.IsBuilding = false;
                }
                Endpoint = hit.point;

                //Debug.Log(Startpoint);
                //Debug.Log(Endpoint);

                float Xdist, Zdist;

                Xdist = Mathf.Abs(Endpoint.x - Startpoint.x); //Maximum distance of X for each unit to have
                Zdist = Mathf.Abs(Endpoint.z - Startpoint.z); //Maximum distance of Z for each unit to have

                foreach (GameObject unit in GG.PlayerUnits) {
                    //Check distance for each object if it's within range of both

                    float UnitXdist, UnitZdist;

                    UnitXdist = Mathf.Abs(unit.transform.position.x - Startpoint.x);
                    UnitZdist = Mathf.Abs(unit.transform.position.z - Startpoint.z);

                    SelectController selector = unit.GetComponent<SelectController>();

                    if (UnitXdist < Xdist && UnitZdist < Zdist)
                    {
                        //Debug.Log("Position of unit selected: " + unit.transform.position);
                        GG.IsBuilding = false;
                        //Unit is in the plane of selection
                        Unitlist.Add(selector); //Add this unit to the selected list
                        selector.isSelected = true;
                    }
                    else {
                        if (selector.isSelected) {
                            //Remove it from the selection list if it's not within the range
                            selector.isSelected = false;
                            Unitlist.Remove(selector);
                        }
                    }
                }
            }


        }
    }

    void Rendercamenable()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Rendercam.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            Rendercam.enabled = false;
        }
    }

    public void UnselectAll() {
        Unitlist.Clear();
    }

    public void ButtonUIPush(int index) {
        Debug.Log("I am a buttonpush with index " + index);
        if (GG.IsBuilding) {
            //This only works if an actual building and not a unit is selected
            GG.SelBuilding.GetComponent<BuildingScript>().BuildUnit(index);
        }
    }

    void DisplayUnitButtons() {
        if (UIBuildingButtons[0].interactable && !GG.IsBuilding)
        {
            foreach (Button buildingButton in UIBuildingButtons)
            {
                buildingButton.interactable = false;
            }

            BuildingPanel.SetActive(false);
            BuildBuildingPanel.SetActive(true);
        }
        else if (!UIBuildingButtons[0].interactable && GG.IsBuilding) {
            int i = 0;
            BuildingPanel.SetActive(true);
            BuildBuildingPanel.SetActive(false);

            //Hiermit werden nun die buttons clickable gemacht und die namen gesetzt
            foreach (Unit buildunit in GG.SelBuilding.GetComponent<BuildingScript>().building.SpawnUnit) {
                UIBuildingButtons[i].interactable = true;
                UIBuildingButtons[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = buildunit.name;
                i++;
            }
        }
    }

    void controlCamera() {
        //First get the current resultion, then calculate the relative position of the mouse

        float Xpercent = Input.mousePosition.x / (float)Screen.currentResolution.width;
        float Ypercent = Input.mousePosition.y / (float)Screen.currentResolution.height;

        if (Xpercent > 0.95f) {
            GameCamera.transform.Translate(Vector3.right);
        }
        if (Ypercent > 0.93f) {
            GameCamera.transform.Translate(Vector3.up);
            GameCamera.transform.Translate(Vector3.forward * 0.5f);
        }

        if (Xpercent < 0.05f) {
            GameCamera.transform.Translate(Vector3.left);
        }

        if (Ypercent < 0.05f) {
            GameCamera.transform.Translate(Vector3.down);
            GameCamera.transform.Translate(Vector3.back * 0.5f);
        }
    }

    void updateSupplyCount() {
        GG.Supply = GG.PlayerUnits.Count;

        curSupply.text = "" + GG.Supply;
        MaxSupply.text = " / " + GG.MaxSupply + "Supply";
    }

	// Update is called once per frame
	void Update () {
        Singleselectfunction();
        Multiselectfunction();
        MovementFunction();
        DisplayUnitButtons();
        controlCamera();
        updateSupplyCount();
        Rendercamenable();

        resourcecount.text = "Resources: " + Resource;
    }
}
