using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public GameObject CharacterPrefab;
    public Transform[] SpawnPoints;
    public Transform[] FoodStations;
    public Transform[] CashRegisters;
    public int CharacterCount = 10;

    void Start()
    {
        for (int i = 0; i < CharacterCount; i++)
        {
            SpawnCharacter();
        }
    }

    void SpawnCharacter()
    {
        Transform spawnPoint = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        GameObject character = Instantiate(CharacterPrefab, spawnPoint.position, Quaternion.identity);

        CharacterBehavior behavior = character.GetComponent<CharacterBehavior>();
        behavior.FoodStations = FoodStations;
        behavior.CashRegisters = CashRegisters;
    }
}
