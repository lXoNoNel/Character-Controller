using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

public class SearchFor : IState
{
    LayerMask searchLayer;
    GameObject ownerGameObj;
    float searchRadius;
    string tagToLookFor;
    public bool searchCompleted = false;

    private System.Action<SearchResults> searchResultsCallback;


    public SearchFor(LayerMask searchLayer, GameObject ownerGameObj, float searchRadius, string tagToLookFor, Action<SearchResults> searchResultsCallback)
    {
        this.searchLayer = searchLayer;
        this.ownerGameObj = ownerGameObj;
        this.searchRadius = searchRadius;
        this.tagToLookFor = tagToLookFor;
        this.searchResultsCallback = searchResultsCallback;
    }

    public override void Enter()
    {
        
    }
    public override void Execute()
    {
        if(!searchCompleted)
        {
            var hitObjects = Physics.OverlapSphere(this.ownerGameObj.transform.position, this.searchRadius);

            var allObjectsWithTheRequiredTag = new List<Collider>();

            for(int i = 0; i < hitObjects.Length; i++)
            {
                if(hitObjects[i].CompareTag(this.tagToLookFor))
                {
                    allObjectsWithTheRequiredTag.Add(hitObjects[i]);
                }
            }

            var searchResults = new SearchResults(hitObjects, allObjectsWithTheRequiredTag);

        
            if(searchResults.allHitObjectsWithRequiredTag != null)
                {
                    this.searchResultsCallback(searchResults);
                    this.searchCompleted = true;
                }
            }
        }

    
        public override void FixedExecute()
        {
            
        }

    public override void Exit()
    {
        
    }   
}


public class SearchResults
{
    public Collider[] allHitObjectsInSearchRadius;
    public List<Collider> allHitObjectsWithRequiredTag;

    public SearchResults(Collider[] allHitObjectsInSearchRadius, List<Collider> allHitObjectsWithRequiredTag)
    {
        this.allHitObjectsInSearchRadius = allHitObjectsInSearchRadius;
        this.allHitObjectsWithRequiredTag = allHitObjectsWithRequiredTag;
    }
}