using UnityEngine;
using System.Collections;

public class QueueManager : MonoBehaviour
{
    public GameObject CharacterPrefab;
    public Transform SpawnPoint;
    public int CharacterCount = 10;
    public float SpawnInterval = 7f; 

    void Start()
    {
        StartCoroutine(SpawnCharactersWithInterval());
    }

    IEnumerator SpawnCharactersWithInterval()
    {
        for (int i = 0; i < CharacterCount; i++)
        {
            SpawnCharacter();
            yield return new WaitForSeconds(SpawnInterval); 
        }
    }

    void SpawnCharacter()
    {
        GameObject character = Instantiate(CharacterPrefab, SpawnPoint.position, Quaternion.identity);

        NPCController behavior = character.GetComponent<NPCController>();
    }
}
