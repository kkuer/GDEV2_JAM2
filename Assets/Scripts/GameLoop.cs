using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public List<GameObject> rooms;
    public List<GameObject> activeRooms;

    public GameObject currentRoom;

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
            GameObject newRoom = Instantiate(rooms[Random.Range(0, rooms.Count)], new Vector3(0, 0, currentRoom.transform.localPosition.z + 20), Quaternion.identity);
            activeRooms.Add(newRoom);
        }
    }
}
