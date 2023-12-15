using static Nazdar.Enums;

namespace Nazdar.Shared;

public class ScoreStrings
{
    public int Village { get; set; }
    public Villages VillageName { get; set; }
    public int Days { get; set; }
    public int ScoreValue { get; set; }
    public int Money { get; set; }
    public int Kills { get; set; }
    public ScoreStrings(int village, int days, int scoreValue, int money, int kills)
    {
        Village = village;
        VillageName = villageName;
        Days = days;
        ScoreValue = scoreValue;
        Money = money;
        Kills = kills;
    }
}