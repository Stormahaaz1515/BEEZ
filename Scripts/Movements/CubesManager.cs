using System.Collections;
using System.Collections.Generic;
using UnityEngine.Accessibility;
using UnityEngine;

public class CubesManager : MonoBehaviour
{
    [Tooltip("Prefab of the item we want to manage")]
    public Transform cubesPrefabs;
    [Tooltip("Number of instance we want to create")]
    public int nbCubesCreate = 5;
    [Tooltip("Size of the zone to make spawn our instances")]
    public float zoneRange = 20f;

    // Start is called before the first frame update
    void Start()
    {
        Transform prevCube = null;
        Color[] palette = new Color[nbCubesCreate];
        VisionUtility.GetColorBlindSafePalette(palette, 0.5f, 1f);
        for (int i = 0; i < nbCubesCreate; i++)
        {
            Transform cube = GameObject.Instantiate<Transform>(cubesPrefabs);
            cube.parent = transform;
            cube.position = Random.insideUnitSphere * zoneRange;
            cube.position = new Vector3(cube.localPosition.x, 0, cube.localPosition.z);

            cube.GetComponent<RandomMovements>().vitesseMax *= Random.Range(0.5f, 2f);
            cube.localScale *= Random.Range(0.5f, 2f);
            cube.GetComponent<RandomMovements>().acceleration *= Random.Range(0.5f, 2f);
            cube.GetComponent<Renderer>().material.color = palette[i];

            if (Random.Range(0.0f, 1.0f) < 0.5f)
            {
                cube.GetComponent<RandomMovements>().target = prevCube;
            }
            prevCube = cube;
        }
    }
}
