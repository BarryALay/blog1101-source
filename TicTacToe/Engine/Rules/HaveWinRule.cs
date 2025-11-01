using NRules.Fluent.Dsl;
using TicTacToe.Engine.Model;

namespace TicTacToe.Engine.Rules
{
    internal class HaveWinRule : Rule
    {
        public override void Define()
        {
            GameStage stage = default!;
            Move move = default!;
            IEnumerable<Line> lines = default!;

            When()
                .Match(() => stage, gameStage => gameStage.Stage == GameStageType.Active)
                .Match(() => move, move => move.Status == MoveStatus.Processed)
                .Query(() => lines, query => query
                    .Match<Line>(l => l.HasWin())
                    .Collect()
                    .Where(l => l.Any())
                )
                ;

            Then()
                .Do(ctx => stage.SetStage(GameStageType.Win))
                .Do(ctx => stage.SetWinner(lines.First().GetWinType()))
                .Do(ctx => ctx.Update(stage))
                ;
        }
    }
}
