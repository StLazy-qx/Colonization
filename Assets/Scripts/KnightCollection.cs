using System.Linq;

public class KnightCollection : Collector<Knight>
{
    public bool TryGetFreeKnight(out Knight knight)
    {
        knight = GetActiveObjects().
            FirstOrDefault(knight => knight.IsBusy == false);
        return knight != null;
    }

    public bool HasKnights()
    {
        int minKnightCount = 1;

        return Count > minKnightCount;
    }
}
