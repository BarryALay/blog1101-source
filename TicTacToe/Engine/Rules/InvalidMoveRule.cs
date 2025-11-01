using NRules.Fluent.Dsl;
using TicTacToe.Engine.Model;

namespace TicTacToe.Engine.Rules
{
    internal class InvalidMoveRule : Rule
    {
        public override void Define()
        {
            GameStage stage = default!;
            Move move = default!;
            Grid grid = default!;

            When()
                .Match(() => stage, gameStage => gameStage.Stage == GameStageType.Active)
                .Match(() => move, m => m.Status == MoveStatus.Pending)
                .Match(() => grid, g => g[move.Row, move.Column] == CellType.None);
                ;

            Then()
                .Do(ctx => move.SetStatus(MoveStatus.Error))
                .Do(ctx => ctx.Update(move))
                .Do(ctx => stage.SetStage(GameStageType.Error))
                .Do(ctx => stage.SetErrorType(GameStageErrorType.MoveNotValid))
                .Do(ctx => ctx.Update(stage))
                ;
        }
    }
}
