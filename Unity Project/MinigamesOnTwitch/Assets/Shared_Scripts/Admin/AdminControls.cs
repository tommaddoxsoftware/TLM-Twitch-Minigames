using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class AdminControls : MonoBehaviour {
    public List<Admin> admins;

    private AdminContainer m_admins;

    private TwitchIRC m_irc;

	// Use this for initialization
	void Start () {
        m_irc = this.GetComponent<TwitchIRC>();

        Debug.Log("XML exists: " + File.Exists(Path.Combine(Application.dataPath, "admins.xml")));

        //If no XML file exsists, create one
        if (!File.Exists(Path.Combine(Application.dataPath, "admins.xml")))
        {
            //Save XML
            AdminContainer newXml = new AdminContainer();

            newXml.admins = new List<Admin>();

            newXml.Save(Path.Combine(Application.dataPath, "admins.xml"));
        }      

        //Load XML
        AdminContainer adminCollection = AdminContainer.Load(Path.Combine(Application.dataPath, "admins.xml"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Commands(string user, string[] msgArray)
    {
        Debug.Log(user + " is an admin");

        switch (msgArray[0].ToLower())
        {
            
        }
    }

    public bool CheckIfAdmin(string user)
    {
        //If the user is the channel owner they are an admin
        if (user == PlayerPrefs.GetString("TwitchUsr"))
        {
            return true;
        }

        //Checks if there are any admins
        if (admins != null)
        {
            //Check the list for admins
            for (int i = 0; i < admins.Count; i++)
            {
                if (user == admins[i].name)
                {
                    //Returns true if an admin is found
                    return true;
                }
            }
        }

        return false;
    }
}
