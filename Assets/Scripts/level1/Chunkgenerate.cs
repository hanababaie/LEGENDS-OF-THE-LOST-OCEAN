using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Chunks")]
    public GameObject beggingChunk; // start chunk
    public GameObject[] randomChunks;  
    public GameObject finalChunk;   // final chunk

    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Settings")]
    public Vector3 startp1 = new Vector3(-20, 0, 0);
    public Vector3 startp2 = new Vector3(20, 0, 0);
    public int chunkCount = 5;

    private List<int> chunkSequence = new List<int>();

    public void GenerateChunksAtStart(Vector3 startPos)
    {
        if (chunkSequence == null || chunkSequence.Count == 0)
        {
            chunkSequence = GenerateNewSequence();
        }
        generateforplayers(startPos, chunkSequence);
    }

    List<int> GenerateNewSequence()
    {
        List<int> sequence = new List<int>();
        for (int i = 0; i < chunkCount; i++)
        {
            int rand = Random.Range(0, randomChunks.Length);
            sequence.Add(rand);
        }
        return sequence;
    }

    void generateforplayers(Vector3 startPos, List<int> chunkList)
    {
        Vector3 spawnPos = startPos;
        GameObject startChunk = Instantiate(beggingChunk, spawnPos, Quaternion.identity);

        Transform endpoint = startChunk.transform.Find("endpoint");
        if (endpoint != null)
        {
            Vector3 localOffset = endpoint.localPosition;
            spawnPos += localOffset;
        }

        foreach (int index in chunkList)
        {
            GameObject chunk = Instantiate(randomChunks[index], spawnPos, Quaternion.identity);

            Transform endPoint = chunk.transform.Find("endpoint");
            if (endPoint != null)
            {
                Vector3 localOffset = endPoint.localPosition;
                spawnPos += localOffset;
            }
        }

        Instantiate(finalChunk, spawnPos, Quaternion.identity);
    }

    public List<int> GetChunkSequence()
    {
        return chunkSequence;
    }

    public void SetChunkSequence(List<int> sequence)
    {
        chunkSequence = sequence;
    }

    public void ClearChunks()
    {
        chunkSequence.Clear();
    }
}
