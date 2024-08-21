using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public List<Fish> fishes;

    public Fish GetRandomFish()
    {
        //chatgpt help with weight calculation
        float totalWeight = 0f;

        foreach (Fish fish in fishes)
        {
            totalWeight += 1f / fish.rarity;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;
        foreach (Fish fish in fishes)
        {
            cumulativeWeight += fish.rarity;
            if (randomValue <= cumulativeWeight)
            {
                return fish;
            }
        }

        return null;
    }
}
