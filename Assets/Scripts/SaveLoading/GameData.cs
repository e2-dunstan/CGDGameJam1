using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class GameData : MonoBehaviour
{
    public struct LeaderboardEntry
    {
        public string playerName;
        public int playerScore;
    }

    public List<EnemyMetaData> enemyMetaDataList = new List<EnemyMetaData>();

    public List<LeaderboardEntry> leaderboardList = new List<LeaderboardEntry>();

    public void AddScoreToLeaderboard(LeaderboardEntry _leaderboardEntry)
    {
        leaderboardList.Where(x => x.playerScore > _leaderboardEntry.playerScore);
    }
}
