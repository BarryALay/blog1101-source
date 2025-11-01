namespace TicTacToe.Engine.Model
{
    internal enum MoveStatus
    {
        Pending, Processed, Error
    }

    internal class Move(CellType type, int row, int column)
    {
        public CellType Type { get; set; } = type;
        public MoveStatus Status { get; set; } = MoveStatus.Pending;

        /// <summary>
        /// Row number - 1 to 3
        /// </summary>
        public int Row { get; } = row;

        /// <summary>
        /// Column number - 1 to 3
        /// </summary>
        public int Column { get; } = column;

        public void SetStatus(MoveStatus status) => Status = status;
    }
}
