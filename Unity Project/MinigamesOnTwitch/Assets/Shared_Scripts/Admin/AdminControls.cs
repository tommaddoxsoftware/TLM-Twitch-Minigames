using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class AdminControls : MonoBehaviour {
    public List<Admin> admins;

    private AdminContainer m_admins;

	// Use this for initialization
	void Start () {
        admins = new List<Admin>();

        Admin adminTest = new Admin();
        adminTest.name = "electric_pie";

        admins.Add(adminTest);

        //Save XML
        AdminContainer test = new AdminContainer();

        test.admins = admins;

        test.Save(Path.Combine(Application.dataPath, "admins.xml"));

        //Load XML
        AdminContainer adminCollection = AdminContainer.Load(Path.Combine(Application.dataPath, "admins.xml"));

        Debug.Log("Admin: " + adminCollection.admins[0].name);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
