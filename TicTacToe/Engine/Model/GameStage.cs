namespace TicTacToe.Engine.Model
{
    internal enum GameStageType
    {
        Initial, New, Active, Win, Draw, Error
    }

    internal enum GameStageErrorType
    {
        None, MoveNotValid, MoveInUse
    }

    internal class GameStage(GameStageType stage)
    {
        public GameStageType Stage { get; set; } = stage;
        public CellType Winner { get; set; } = CellType.Empty;
        public GameStageErrorType Error { get; set; } = GameStageErrorType.None;

        public void SetStage(GameStageType newStage) => Stage = newStage;
        public void SetWinner(CellType winner) => Winner = winner;
        public void SetErrorType(GameStageErrorType error) => Error = error;
    }
}
