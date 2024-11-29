using UnityEngine;

public class BuildingSorter : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    //public Transform referencePointY; // Reference point for Y-direction
    public Transform referencePointX; // Reference point for X-direction
    public Transform referencePointX2; // Reference point for X-direction

    public int leftOfBuildingOrder; // Left of building (default)
    public int rightOfBuildingOrder; // Right of building (player order default)
    public int aboveBuildingOrder; // On top of building (player order higher)
    //public int belowBuildingOrder; // Below building (default)

    private SpriteRenderer playerRenderer; // Reference to player's SpriteRenderer

    void Start()
    {
        // Get the SpriteRenderer component from the player GameObject
        playerRenderer = player.GetComponent<SpriteRenderer>();
    }

    public void UpdateSortingOrder()
    {
        // Get the player's position
        //float playerY = player.transform.position.y;
        float playerX = player.transform.position.x;

        // Get the reference points' positions
        //float referenceY = referencePointY.position.y;
        float referenceX = referencePointX.position.x;
        float referenceX2 = referencePointX2.position.x;

        // Update the sorting order
        if (playerX < referenceX)
        {
            playerRenderer.sortingOrder = leftOfBuildingOrder; // Player in front of building
        }
        else if (playerX > referenceX2)
        {
            playerRenderer.sortingOrder = rightOfBuildingOrder; // Player is past building
        }
        else
        {
            playerRenderer.sortingOrder = aboveBuildingOrder; // Between X and X2 -> Player on top of building
        }
    }
}