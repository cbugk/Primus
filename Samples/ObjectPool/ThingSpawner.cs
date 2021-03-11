using System;
using System.Collections.Concurrent;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Primus.ObjectPool.Example
{
    public class ThingSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _thingOne;
        [SerializeField] private GameObject _thingTwo;

        private ThingOnePool _poolOne;
        private ThingTwoPool _poolTwo;

        private GenericPool<GenericProduct>[] _poolArray;

        // New input system controls.
        private SpawnerControls _spawnerControls;
        // New input system button returns float (0.0f or 1.0f). Will be used like a bool here.
        private float _valueTriggerSpawn;
        // Spawnrate limit in seconds
        private float _spawnTimeLimitInSeconds = 1.5f;
        // Last known spawn time;
        private float _lastSpawnInSeconds;

        private void Spawn(int typeIndex)
        {
            GenericProduct someThing = null;

            switch (typeIndex)
            {
                case 0:
                    someThing = _poolOne.GetPrefabInstance();
                    break;
                case 1:
                    someThing = _poolTwo.GetPrefabInstance();
                    break;
            }

            if (someThing != null) someThing.transform.localPosition = 5.0f * new Vector3(Random.value, Random.value, Random.value);

        }

        private void Awake()
        {
            _spawnerControls = new SpawnerControls();

            // Temporary reference for creating pools.
            GameObject temporaryObject;

            // Create ThingOnePool
            temporaryObject = new GameObject();
            temporaryObject.transform.parent = transform;
            temporaryObject.name = "GameObjPoolOne";
            _poolOne = temporaryObject.AddComponent<ThingOnePool>();
            // Set corresponding prefab.
            _poolOne.Prefab = _thingOne.GetComponent<ThingOne>();

            // Create ThingTwoPool
            temporaryObject = new GameObject();
            temporaryObject.transform.parent = transform;
            temporaryObject.name = "GameObjPoolTwo";
            _poolTwo = temporaryObject.AddComponent<ThingTwoPool>();
            // Set corresponding prefab.
            _poolTwo.Prefab = _thingTwo.GetComponent<ThingTwo>();
        }

        private void Update()
        {
            // If set button pressed and should trigger a spawn
            if (Mathf.Approximately(1.0f, _spawnerControls.Example.SpawnThingOne.ReadValue<float>()))
            {
                float currentTime = Time.time;
                // Nested for easier reading only.
                // If timelimit has passed
                if ((currentTime - _lastSpawnInSeconds) >= _spawnTimeLimitInSeconds)
                {
                    _lastSpawnInSeconds = currentTime;
                    Spawn(0);
                }
            }

            // If set button pressed and should trigger a spawn
            if (Mathf.Approximately(1.0f, _spawnerControls.Example.SpawnThingTwo.ReadValue<float>()))
            {
                float currentTime = Time.time;
                // Nested for easier reading only.
                // If timelimit has passed
                if ((currentTime - _lastSpawnInSeconds) >= _spawnTimeLimitInSeconds)
                {
                    _lastSpawnInSeconds = currentTime;
                    Spawn(1);
                }
            }
        }

        // For new input system.
        private void OnEnable()
        {
            _spawnerControls.Enable();
        }
        // For new input system.
        private void Disable()
        {
            _spawnerControls.Disable();
        }
    }
}