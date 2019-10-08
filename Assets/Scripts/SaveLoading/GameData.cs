using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct LeaderboardEntry
    {
        public string playerName;
        public int playerScore;
    }

[System.Serializable]
public class GameData
{
    
    public List<EnemyMetaData> enemyMetaDataList = new List<EnemyMetaData>();

    public List<LeaderboardEntry> leaderboardList = new List<LeaderboardEntry>();

    public void AddScoreToLeaderboard(LeaderboardEntry _leaderboardEntry)
    {
        int insertionIndex = leaderboardList.FindIndex(x => x.playerScore <= _leaderboardEntry.playerScore);

        leaderboardList.Insert(insertionIndex, _leaderboardEntry);
    }
}
