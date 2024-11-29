using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSorterManager : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    private List<BuildingSorter> buildingSorters = new List<BuildingSorter>(); // List to store BuildingSorters

    void Start()
    {
        BuildingSorter[] sorters = FindObjectsOfType<BuildingSorter>(); // Find all BuildingSorters
        buildingSorters.AddRange(sorters); // Adds all building sorters to list
    }

    void Update()
    {
        BuildingSorter closestSorter = null;
        float closestDistance = float.MaxValue;

        // Determine the closest building sorter
        foreach (BuildingSorter sorter in buildingSorters)
        {
            if(player != null)
            {
                // Get player's position relative to building
                float distance = Vector3.Distance(player.transform.position, sorter.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestSorter = sorter;
                }
            }   
        }

        // Update the sorting order based on the closest building sorter
        if (closestSorter != null)
        {
            closestSorter.UpdateSortingOrder();
        }
    }
}