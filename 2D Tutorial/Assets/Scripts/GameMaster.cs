using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;
    public Transform playerPrefab;
    public Transform spawnParticles;
    public Transform spawnPoint;
    public float SpawnDelay = 2f;

    string gameMasterTag = "GM";
        
    // Use this for initialization
    void Start()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag(gameMasterTag).GetComponent<GameMaster>();
        }
    }

    public IEnumerator RespawnPlayer()
    {
        this.GetComponent<AudioSource>().Play();
        //Debug.Log(Time.time);
        yield return new WaitForSeconds(SpawnDelay);
        //Debug.Log(Time.time);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        GameObject cloneOfParticles = Instantiate(spawnParticles, spawnPoint.position, spawnPoint.rotation) as GameObject;
        Destroy(cloneOfParticles, 3f);
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.RespawnPlayer());
    }
}
