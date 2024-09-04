using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToPlayer : MonoBehaviour
{ 
    [SerializeField] private Transform player;   // Reference to the player's transform
    [SerializeField] private float threshold = 3f; // Distance threshold to check against

    private Transform entityTransform;            // Cached reference to this entity's transform

    private void Start()
    {
        entityTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        bool isWithinRange = IsPlayerWithinDistance();
        Debug.Log("Is player within range: " + isWithinRange);
    }

    private bool IsPlayerWithinDistance()
    {
        // Calculate the squared distance to avoid using the expensive sqrt operation
        float squaredDistance = (player.position - entityTransform.position).sqrMagnitude;

        // Compare squared distance with the squared threshold
        return squaredDistance < threshold * threshold;
    }
}
