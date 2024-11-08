using DummyMaze.Maze;

namespace DummyMaze
{
	public class Boot
	{
		static void Main(string[] args)
		{
			do
			{
				Game game = new Game();

				game.Start();

				Console.WriteLine("Play again? (Y / N)");
			}
			while (Console.ReadKey(true).Key == ConsoleKey.Y);		
		}
	}
}