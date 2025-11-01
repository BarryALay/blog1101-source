using NRules.Fluent.Dsl;
using NRules.RuleModel;
using TicTacToe.Engine.Model;

namespace TicTacToe.Engine.Rules
{
    internal class NewGameRule : Rule
    {
        public override void Define()
        {
            GameStage stage = default!;
            Grid grid = default!;
            IEnumerable<Line> lines = default!;

            When()
                .Match(() => stage, gameStage => gameStage.Stage == GameStageType.New)
                .Match(() => grid)
                .Query(() => lines, query => query.Match<Line>([]).Collect())
                ;

            Then()
                .Do(ctx => stage.SetStage(GameStageType.Active))
                .Do(ctx => ctx.Update(stage))
                .Do(ctx => ClearLines(ctx, lines))
                .Do(ctx => grid.Clear())
                .Do(ctx => ctx.Update(grid))
                ;
        }

        private static void ClearLines(IContext context, IEnumerable<Line> lines)
        {
            foreach (Line line in lines)
            {
                line.Clear();
                context.Update(line);
            }
        }
    }
}
