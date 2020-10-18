using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecleonSpawner : MonoBehaviour {
    [Range(0, 1)]
    public float timeBetweenSpawner;
    public float spawnerDistance;
    public Nucleon[] nucleonPrefabs;

    private float timeSinceLastSpawner = 0;

    private void FixedUpdate()
    {
        timeSinceLastSpawner += Time.deltaTime;
        if (timeSinceLastSpawner >= timeBetweenSpawner)
        {
            timeSinceLastSpawner -= timeBetweenSpawner;
            SpawnNucleon();
        }
    }

    private void SpawnNucleon()
    {
        Nucleon nucleon = nucleonPrefabs[Random.Range(0, nucleonPrefabs.Length)];
        Nucleon spawn = Instantiate<Nucleon>(nucleon);

        spawn.transform.localPosition = Random.onUnitSphere * spawnerDistance;
    }
}
