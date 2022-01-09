using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
    public class ARTapToPlaceObject : MonoBehaviour
    {
        //Events for tutorial before game start
        public delegate void ArenaSpawned();
        public static event ArenaSpawned OnArenaSpawned;
        
        public GameObject gameObjectToInstantiate;

        private GameObject _spawnedObject;
        private ARRaycastManager _arRaycastManager;
        private Vector2 _touchedPosition;

        static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

        private void Awake()
        {
            _arRaycastManager = GetComponent<ARRaycastManager>();
        }
        private bool tryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
            touchPosition = default;
            return false;
        }

        void Update()
        {
            if (!tryGetTouchPosition(out Vector2 touchPosition))
                return;

            if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;

                //is there a spawnedObject already to move it around? or we have to create it
                if(_spawnedObject == null)
                {
                    _spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
                    //Event that Arena was spawned
                    OnArenaSpawned?.Invoke();
                }
                else
                {
                    //move around
                    _spawnedObject.transform.position = hitPose.position;
                }
            }
        }
    }
