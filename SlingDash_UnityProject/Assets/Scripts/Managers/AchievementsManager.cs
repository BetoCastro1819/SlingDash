﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Achievements
{
	EARN_MORE_THAN_100_COINS,
	EARN_MORE_THAN_500_COINS,
	EARN_MORE_THAN_1000_COINS
}

public class AchievementsManager : MonoBehaviour 
{
	#region Singleton
	public static AchievementsManager Instance { get; private set; }
	void Awake() 
	{
		Instance = this;	
	}
	#endregion

	private Dictionary<Achievements, bool> achievementsData;
	private PersistentGameData persistentGameData;

	private void Start()
	{
		AchievementsManager[] achievementsManager = FindObjectsOfType<AchievementsManager>();
		if (achievementsManager.Length == 1)
		{
			Initialize();
		}
	}

	private void Initialize()
	{
		// TODO initialize achievements with values saved on persistentData
		persistentGameData = PersistentGameData.Instance;
		achievementsData = persistentGameData.gameData.achievementsData;

		if (achievementsData.Count <= 0)
		{
			achievementsData = new Dictionary<Achievements, bool>();
			achievementsData.Add(Achievements.EARN_MORE_THAN_100_COINS, false);
			achievementsData.Add(Achievements.EARN_MORE_THAN_500_COINS, false);
			achievementsData.Add(Achievements.EARN_MORE_THAN_1000_COINS, false);

			PersistentGameData.Instance.UpdateAchivements(achievementsData);
		}

		DontDestroyOnLoad(this.gameObject);
	}

	public void CheckForUnlockingCoinsAchievement(int coins)
	{
		if (coins < 100) return;

		Achievements achievementToCheck = Achievements.EARN_MORE_THAN_100_COINS;

		if (coins >= 500) achievementToCheck = Achievements.EARN_MORE_THAN_500_COINS;
		if (coins >= 1000) achievementToCheck = Achievements.EARN_MORE_THAN_1000_COINS;

		// Achievement is already unlocked
		if (achievementsData[achievementToCheck] == true)
		{
			Debug.Log("Achievement already unlocked: " + achievementToCheck.ToString());
			return;
		}

		switch(achievementToCheck)
		{
			case Achievements.EARN_MORE_THAN_100_COINS:
				if (coins >= 100)
				{
					achievementsData[achievementToCheck] = true;
					persistentGameData.UpdateAchivements(achievementsData);
					Debug.Log("Achievement Unlocked: " + achievementToCheck.ToString());
				}
				break;

			case Achievements.EARN_MORE_THAN_500_COINS:
				if (coins >= 500)
				{
					achievementsData[achievementToCheck] = true;
					persistentGameData.UpdateAchivements(achievementsData);

					Debug.Log("Achievement Unlocked: " + achievementToCheck.ToString());
				}
				break;

			case Achievements.EARN_MORE_THAN_1000_COINS:
				if (coins >= 1000)
				{
					achievementsData[achievementToCheck] = true;
					persistentGameData.UpdateAchivements(achievementsData);

					Debug.Log("Achievement Unlocked: " + achievementToCheck.ToString());
				}
				break;
		}
	}
}