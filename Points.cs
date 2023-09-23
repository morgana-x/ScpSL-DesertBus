using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
namespace Desert_Bus_SCP_SL
{
    public class Points
    {
        public Dictionary<string, int> playerPoints = new Dictionary<string, int>();

        public void saveData()
        {

        }
        public void loadData()
        {

        }
        public void givePointsAll()
        {
            foreach (Player pl in Player.List)
            {
                givePoints(pl.UserId, 1); // lol!
            }
            Log.Debug("Gave a point to every player");
        }
        public void updateCustomInfo(string userId)
        {
            Player pl = Player.Get(userId);
            if (pl == null)
            {
                return;
            }
            if (!playerPoints.ContainsKey(userId))
            {
                playerPoints.Add(userId, 0);
            }
            pl.CustomInfo = playerPoints[userId].ToString();
        }
        public void givePoints(string userId, int amount) 
        {
            if (!playerPoints.ContainsKey(userId))
            {
                playerPoints.Add(userId, 0);
            }
            playerPoints[userId] += amount;
            updateCustomInfo(userId);
        }
        public void takePoints(string userId, int amount)
        {
            if (!playerPoints.ContainsKey(userId))
            {
                playerPoints.Add(userId, 0);
                return;
            }
            playerPoints[userId] -= amount;
            if (playerPoints[userId] < 0) 
            {
                playerPoints[userId] = 0;
            }
            updateCustomInfo(userId);
        }
    }
}
