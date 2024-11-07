namespace DummyMaze
{
	public class Program
	{
		static Random random = new();

		static char[,] map = new char[width, height];
		static int width = 10;
		static int height = 12;

		static int blockFrequency = 20;

		static char wallIcon = '#';
		static char freeAreaIcon = '.';

		//dog

		static char dogIcon = '@';
		static int dogX = 0;
		static int dogY = 0;

		static int deltaX = 0;
		static int deltaY = 0;

		//finish

		static char finishIcon = 'O';
		static int finishX = 0;
		static int finishY = 0;

		static bool isReachedFinish = false;

		static void Main(string[] args)
		{
			Console.CursorVisible = false;

			Generate();

			DrawMap();

			while (!IsEndGame())
			{
				var delta = GetInput();

				deltaX = delta.deltaX;
				deltaY = delta.deltaY;

				Logic();

				DrawMap();
			}

			Console.Clear();

			Console.WriteLine("Nice");
		}

		static bool IsEndGame()
		{
			return isReachedFinish;
		}

		static (int deltaX, int deltaY) GetInput()
		{
			int deltaX = 0;
			int deltaY = 0;

			switch (Console.ReadKey().Key)
			{
				case ConsoleKey.UpArrow or ConsoleKey.W:
					deltaY = -1;
					break;
				case ConsoleKey.DownArrow or ConsoleKey.S:
					deltaY = 1;
					break;
				case ConsoleKey.LeftArrow or ConsoleKey.A:
					deltaX = -1;
					break;
				case ConsoleKey.RightArrow or ConsoleKey.D:
					deltaX = 1;
					break;
			}

			return (deltaX, deltaY);
		}

		static void Logic()
		{
			TryGoTo(dogX + deltaX, dogY + deltaY);

			IsReachedFinish();
		}

		static void IsReachedFinish()
		{
			isReachedFinish = dogX == finishX && dogY == finishY;
		}

		static void TryGoTo(int newX, int newY)
		{
			if (!CanGoTo(newX, newY))
			{
				return;
			}

			dogX = newX;
			dogY = newY;
		}

		static bool CanGoTo(int newX, int newY)
		{
			return IsWalkablePoint(newX, newY);
		}

		static bool IsWalkablePoint(int newX, int newY)
		{
			if (newX < 0 || newX >= width || newY < 0 || newY >= height)
			{
				return false;
			}

			if (map[newX, newY] != freeAreaIcon && map[newX, newY] != finishIcon)
			{
				return false;
			}

			return true;
		}

		static void GenerateMaze()
		{
			map = new char[width, height];

			PlaceFinish();

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (!CanPlaceWall(j, i))
					{
						continue;
					}

					int randomNumber = random.Next(0, 100);

					map[j, i] = randomNumber < blockFrequency ? wallIcon : freeAreaIcon;
				}
			}
		}

		static bool CanPlaceWall(int x, int y)
		{
			if (map[x, y] == finishIcon)
			{
				return false;
			}

			return true;
		}

		static void DrawMap()
		{
			Console.Clear();

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (j == dogX && i == dogY)
					{
						Console.Write(dogIcon);

						continue;
					}

					Console.Write(map[j, i]);
				}

				Console.WriteLine();
			}
		}

		static void PlaceDog()
		{
			var dogPosition = GetRandomPosition();

			dogX = dogPosition.x;
			dogY = dogPosition.y;
		}

		static void PlaceFinish()
		{
			var finishPosition = GetRandomPosition();

			finishX = finishPosition.x;
			finishY = finishPosition.y;

			map[finishX, finishY] = finishIcon;
		}

		static (int x, int y) GetRandomPosition()
		{
			return (random.Next(0, width - 1), random.Next(0, height - 1));
		}

		static void Generate()
		{
			GenerateMaze();

			PlaceDog();
		}
	}
}