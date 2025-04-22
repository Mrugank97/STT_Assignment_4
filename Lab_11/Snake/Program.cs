using System;
using System.Collections.Generic;

Exception? exception = null;        // Confirms the entry point — first line of the implicit Main()
int speedInput;
string prompt = $"Select speed [1], [2] (default), or [3]: ";
string? input;
Console.Write(prompt);      // When the speed prompt is shown
while (!int.TryParse(input = Console.ReadLine(), out speedInput) || speedInput < 1 || 3 < speedInput) //Input validation loop for speed
{
	if (string.IsNullOrWhiteSpace(input))
	{
		speedInput = 2;
		break;
	}
	else
	{
		Console.WriteLine("Invalid Input. Try Again...");
		Console.Write(prompt);
	}
}
int[] velocities = [100, 100, 100];   //  See which speed corresponds to which input (validates index logic).
int velocity = velocities[speedInput - 1];
char[] DirectionChars = ['^', 'v', '<', '>',];
TimeSpan sleep = TimeSpan.FromMilliseconds(velocity);
int width = Console.WindowWidth;
int height = Console.WindowHeight;
Tile[,] map = new Tile[width, height];
Direction? direction = null;
Queue<(int X, int Y)> snake = new();
(int X, int Y) = (width / 2, height / 2);
bool closeRequested = false;

try
{
	Console.CursorVisible = false;
	Console.Clear();
	snake.Enqueue((X, Y));      // First snake segment is placed
	map[X, Y] = Tile.Snake;
	PositionFood();     //First food placement
	Console.SetCursorPosition(X, Y);
	Console.Write('@');
	while (!direction.HasValue && !closeRequested)
	{
		GetDirection();
	}
	while (!closeRequested) // Game loop starts
	{
		if (Console.WindowWidth != width || Console.WindowHeight != height)
		{
			width = Console.WindowWidth;
			height = Console.WindowHeight;

			Console.Clear();

			// Optional: update map size if it's dynamically allocated
			map = new Tile[width, height];

			// Redraw snake
			foreach ((int x, int y) in snake)
			{
				if (x < width && y < height) // avoid out-of-bounds
				{
					Console.SetCursorPosition(x, y);
					Console.Write('@');
					map[x, y] = Tile.Snake;
				}
			}

			// Redraw food
			PositionFood();

			Console.SetCursorPosition(0, height - 1);
			Console.Write($"[Window resized] New Size: {width} x {height}");
		}
		switch (direction)      // Snake moves
		{
			case Direction.Up: Y--; break;
			case Direction.Down: Y++; break;
			case Direction.Left: X--; break;
			case Direction.Right: X++; break;
		}
		if (X < 0 || X >= width ||
			Y < 0 || Y >= height ||
			map[X, Y] is Tile.Snake)  // Checks if food was eaten
		{
			Console.Clear();
			Console.Write("Game Over. Score: " + (snake.Count - 1) + ".");      // Break here to inspect final snake state when player hits wall/self.
			return;
		}
		Console.SetCursorPosition(X, Y);
		Console.Write(DirectionChars[(int)direction!]);
		snake.Enqueue((X, Y));
		if (map[X, Y] is Tile.Food)
		{
			PositionFood();
		}
		else
		{
			(int x, int y) = snake.Dequeue();
			map[x, y] = Tile.Open;
			Console.SetCursorPosition(x, y);
			Console.Write(' ');
		}
		map[X, Y] = Tile.Snake;
		if (Console.KeyAvailable)       // Captures player input
		{
			GetDirection();
		}
		System.Threading.Thread.Sleep(sleep);
	}
}
catch (Exception e)
{
	exception = e;
	throw;
}
finally
{
	Console.CursorVisible = true;       // Cleanup after game ends
	Console.Clear();
	Console.WriteLine(exception?.ToString() ?? "Snake was closed.");
}

void GetDirection()
// takes direction from arrow keys
{
	switch (Console.ReadKey(true).Key)
	{
		case ConsoleKey.UpArrow: direction = Direction.Up; break;
		case ConsoleKey.DownArrow: direction = Direction.Down; break;
		case ConsoleKey.LeftArrow: direction = Direction.Left; break;
		case ConsoleKey.RightArrow: direction = Direction.Right; break;
		case ConsoleKey.Escape: closeRequested = true; break;
	}
}

void PositionFood()
{
	List<(int X, int Y)> possibleCoordinates = new();
	for (int i = 0; i < width; i++)
	{
		for (int j = 0; j < height; j++)
		{
			if (map[i, j] is Tile.Open)
			{
				possibleCoordinates.Add((i, j));
			}
		}
	}
	int index = Random.Shared.Next(possibleCoordinates.Count);
	(int X, int Y) = possibleCoordinates[index];
	map[X, Y] = Tile.Food;
	Console.SetCursorPosition(X, Y);      
	Console.Write('+');             // Useful to ensure food is being placed & visualized correctly.
}

enum Direction
{
	Up = 0,
	Down = 1,
	Left = 2,
	Right = 3,
}

enum Tile
{
	Open = 0,
	Snake,
	Food,
}
