using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringSplitter {

    public string[] Splitter(string str, char split)
    {
        string[] strArr = null;
        strArr = str.Split(split);

        return strArr;
    }
}
