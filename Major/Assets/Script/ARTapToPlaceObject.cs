using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.EventSystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;
    private ARRaycastManager raycastmanager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    public bool IsPlaced = false;

    public GameObject currentBuilding;
    public GameObject basketBall;
    private GameObject spawnedBall;

    public GameObject scanInstruction;
    public GameObject scoreBoard;
    // Start is called before the first frame update
    void Start()
    {
        raycastmanager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsPlaced)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                PlaceObject();
            }
           
           
            // This code now runs with play button
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        //raycastmanager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon);

        raycastmanager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }

    }

    public void PlaceObject()
    {
        
        currentBuilding.transform.position = placementPose.position;
        currentBuilding.SetActive(true);
        spawnedBall = Instantiate(basketBall);
        spawnedBall.transform.parent = raycastmanager.transform.Find("AR Camera").gameObject.transform;

        IsPlaced = true;

      
    }
    // Not Using this 
    private void UpdatePlacementIndicator()
    {
        if (!IsPlaced)
        {
            if (placementPoseIsValid)
            {
                scanInstruction.SetActive(false);
                placementIndicator.SetActive(true);
                placementIndicator.transform.position = placementPose.position;
                placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
            else
            {
              //  playButton.SetActive(false);
                placementIndicator.SetActive(false);
            }
        }
        else
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.GetChild(0).gameObject.SetActive(false);
            scoreBoard.SetActive(true);
        }
    }
    public void ResetTracking()
    {
        if (IsPlaced)
        {
            
            IsPlaced = false;
            placementIndicator.SetActive(true);
            placementIndicator.transform.GetChild(0).gameObject.SetActive(true);
            currentBuilding.SetActive(false);
            currentBuilding.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            
        }

    }
}
