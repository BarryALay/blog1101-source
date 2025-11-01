using TicTacToe;

namespace TicTacToeTest
{
    [TestClass]
    public sealed class TestRules
    {
        [TestMethod]
        public void TestNewGame()
        {
            var api = new API();

            var rules = api.GetRules();
            Assert.IsGreaterThan(0, rules.Count);

            api.NewGame();

            var lines = api.GetLines();
            Assert.HasCount(8, lines);
        }

        [TestMethod]
        public void TestMove()
        {
            var api = new API();

            api.NewGame();
            api.Move(API.Side.X, 1, 1);
            var lines = api.GetLines();
            int count = lines.Where(l => l.Contains('X')).Count();
            Assert.IsGreaterThan(0, count);
        }

        [TestMethod]
        public void TestInvalidMove()
        {
            var api = new API();

            api.NewGame();
            api.Move(API.Side.X, 1, 4);

            var status = api.GetStatus();
            var errorType = api.GetErrorType();
            Assert.AreEqual(API.GameStatus.Error, status, "no error");
            Assert.AreEqual(API.ErrorType.MoveNotValid, errorType, "error type not correct");
        }

        [TestMethod]
        public void TestDuplicateMove()
        {
            var api = new API();

            api.NewGame();
            api.Move(API.Side.X, 1, 1);
            api.Move(API.Side.O, 1, 1);

            var status = api.GetStatus();
            var errorType = api.GetErrorType();
            Assert.AreEqual(API.GameStatus.Error, status, "no error");
            Assert.AreEqual(API.ErrorType.MoveInUse, errorType, "error type not correct");
        }

        [TestMethod]
        public void TestDuplicateMoveWithReset()
        {
            var api = new API();

            api.NewGame();
            api.Move(API.Side.X, 1, 1);
            api.Move(API.Side.O, 1, 1);

            var status1 = api.GetStatus();
            var errorType1 = api.GetErrorType();
            Assert.AreEqual(API.GameStatus.Error, status1, "no error");
            Assert.AreEqual(API.ErrorType.MoveInUse, errorType1, "error type not correct");

            api.ResetError();
            api.Move(API.Side.O, 1, 2);

            var status2 = api.GetStatus();
            Assert.AreEqual(API.GameStatus.Active, status2, "not active");
        }

        [TestMethod]
        public void TestRowWin()
        {
            var api = new API();

            api.NewGame();

            api.Move(API.Side.X, 1, 1);
            api.Move(API.Side.X, 1, 2);
            api.Move(API.Side.X, 1, 3);
            var lines = api.GetLines();

            var status = api.GetStatus();
            Assert.AreEqual(API.GameStatus.Win, status, "no winner");

            var winner = api.GetWinner();
            Assert.AreNotEqual(API.Side.None, winner, "no winner");
        }

        [TestMethod]
        public void TestColumnWin()
        {
            var api = new API();

            api.NewGame();

            api.Move(API.Side.X, 1, 2);
            api.Move(API.Side.X, 2, 2);
            api.Move(API.Side.X, 3, 2);
            var lines = api.GetLines();

            var status = api.GetStatus();
            Assert.AreEqual(API.GameStatus.Win, status, "no winner");

            var winner = api.GetWinner();
            Assert.AreNotEqual(API.Side.None, winner, "no winner");
        }

        [TestMethod]
        public void TestDiagonalWin()
        {
            var api = new API();

            api.NewGame();

            api.Move(API.Side.X, 1, 1);
            api.Move(API.Side.X, 2, 2);
            api.Move(API.Side.X, 3, 3);
            var lines = api.GetLines();

            var status = api.GetStatus();
            Assert.AreEqual(API.GameStatus.Win, status, "no winner");

            var winner = api.GetWinner();
            Assert.AreNotEqual(API.Side.None, winner, "no winner");
        }

        [TestMethod]
        public void TestSecondDiagonalWin()
        {
            var api = new API();

            api.NewGame();

            api.Move(API.Side.O, 1, 3);
            api.Move(API.Side.O, 2, 2);
            api.Move(API.Side.O, 3, 1);
            var lines = api.GetLines();

            var status = api.GetStatus();
            Assert.AreEqual(API.GameStatus.Win, status, "no winner");

            var winner = api.GetWinner();
            Assert.AreNotEqual(API.Side.None, winner, "no winner");
        }

        [TestMethod]
        public void TestDraw()
        {
            var api = new API();

            api.NewGame();

            //
            //   1  2  3 
            // 1 1X 6O 5X
            // 2 3X 2O 8O
            // 3 4O 7X 9X
            //

            api.Move(API.Side.X, 1, 1); // 1
            api.Move(API.Side.O, 2, 2); // 2
            api.Move(API.Side.X, 2, 1); // 3
            api.Move(API.Side.O, 3, 1); // 4
            api.Move(API.Side.X, 1, 3); // 5
            api.Move(API.Side.O, 1, 2); // 6
            api.Move(API.Side.X, 3, 2); // 7
            api.Move(API.Side.O, 2, 3); // 8
            api.Move(API.Side.X, 3, 3); // 9

            var lines = api.GetLines();

            var status = api.GetStatus();
            Assert.AreEqual(API.GameStatus.Draw, status, "not draw");

            var winner = api.GetWinner();
            Assert.AreEqual(API.Side.None, winner, "have winner");
        }

        [TestMethod]
        public void TestMultipleGames()
        {
            var api = new API();

            api.NewGame();

            api.Move(API.Side.X, 1, 1);
            api.Move(API.Side.X, 2, 2);
            api.Move(API.Side.X, 3, 3);
            var lines1 = api.GetLines();
            var grid1 = api.GetGrid();

            var status1 = api.GetStatus();
            Assert.AreEqual(API.GameStatus.Win, status1, "no winner");

            var winner1 = api.GetWinner();
            Assert.AreNotEqual(API.Side.None, winner1, "no winner");

            api.NewGame();
            var lines2 = api.GetLines();
            var grid2 = api.GetGrid();

            api.Move(API.Side.O, 1, 1);
            api.Move(API.Side.O, 2, 2);
            api.Move(API.Side.O, 3, 3);
            lines2 = api.GetLines();

            var status2 = api.GetStatus();
            Assert.AreEqual(API.GameStatus.Win, status2, "no winner");

            var winner2 = api.GetWinner();
            Assert.AreNotEqual(API.Side.None, winner2, "no winner");

        }
    }
}
