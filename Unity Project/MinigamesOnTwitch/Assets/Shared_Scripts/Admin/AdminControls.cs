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
    void Start()
    {
        m_irc = this.GetComponent<TwitchIRC>();

        Debug.Log("XML exists: " + File.Exists(Path.Combine(Application.dataPath, "admins.xml")));

        //If no XML file exsists, create one
        if (!File.Exists(Path.Combine(Application.dataPath, "admins.xml")))
        {
            //Save XML
            AdminContainer newXml = new AdminContainer();

            newXml.admins = new List<Admin>();

            newXml.Save(Path.Combine(Application.dataPath, "admins.xml"));

            Debug.Log("Created new admin file");
        }

        //Load XML
        m_admins = AdminContainer.Load(Path.Combine(Application.dataPath, "admins.xml"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Commands(string user, string[] msgArray)
    {
        Debug.Log(user + " is an admin");

        switch (msgArray[0].ToLower())
        {
            case "!addadmin":
                AddAdmin(msgArray[1].ToLower());
                break;
            case "!removeadmin":
                RemoveAdmin(msgArray[1].ToLower());
                break;
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
        if (m_admins.admins != null)
        {
            //Check the list for admins
            for (int i = 0; i < m_admins.admins.Count; i++)
            {
                if (user == m_admins.admins[i].name)
                {
                    //Returns true if an admin is found
                    return true;
                }
            }
        }

        return false;
    }

    private int GetAdminIndex(string user)
    {
        //If the user is the channel owner they are an admin
        if (user == PlayerPrefs.GetString("TwitchUsr"))
        {
            return -2;
        }

        //Checks if there are any admins
        if (m_admins.admins != null)
        {
            //Check the list for admins
            for (int i = 0; i < m_admins.admins.Count; i++)
            {
                if (user == m_admins.admins[i].name)
                {
                    //Returns true if an admin is found
                    return i;
                }
            }
        }

        return -1;
    }

    private void AddAdmin(string user)
    {
        Admin newAdmin = new Admin();
        newAdmin.name = user.ToLower();

        m_admins.admins.Add(newAdmin);

        UpdateFile();

        m_irc.SendMsg(user + " is now an admin");
    }

    private void RemoveAdmin(string user)
    {
        //Gets the index value of the user
        int index = GetAdminIndex(user);

        //Is an admin
        if (index >= 0)
        {
            m_admins.admins.RemoveAt(index);

            UpdateFile();

            m_irc.SendMsg(user + " is no longer an admin");
        }
        //Is not an admin
        else if (index == -1)
        {
            m_irc.SendMsg(user + " is not an admin");
        }
        //Is the channel owner
        else if (index == -2)
        {
            m_irc.SendMsg(user + " is the channel owner, cannot remove");
        }
    }

    private void UpdateFile()
    {
        m_admins.Save(Path.Combine(Application.dataPath, "admins.xml"));
    }
}
