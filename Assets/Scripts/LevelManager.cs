using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager sharedInstance;

    public List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();  // All the level block avalaibles.
    public List<LevelBlock> currentLevelBlocks = new List<LevelBlock>(); // Current level blocks in the scene.
    public Transform levelStartPosition;


    void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()     {
        GenerateInitialLevelsBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLevelBlock() {
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

        block.transform.SetParent(this.transform, false);

        Vector3 correction = new Vector3(
            spawnPosition.x - block.StartPoint.position.x,
            spawnPosition.y - block.StartPoint.position.y,
            0
        );

        block.transform.position = correction;

        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock() {
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
        for (int i = 0; i < 4; i++) {
            AddLevelBlock();
        }
    }
}
