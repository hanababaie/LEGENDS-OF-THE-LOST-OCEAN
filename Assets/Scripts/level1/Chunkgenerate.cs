using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Chunks")]
    public GameObject beggingChunk; //start chuck
    public GameObject[] randomChunks;  
    public GameObject finalChunk;   //final chuck

    [Header("Players")] // for players
    public Transform player1;
    public Transform player2;

    [Header("Settings")] // for setting of  the chucks 
    public Vector3 startp1 = new Vector3(-20, 0, 0); //staring for player 1
    public Vector3 startp2 = new Vector3(20, 0, 0);  //starting for player 2
    public int chunkCount = 5; //number of random chucks

    private void Start()
    {
        List<int> chunkSequence = generate();

        generateforplayers(startp1, chunkSequence);
        generateforplayers(startp2, chunkSequence);
    }

    List<int> generate()
    {
        List<int> sequence = new List<int>(); // we create a lost for random chucks 
        for (int i = 0; i < chunkCount; i++)
        {
            int rand = Random.Range(0, randomChunks.Length); 
            sequence.Add(rand); // add it to the list
        }
        return sequence;
    }

    void generateforplayers(Vector3 startPos, List<int> chucklist)
    {
        Vector3 spawnPos = startPos;
        GameObject startchuck = Instantiate(beggingChunk, spawnPos, Quaternion.identity);
        //instantiate chuck1
        Transform endpoint = startchuck.transform.Find("endpoint"); // find the location of the end point in the start chuck
        if (endpoint != null)
        {
            Vector3 localOffset = endpoint.localPosition;
            spawnPos += localOffset;
            //move the spawn position to the end point of the chuck
        }

        foreach (int index in chucklist)
        {
            GameObject chunk = Instantiate(randomChunks[index], spawnPos, Quaternion.identity);
            //instantiate the random chuck

            Transform endPoint = chunk.transform.Find("endpoint");
            if (endPoint != null)
            {
                Vector3 localOffset = endPoint.localPosition;
                spawnPos += localOffset;
            }

            //find the endpoint and move the spawn pos to there 

        }

        GameObject final = Instantiate(finalChunk, spawnPos, Quaternion.identity);
        // instatiate the final chuck
    }
}
