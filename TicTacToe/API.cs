using NRules;
using NRules.Diagnostics;
using NRules.Fluent;
using System.Reflection;
using System.Text;
using TicTacToe.Engine.Model;

namespace TicTacToe
{
    public class API
    {
        private readonly RuleRepository ruleRepository;
        private readonly ISessionFactory sessionFactory;
        private readonly ISession session;
        private readonly GameStage gameStage;
        private readonly Grid grid;

        public void NewGame()
        {
            gameStage.SetStage(GameStageType.New);
            session.Update(gameStage);
            session.Fire();
        }

        public enum Side { X, O, None };
        public void Move(Side side, int row, int column)
        {
            if (side == Side.None) return;
            CellType type = side switch
            {
                Side.X => CellType.X,
                Side.O => CellType.O,
                _ => CellType.Empty,
            };
            var move = new Move(type, row, column);
            session.Insert(move);
            session.Fire();
        }

        public void ResetError()
        {
            if (gameStage.Stage != GameStageType.Error) return;
            gameStage.SetStage(GameStageType.Active);
            session.Update(gameStage);
            session.Fire();
        }

        public List<string> GetRules()
        {
            List<string> rules = [];
            foreach (var rule in ruleRepository.GetRules())
            {
                rules.Add(rule.Name);
            }
            return rules;
        }

        public List<string> GetLines()
        {
            List<string> lines = [];
            foreach (var line in session.Query<Line>())
            {
                lines.Add(line.ToString());
            }
            return lines;
        }

        public List<string> GetGrid()
        {
            List<string> gridLines = [];
            for (int row = Grid.MinRow; row <= Grid.MaxRow; row++)
            {
                StringBuilder sb = new();
                for (int column = Grid.MinColumn; column <= Grid.MaxColumn; column++)
                {
                    sb.Append(grid[row, column] switch { CellType.Empty => " ", CellType.X => "X", CellType.O => "O", _ => "*" });
                }
                gridLines.Add(sb.ToString());
            }
            return gridLines;
        }

        public enum GameStatus { None, Active, Win, Draw, Error };
        public GameStatus GetStatus()
        {
            return gameStage.Stage switch
            {
                GameStageType.Active => GameStatus.Active,
                GameStageType.Win => GameStatus.Win,
                GameStageType.Draw => GameStatus.Draw,
                GameStageType.Error => GameStatus.Error,
                _ => GameStatus.None,
            };
        }

        public enum ErrorType { None, MoveNotValid, MoveInUse };
        public ErrorType GetErrorType()
        {
            return gameStage.Error switch
            {
                GameStageErrorType.MoveNotValid => ErrorType.MoveNotValid,
                GameStageErrorType.MoveInUse => ErrorType.MoveInUse,
                _ => ErrorType.None,
            };
        }

        public Side GetWinner()
        {
            if (gameStage.Stage == GameStageType.Win)
            {
                return gameStage.Winner switch
                {
                    CellType.X => Side.X,
                    CellType.O => Side.O,
                    _ => Side.None,
                };
            }
            else
            {
                return Side.None;
            }
        }

        private void RuleFiring(Object? obj, AgendaEventArgs args)
        {
            Console.WriteLine($"..rule {args.Rule.Name} firing");
        }

        private void RuleFired(Object? obj, AgendaEventArgs args)
        {
            Console.WriteLine($"..rule {args.Rule.Name} fired");
        }

        public API()
        {
            ruleRepository = new RuleRepository();
            ruleRepository.Load(x => x.PrivateTypes(true).From(Assembly.GetExecutingAssembly()));
            sessionFactory = ruleRepository.Compile();
            session = sessionFactory.CreateSession();

            // initialize model

            gameStage = new GameStage(GameStageType.Initial);
            session.Insert(gameStage);

            grid = new Grid();
            session.Insert(grid);

            foreach (int item in Enumerable.Range(1, 3))
            {
                session.Insert(new Line(LineType.Row, item));
                session.Insert(new Line(LineType.Column, item));
            }
            session.Insert(new Line(LineType.Diagonal, Line.MainDiagonal));
            session.Insert(new Line(LineType.Diagonal, Line.CounterDiagonal));

            session.Events.RuleFiringEvent += RuleFiring;
            session.Events.RuleFiredEvent += RuleFired;
        }
    }
}
