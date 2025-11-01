using NRules.Fluent.Dsl;
using TicTacToe.Engine.Model;

namespace TicTacToe.Engine.Rules
{
    internal class DuplicateMoveRule : Rule
    {
        public override void Define()
        {
            GameStage stage = default!;
            Move move = default!;
            Grid grid = default!;

            When()
                .Match(() => stage, gameStage => gameStage.Stage == GameStageType.Active)
                .Match(() => move, m => m.Status == MoveStatus.Pending)
                .Or(x => x
                    .Match(() => grid, g => g[move.Row, move.Column] == CellType.X)
                    .Match(() => grid, g => g[move.Row, move.Column] == CellType.O)
                    )
                ;

            Then()
                .Do(ctx => move.SetStatus(MoveStatus.Error))
                .Do(ctx => ctx.Update(move))
                .Do(ctx => stage.SetStage(GameStageType.Error))
                .Do(ctx => stage.SetErrorType(GameStageErrorType.MoveInUse))
                .Do(ctx => ctx.Update(stage))
                ;
        }
    }
}
