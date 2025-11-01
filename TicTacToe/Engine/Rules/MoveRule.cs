using NRules.Fluent.Dsl;
using NRules.RuleModel;
using TicTacToe.Engine.Model;

namespace TicTacToe.Engine.Rules
{
    internal class MoveRule : Rule
    {
        public override void Define()
        {
            GameStage stage = default!;
            Grid grid = default!;
            Move move = default!;
            IEnumerable<Line> lines = default!;

            When()
                .Match(() => stage, gameStage => gameStage.Stage == GameStageType.Active)
                .Match(() => move, move => move.Status == MoveStatus.Pending)
//                .Match(() => grid, grid => grid[move.Row, move.Column] == CellType.Empty)
                .Match(() => grid, grid => TestGridCell(grid, move.Row, move.Column))
                .Query(() => lines, query => query
                    .Match<Line>(l => l.AcceptsMove(move))
                    .Collect()
                )
                ;

            Then()
                .Do(ctx => move.SetStatus(MoveStatus.Processed))
                .Do(ctx => ctx.Update(move))
                .Do(ctx => grid.SetCellType(move.Row, move.Column, move.Type))
                .Do(ctx => ctx.Update(grid))
                .Do(ctx => UpdateLines(ctx, lines, move))
                ;
        }

        private static bool TestGridCell(Grid grid, int row, int column)
        {
            bool result = grid[row, column] == CellType.Empty;

            Console.WriteLine($"..rule {nameof(MoveRule)}: test 'grid[row, column] == CellType.Empty' = {result}");

            return result;
        }

        private static void UpdateLines(IContext context, IEnumerable<Line> lines, Move move)
        {
            foreach (Line line in lines)
            {
                line.AddMove(move);
                context.Update(line);
            }
        }
    }
}
