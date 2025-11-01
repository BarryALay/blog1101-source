using System.Text;

namespace TicTacToe.Engine.Model
{
    internal enum LineType
    {
        Row, Column, Diagonal
    }

    internal class Line(LineType type, int id)
    {
        public LineType Type { get; set; } = type;

        /// <summary>
        /// Identifies line.
        /// <list type="bullet">
        /// <item>For Row, row number (1-3)</item>
        /// <item>For Column, column number (1-3)</item>
        /// <item>For Diagonal, 0 for top-left to bottom-right, 1 for bottom-left to top-right</item>
        /// </list>
        /// </summary>
        public int Id { get; set; } = id;

        /// <summary>
        /// Maximum number of different lines.
        /// </summary>
        public static int MaxLines = 8;

        public static int MinIndex = 1;
        public static int MaxIndex = 3;

        public static int MainDiagonal = 0;
        public static int CounterDiagonal = 1;

        private CellType[] cells = [CellType.Empty, CellType.Empty, CellType.Empty];

        private readonly CellType[] XWin = [CellType.X, CellType.X, CellType.X];
        private readonly CellType[] OWin = [CellType.O, CellType.O, CellType.O];

        public void Clear()
        {
            cells = [CellType.Empty, CellType.Empty, CellType.Empty];
        }

        public bool AcceptsMove(Move move)
        {
            if (move.Row < MinIndex || move.Row > MaxIndex || move.Column < MinIndex || move.Column > MaxIndex) return false;
            switch (Type)
            {
                case LineType.Row:
                    return Id == move.Row && cells[move.Column - 1] == CellType.Empty;
                case LineType.Column:
                    return Id == move.Column && cells[move.Row - 1] == CellType.Empty;
                case LineType.Diagonal:
                    if (Id == MainDiagonal)
                    {
                        return move.Row == move.Column && cells[move.Column - 1] == CellType.Empty;
                    }
                    else
                    {
                        return move.Row + move.Column == 4 && cells[move.Column - 1] == CellType.Empty;
                    }
            }

            return false;
        }

        public void AddMove(Move move)
        {
            if (!AcceptsMove(move)) return;
            switch (Type)
            {
                case LineType.Row:
                    cells[move.Column - 1] = move.Type;
                    break;
                case LineType.Column:
                    cells[move.Row - 1] = move.Type;
                    break;
                case LineType.Diagonal:
                    cells[move.Column - 1] = move.Type;
                    break;
            }
        }

        public bool XWins => cells.SequenceEqual(XWin);
        public bool OWins => cells.SequenceEqual(OWin);
        public bool HasWin()
        {
            return XWins || OWins;
        }

        public bool IsFullMixed()
        {
            if (HasWin()) return false;
            return !cells.Any(p => p == CellType.Empty);
        }

        public CellType GetWinType()
        {
            if (HasWin())
            {
                return cells[0];
            }
            else
            {
                return CellType.Empty;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            switch (Type)
            {
                case LineType.Row:
                    sb.Append($"{Id}-");
                    break;
                case LineType.Column:
                    sb.Append($"{Id}|");
                    break;
                case LineType.Diagonal:
                    if (Id == MainDiagonal)
                    {
                        sb.Append($"*\\");
                    }
                    else
                    {
                        sb.Append($"*/");
                    }
                    break;
            }

            foreach (var content in cells)
            {
                switch (content)
                {
                    case CellType.Empty:
                        sb.Append(' ');
                        break;
                    case CellType.X:
                        sb.Append('X');
                        break;
                    case CellType.O:
                        sb.Append('O');
                        break;
                }
            }

            return sb.ToString();
        }

    }
}
