﻿namespace DummyMaze.Maze
{
	public class Game
	{
		//Configs

		private const int Width = 10;
		private const int Height = 10;

		private const int BlockFrequency = 20;
		private const int DefaultMovesCount = 20;

		private const char WallIcon = '#';
		private const char FreeAreaIcon = '.';

		private const char DogIcon = '@';
		private const char FinishIcon = 'O';
		private const char JetpackIcon = 'J';

		//////////////////////////////////////////////////

		private Random _random = new();

		private char[,] _map = new char[Width, Height];

		private int _deltaX = 0;
		private int _deltaY = 0;

		private int _dogX = 0;
		private int _dogY = 0;

		private int _finishX = 0;
		private int _finishY = 0;

		private int _jetpackX = 0;
		private int _jetpackY = 0;

		private bool _isWinGame = false;
		private bool _isReachedFinish = false;
		private bool _isTakenJetpack = false;

		private int _movesCount = DefaultMovesCount;

		public void Start()
		{
			Console.CursorVisible = false;

			Generate();

			Draw();

			while (!IsEndGame())
			{
				var delta = GetInput();

				_deltaX = delta.deltaX;
				_deltaY = delta.deltaY;

				Logic();

				Draw();
			}

			IsWinGame();

			DrawEndGame();		
		}

		private bool IsEndGame()
		{
			return _isReachedFinish || _movesCount <= 0;
		}

		private void IsReachedFinish()
		{
			_isReachedFinish = _dogX == _finishX && _dogY == _finishY;
		}

		private void IsWinGame()
		{
			_isWinGame = _isReachedFinish;
		}

		private void TryTakeJetpack()
		{
			if (!CanTakeJetpack())
			{
				return;
			}		

			_isTakenJetpack = true;

			_map[_jetpackX, _jetpackY] = FreeAreaIcon;
		}

		private bool CanTakeJetpack()
		{
			if (_isTakenJetpack || _dogX != _jetpackX || _dogY != _jetpackY)
			{
				return false;
			}

			if (_map[_jetpackX, _jetpackY] != JetpackIcon)
			{
				return false;
			}

			return true;
		}

		private void DecreaseMoves()
		{
			_movesCount--;
		}

		private (int deltaX, int deltaY) GetInput()
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

		private void Logic()
		{
			TryGoTo(_dogX + _deltaX, _dogY + _deltaY);

			TryTakeJetpack();

			DecreaseMoves();

			IsReachedFinish();
		}

		private void TryGoTo(int newX, int newY)
		{
			if (!CanGoTo(newX, newY))
			{
				return;
			}

			_dogX = newX;
			_dogY = newY;
		}

		private bool CanGoTo(int newX, int newY)
		{
			return IsWalkablePoint(newX, newY);
		}

		private bool IsWalkablePoint(int newX, int newY)
		{
			if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
			{
				return false;
			}

			if (_map[newX, newY] == WallIcon)
			{
				if (_isTakenJetpack)
				{
					_isTakenJetpack = false;

					return true;
				}

				return false;
			}

			return true;
		}

		private bool CanPlaceWall(int x, int y)
		{
			if (_map[x, y] == FinishIcon || _map[x, y] == JetpackIcon)
			{
				return false;
			}

			return true;
		}

		private void GenerateMaze()
		{
			_map = new char[Width, Height];

			PlaceFinish();

			PlaceJetpack();

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					if (!CanPlaceWall(j, i))
					{
						continue;
					}

					int randomNumber = _random.Next(0, 100);

					_map[j, i] = randomNumber < BlockFrequency ? WallIcon : FreeAreaIcon;
				}
			}
		}		
		
		private void PlaceDog()
		{
			var dogPosition = GetRandomPosition();

			_dogX = dogPosition.x;
			_dogY = dogPosition.y;
		}

		private void PlaceFinish()
		{
			var finishPosition = GetRandomPosition();

			_finishX = finishPosition.x;
			_finishY = finishPosition.y;

			_map[_finishX, _finishY] = FinishIcon;
		}

		private void PlaceJetpack()
		{
			var jetpackPosition = GetRandomPosition();

			_jetpackX = jetpackPosition.x;
			_jetpackY = jetpackPosition.y;

			_map[_jetpackX, _jetpackY] = JetpackIcon;
		}

		private (int x, int y) GetRandomPosition()
		{
			return (_random.Next(0, Width - 1), _random.Next(0, Height - 1));
		}

		private void Generate()
		{
			GenerateMaze();

			PlaceDog();
		}

		//////////////////////////////////////////////////

		private void Draw()
		{
			DrawMap();

			DrawStats();
		}

		private void DrawMap()
		{
			Console.Clear();

			for (int i = 0; i < Height; i++)
			{
				for (int j = 0; j < Width; j++)
				{
					if (j == _dogX && i == _dogY)
					{
						Console.Write(DogIcon);

						continue;
					}

					Console.Write(_map[j, i]);
				}

				Console.WriteLine();
			}
		}

		private void DrawStats()
		{
			Console.WriteLine($"\nWith Jetpack? : {_isTakenJetpack}");

			Console.WriteLine($"\nMoves left: {_movesCount}");
		}

		private void DrawEndGame()
		{
			Console.Clear();

			Console.WriteLine(_isWinGame ? "You won!" : "You lost!");
		}
	}
}