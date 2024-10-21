using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<GameObject> activeRooms;

    public List<GameObject> walls;

    public List<GameObject> obstacles;
    public List<GameObject> paintings;

    public GameObject currentRoom;

    public Vector3 treePosition;
    public Vector3 printerPosition;
    public Vector3 boostPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeRooms.Count == 6)
        {
            activeRooms.Remove(activeRooms[0]);
            Destroy(activeRooms[0]);
        }
    }

    public void setRoom(GameObject room)
    {
        currentRoom = room;
        createRoom();
    }

    public void createRoom()
    {
        if (currentRoom)
        {
            GameObject newRoom = Instantiate(rooms[Random.Range(0, rooms.Count)], 
                new Vector3(0, 0, currentRoom.transform.localPosition.z + 40), 
                Quaternion.identity);

            if (newRoom.transform.Find("Ramp"))
            {
                GameObject newPainting = Instantiate(paintings[Random.Range(0, paintings.Count)], 
                    newRoom.transform.Find("Ramp").Find("ObjectPosition").transform.position, 
                    Quaternion.Euler(0, 180, 0));
            }

            GameObject newOsbtacle = Instantiate(obstacles[Random.Range(0, obstacles.Count)], 
                newRoom.transform.Find("ObjectPosition").transform.position, 
                Quaternion.Euler(0, 180, 0));

            GameObject newWall_1 = Instantiate(walls[Random.Range(0, walls.Count)], 
                newRoom.transform.Find("WallSpawns").Find("WallSpawn1").transform.position, 
                Quaternion.Euler(0, 90, 0));

            GameObject newWall_2 = Instantiate(walls[Random.Range(0, walls.Count)], 
                newRoom.transform.Find("WallSpawns").Find("WallSpawn2").transform.position, 
                Quaternion.Euler(0, 90, 0));

            if (newOsbtacle.gameObject.name == "TREE_Solid(Clone)")
            {
                newOsbtacle.transform.position += treePosition;
            }
            else if (newOsbtacle.gameObject.name == "PRINTER_SOLID(Clone)")
            {
                newOsbtacle.transform.position += printerPosition;
            }
            else if (newOsbtacle.gameObject.name == "BoostItem(Clone)")
            {
                newOsbtacle.transform.position += boostPosition;
            }

            activeRooms.Add(newRoom);
        }
    }
}
