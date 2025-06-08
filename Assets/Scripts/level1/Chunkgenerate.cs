using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Chunks")]
    public GameObject beggingChunk;
    public GameObject[] randomChunks;   // Prefabهای چانک‌های تصادفی
    public GameObject finalChunk;       // Prefab چانک پایانی

    [Header("Players")]
    public Transform player1;
    public Transform player2;

    [Header("Settings")]
    public Vector3 startPositionPlayer1 = new Vector3(-20, 0, 0);
    public Vector3 startPositionPlayer2 = new Vector3(20, 0, 0); 
    public int chunkCount = 5; 

    private void Start()
    {
        List<int> chunkSequence = GenerateChunkSequence();

        GeneratePathForPlayer(startPositionPlayer1, chunkSequence);
        GeneratePathForPlayer(startPositionPlayer2, chunkSequence);
    }

    List<int> GenerateChunkSequence()
    {
        List<int> sequence = new List<int>();
        for (int i = 0; i < chunkCount; i++)
        {
            int rand = Random.Range(0, randomChunks.Length);
            sequence.Add(rand);
        }
        return sequence;
    }

    void GeneratePathForPlayer(Vector3 startPos, List<int> chunkSequence)
    {
        Vector3 spawnPos = startPos;
        GameObject startChunk = Instantiate(beggingChunk, spawnPos, Quaternion.identity);
        Transform beginEndPoint = startChunk.transform.Find("endpoint");
        if (beginEndPoint != null)
        {
            Vector3 localOffset = beginEndPoint.localPosition;
            spawnPos += localOffset;
        }

        foreach (int index in chunkSequence)
        {
            GameObject chunk = Instantiate(randomChunks[index], spawnPos, Quaternion.identity);

            Transform endPoint = chunk.transform.Find("endpoint");
            if (endPoint != null)
            {
                Vector3 localOffset = endPoint.localPosition;
                spawnPos += localOffset;
            }
            
        }

        GameObject final = Instantiate(finalChunk, spawnPos, Quaternion.identity);
    }
}
