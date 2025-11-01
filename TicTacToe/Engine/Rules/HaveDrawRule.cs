using NRules.Fluent.Dsl;
using TicTacToe.Engine.Model;

namespace TicTacToe.Engine.Rules
{
    internal class HaveDrawRule : Rule
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
                    .Match<Line>(l => l.IsFullMixed())
                    .Collect()
                    .Where(l => l.Count() == Line.MaxLines)
                )
                ;

            Then()
                .Do(ctx => stage.SetStage(GameStageType.Draw))
                .Do(ctx => stage.SetWinner(CellType.Empty))
                .Do(ctx => ctx.Update(stage))
                ;
        }
    }
}
