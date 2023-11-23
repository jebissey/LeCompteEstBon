class Program
{
    private static readonly List<int> tiles = [];
    private static bool found = false;
    private static int bestResult = 0;
    private static readonly IDictionary<Operation, Func<int, int, int>> DoOperations = new Dictionary<Operation, Func<int, int, int>>()
    {
        [Operation.Mul] = DoMul,
        [Operation.Add] = DoAdd,
        [Operation.Sub] = DoSub,
        [Operation.Div] = DoDiv,
    };
    private static readonly IDictionary<Operation, Action<int, int>> DisplayOperations = new Dictionary<Operation, Action<int, int>>()
    {
        [Operation.Mul] = DisplayMul,
        [Operation.Add] = DisplayAdd,
        [Operation.Sub] = DisplaySub,
        [Operation.Div] = DisplayDiv,
    };

    enum Operation
    {
        Mul,
        Add,
        Div,
        Sub,
    }

    static void Main(string[] args)
    {
        if (args == null || args.Length < 3 || args.Length > 7)
        {
            Console.WriteLine("Il faut donner au moins 2 plaques et pas plus de 6 et le nombre à trouver");
        }
        else
        {
            args.ToList().Take(args.Length - 1).ToList().ForEach(x => tiles.Add(int.Parse(x)));
            if (!Find(tiles, int.Parse(args.Last()))) Console.WriteLine($"Le nombre le plus proche est {bestResult}");
        }

        #region Local methods
        static bool Find(List<int> tiles, int number)
        {
            int newTile;
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = i + 1; j < tiles.Count; j++)
                {
                    foreach (Operation operation in Enum.GetValues(typeof(Operation)))
                    {
                        if (found) break;
                        if ((newTile = DoOperations[operation](tiles[i], tiles[j])) == number) found = true;
                        else if (newTile != 0)
                        {
                            if (Math.Abs(newTile - number) < Math.Abs(bestResult - number)) bestResult = newTile;
                            if (tiles.Count > 2) ContinueWithNewTile(i, j);
                        }
                        if (found)
                        {
                            DisplayOperations[operation](tiles[i], tiles[j]);
                            return true;
                        }
                    }
                }
            }
            return false;

            #region Local methods
            void ContinueWithNewTile(int i, int j)
            {
                List<int> newTiles = [];
                for (int x = 0; x < tiles.Count; x++)
                {
                    if (x != i && x != j) newTiles.Add(tiles[x]);
                }
                newTiles.Add(newTile);
                _ = Find(newTiles, number);
            }
            #endregion
        }
        #endregion
    }

    #region Local Methods
    private static int DoMul(int x, int y) => x != 1 && y != 1 ? x * y : 0;
    private static int DoAdd(int x, int y) => x + y;
    private static int DoSub(int x, int y) => Math.Abs(x - y);
    private static int DoDiv(int x, int y) => x != 1 && y != 1 && Math.Max(x, y) % Math.Min(x, y) == 0 ? Math.Max(x, y) / Math.Min(x, y) : 0;

    private static void DisplayMul(int x, int y) => Console.WriteLine($"{x} x {y} = {x * y}");
    private static void DisplayAdd(int x, int y) => Console.WriteLine($"{x} + {y} = {x + y}");
    private static void DisplaySub(int x, int y) => Console.WriteLine($"{Math.Max(x, y)} - {Math.Min(x, y)} = {Math.Abs(x - y)}");
    private static void DisplayDiv(int x, int y) => Console.WriteLine($"{Math.Max(x, y)} / {Math.Min(x, y)} = {Math.Max(x, y) / Math.Min(x, y)}");
    #endregion
}
