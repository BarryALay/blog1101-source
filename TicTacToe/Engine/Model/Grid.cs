namespace TicTacToe.Engine.Model
{
    internal enum CellType
    {
        Empty, X, O, None
    }

    internal class Grid
    {
        public static int MinRow = 1;
        public static int MaxRow = 3;
        public static int MinColumn = 1;
        public static int MaxColumn = 3;

        private readonly CellType[,] cells = new CellType[3, 3]
        {
            { CellType.Empty, CellType.Empty, CellType.Empty },
            { CellType.Empty, CellType.Empty, CellType.Empty },
            { CellType.Empty, CellType.Empty, CellType.Empty }
        };

        /// <summary>
        /// Get or set cell.
        /// </summary>
        /// <param name="row">Row number, 1 to 3</param>
        /// <param name="column">Column number, 1 to 3</param>
        /// <returns></returns>
        public CellType this[int row, int column]
        {
            get
            {
                if (row < MinRow || row > MaxRow || column < MinColumn || column > MaxColumn) return CellType.None;
                return cells[row - 1, column - 1];
            }
            set
            {
                if (row < MinRow || row > MaxRow || column < MinColumn || column > MaxColumn) return;
                cells[row - 1, column - 1] = value;
            }
        }

        public void SetCellType(int row, int column, CellType cellType) => this[row, column] = cellType;

        public void Clear()
        {
            for (int row = MinRow; row <= MaxRow; row++)
                for (int column = MinColumn; column <= MaxColumn; column++)
                    this[row, column] = CellType.Empty;
        }
    }
}
