using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    
    public static LevelManager sharedInstance;

    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();  // All the level block avalaibles.
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>(); // Current level blocks in the scene.
    public Transform levelStartPosition;

    
    // Walk Block to not fall from the edge at the level start point.
    public GameObject walkBlockPrefab; // For the prefab
    private GameObject walkBlock; // For the instance of the prefab

    void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start() {
        GenerateInitialLevelsBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLevelBlock(bool inExitZone) {
        // Level block
        int randomIdx = Random.Range(0, allTheLevelBlocks.Count);

        LevelBlock block;
        Vector3 spawnPosition = Vector3.zero;

        if (currentLevelBlocks.Count == 0) { // If it's the first level block.
            block = Instantiate(allTheLevelBlocks[0]);
            spawnPosition = levelStartPosition.position;
        } else {
            block = Instantiate(allTheLevelBlocks[randomIdx]);
            spawnPosition = currentLevelBlocks[currentLevelBlocks.Count - 1].EndPoint.position;
        }

        block.transform.SetParent(this.transform, false); // Add block to the scene

        // Walk Block to not fall from the edge at the level start point.
        if (currentLevelBlocks.Count == 0) { // If it's the first level block.
            AddWalkBlock(block);
        } else if (inExitZone) {
            AddWalkBlock(currentLevelBlocks[1]);
        }

        // Continue the level block logic
        Vector3 correction = new Vector3(
            spawnPosition.x - block.StartPoint.position.x,
            spawnPosition.y - block.StartPoint.position.y,
            0
        );

        block.transform.position = correction;

        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock() {
        // Remove block.
        LevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }

    public void RemoveAllLevelsBlock() {
        while (currentLevelBlocks.Count > 0) {
            RemoveLevelBlock();
        }
    }

    public void GenerateInitialLevelsBlock() {
        for (int i = 0; i < 7; i++) {
            AddLevelBlock(false);
        }
    }

    void AddWalkBlock(LevelBlock block) {
        walkBlock = Instantiate(walkBlockPrefab);
        walkBlock.transform.SetParent(block.transform, false); // Add walkBlock to the Block Level;
    }
}
