﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public int stars;
	public int coins;
	public Dictionary<string, LevelData> levelsData;

	public GameData() 
	{
		levelsData = new Dictionary<string, LevelData>();
		stars = 0;
		coins = 0;
	}
}

public class PersistentGameData : MonoBehaviour
{
	[SerializeField] string gameDataFileName;

	public static PersistentGameData Instance { get; private set; }
	public GameData gameData { get; private set; }

	private void Start()
	{
		Instance = this;

		gameData = new GameData();

		LoadLocalGameData();

		DontDestroyOnLoad(this.gameObject);
	}

	private void LoadLocalGameData()
	{
		string gameDataLocalPath = gameDataFileName;
		if (!Application.isEditor)
			gameDataLocalPath = Application.persistentDataPath + "/" + gameDataFileName;

		if (File.Exists(gameDataFileName))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream stream = new FileStream(gameDataFileName, FileMode.Open);

			gameData = binaryFormatter.Deserialize(stream) as GameData;
			stream.Close();
		}
		else
		{
			gameData = new GameData();

			for (int i = 0; i < LevelBased.GameManager.Instance.starsForUnlockingLevels.Count; i++)
			{
				string currentLevelID = LevelBased.GameManager.Instance.starsForEachLevels[i].levelID;
				gameData.levelsData.Add(currentLevelID, new LevelData());
			}

			UpdateLocalGameData();

			Debug.Log("There is no local saved data");
		}
	}

	public void UpdateLevelData(LevelData levelData)
	{
		LevelData currentLevel = gameData.levelsData[levelData.levelID];

		if (currentLevel.highscore < levelData.highscore)
			currentLevel.highscore = levelData.highscore;

		currentLevel.starsRequiredToUnlock = levelData.starsRequiredToUnlock;

		if (currentLevel.idsOfStarsPicked.Count < levelData.idsOfStarsPicked.Count)
			currentLevel.idsOfStarsPicked = levelData.idsOfStarsPicked;

		gameData.levelsData[levelData.levelID] = currentLevel; 
		UpdateLocalGameData();
	}

	private void UpdateLocalGameData()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();

		string gameDataLocalPath = gameDataFileName;
		if (!Application.isEditor)
			gameDataLocalPath = Application.persistentDataPath + "/" + gameDataFileName;

		FileStream stream = new FileStream(gameDataLocalPath, FileMode.Create);

		binaryFormatter.Serialize(stream, gameData);
		stream.Close();
	}
}
