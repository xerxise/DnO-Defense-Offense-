using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class Item_items
{
    public string itemName;
    public int itemNumber;
    public int itemPower;
    public float itemHP;
    public float itemSpeed;
    public float itemTime;
}

[System.Serializable]
public class ItemDatabase
{
    public List<Item_items> itemList = new List<Item_items>(); 
}

public class XMLTest : MonoBehaviour
{
    public ItemDatabase itemDB;

    public void SaveItem()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Database/Item_Xml.xml", FileMode.Create);
        serializer.Serialize(stream, itemDB);
        stream.Close();
    }

    public void LoadItem()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Database/Item_Xml2.xml", FileMode.Open);
        itemDB = (ItemDatabase)serializer.Deserialize(stream);
    }

    private void Start()
    {
        //SaveItem();
        LoadItem();
    }
}
