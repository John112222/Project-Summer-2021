using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class MazeAnimate : MonoBehaviour
{
	public enum MazeGenerationAlgorithm
	{
//		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.RecursiveTree;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;

	public GameObject UnVistedFloor = null;
	public GameObject CurrentVistFloor = null;
	public GameObject VistedFloor = null;

	public GameObject Wall = null;

	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;

	[Header("Generate Maze Animation Control")]
	public int waitTime = 200;
	public int buildTime = 0;

	private BasicMazeGeneratorAnim mMazeGenerator = null;

	public void ResetMaze()
	{
		var children = new List<GameObject>();
		foreach (Transform child in transform) children.Add(child.gameObject);
		children.ForEach(child => DestroyImmediate(child));

		transform.position = Vector3.zero;
	}

	async public void CreateMaze()
	{
		if (!FullRandom)
		{
			Random.InitState(RandomSeed);
		}
		switch (Algorithm)
		{
			//case MazeGenerationAlgorithm.PureRecursive:
			//	//mMazeGenerator = new RecursiveMazeGeneratorAnim(Rows, Columns);
			//	//break;
			//	throw new UnityException("Pure Recursive Alogrithm does not yet work");
			case MazeGenerationAlgorithm.RecursiveTree:
				mMazeGenerator = new RecursiveTreeMazeGeneratorAnim(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RandomTree:
				mMazeGenerator = new RandomTreeMazeGeneratorAnim(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.OldestTree:
				mMazeGenerator = new OldestTreeMazeGeneratorAnim(Rows, Columns);
				break;
			case MazeGenerationAlgorithm.RecursiveDivision:
				mMazeGenerator = new DivisionMazeGeneratorAnim(Rows, Columns);
				break;
		}

		bool[,] visted = new bool[Rows, Columns];
		for (int row = 0; row < Rows; row++)
		{
			for (int column = 0; column < Columns; column++)
			{
				visted[row, column] = false;
				float x = column * (CellWidth + (AddGaps ? .2f : 0));
				float z = row * (CellHeight + (AddGaps ? .2f : 0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
				GameObject tmp, flr;
				flr = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
				flr.transform.parent = transform;
				tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
				tmp.transform.parent = flr.transform;
				tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
				tmp.transform.parent = flr.transform;
				tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
				tmp.transform.parent = transform;
				tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
				tmp.transform.parent = flr.transform;
			}
		}

		MazeCell[,] final = null;
		await Task.Delay(waitTime*2);

		Debug.Log("Starting Maze Generator");

		foreach (MazeCell[,] snapshot in mMazeGenerator.YieldGenerateMaze())
		{
			final = snapshot;
			await Task.Delay(waitTime);
			ResetMaze();
			for (int row = 0; row < Rows; row++)
			{
				for (int column = 0; column < Columns; column++)
				{
					float x = column * (CellWidth + (AddGaps ? .2f : 0));
					float z = row * (CellHeight + (AddGaps ? .2f : 0));

					MazeCell cell = snapshot[row, column];
					GameObject tmp, flr = null;
                    if (cell.IsVisited)
                    {
                        if (VistedFloor != null && visted[row,column])
                        {
							flr = Instantiate(VistedFloor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
							flr.transform.parent = transform;
						}
						else if (CurrentVistFloor != null)
						{
							flr = Instantiate(CurrentVistFloor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
							flr.transform.parent = transform;
							visted[row, column] = true;
						}
						else
                        {
							flr = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
							flr.transform.parent = transform;
						}
					}
                    else
                    {
						if (UnVistedFloor != null)
                        {
							flr = Instantiate(UnVistedFloor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
							flr.transform.parent = transform;
						}
                        else
                        {
							flr = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
							flr.transform.parent = transform;
						}
					}
					
                    if (cell.IsVisited)
                    {
						if (cell.WallRight)
						{
							tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
							tmp.transform.parent = flr?.transform;
						}
						if (cell.WallFront)
						{
							tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
							tmp.transform.parent = flr?.transform;
						}
						if (cell.WallLeft)
						{
							tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
							tmp.transform.parent = flr?.transform; ;
						}
						if (cell.WallBack)
						{
							tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
							tmp.transform.parent = flr?.transform; ;
						}
					}
                    else
                    {
						tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
						tmp.transform.parent = flr.transform;
						tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
						tmp.transform.parent = flr.transform;
						tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
						tmp.transform.parent = transform;
						tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
						tmp.transform.parent = flr.transform;
					}	
					await Task.Delay(buildTime);
				}
			}
		}

		if (final == null)
        {
			throw new MissingReferenceException("Final snapshot of Maze generated not found!");
        }
        else
        {
			ResetMaze();
		}

		for (int row = 0; row < Rows; row++)
		{
			for (int column = 0; column < Columns; column++)
			{
				float x = column * (CellWidth + (AddGaps ? .2f : 0));
				float z = row * (CellHeight + (AddGaps ? .2f : 0));

				MazeCell cell = final[row, column];
				GameObject tmp, flr;
				flr = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
				flr.transform.parent = transform;
				if (cell.WallRight)
				{
					tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
					tmp.transform.parent = flr.transform;
				}
				if (cell.WallFront)
				{
					tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
					tmp.transform.parent = flr.transform;
				}
				if (cell.WallLeft)
				{
					tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
					tmp.transform.parent = flr.transform; ;
				}
				if (cell.WallBack)
				{
					tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
					tmp.transform.parent = flr.transform; ;
				}
			}
		}


		Debug.Log("Finished Generating Maze");
	}
}


#region MazeAnimClasses
/// <summary>
/// Basic Maze Generate that gives snapshots
/// </summary>
public abstract class BasicMazeGeneratorAnim
{
	public int RowCount { get { return mMazeRows; } }
	public int ColumnCount { get { return mMazeColumns; } }

	private int mMazeRows;
	private int mMazeColumns;
	private MazeCell[,] mMaze;

	public bool slowMazeGenerate = false;

	public BasicMazeGeneratorAnim(int rows, int columns)
	{
		mMazeRows = Mathf.Abs(rows);
		mMazeColumns = Mathf.Abs(columns);
		if (mMazeRows == 0)
		{
			mMazeRows = 1;
		}
		if (mMazeColumns == 0)
		{
			mMazeColumns = 1;
		}
		mMaze = new MazeCell[rows, columns];
		for (int row = 0; row < rows; row++)
		{
			for (int column = 0; column < columns; column++)
			{
				mMaze[row, column] = new MazeCell();
			}
		}
	}

	public abstract IEnumerable<MazeCell[,]> YieldGenerateMaze();

	public MazeCell GetMazeCell(int row, int column)
	{
		if (row >= 0 && column >= 0 && row < mMazeRows && column < mMazeColumns)
		{
			return mMaze[row, column];
		}
		else
		{
			Debug.Log(row + " " + column);
			throw new System.ArgumentOutOfRangeException();
		}
	}

	protected void SetMazeCell(int row, int column, MazeCell cell)
	{
		if (row >= 0 && column >= 0 && row < mMazeRows && column < mMazeColumns)
		{
			mMaze[row, column] = cell;
		}
		else
		{
			throw new System.ArgumentOutOfRangeException();
		}
	}

	protected MazeCell[,] GetCurrentMaze()
    {
		return mMaze;

	}
}

/// <summary>
/// Pure recursive maze generation that gives snapshots.
/// </summary>
public class RecursiveMazeGeneratorAnim : BasicMazeGeneratorAnim
{

	public RecursiveMazeGeneratorAnim(int rows, int columns) : base(rows, columns)
	{

	}

    public override IEnumerable<MazeCell[,]> YieldGenerateMaze()
	{
		foreach(MazeCell[,] val in VisitCell(0, 0, Direction.Start))
        {
			yield return val;

		}
	}

	private IEnumerable<MazeCell[,]> VisitCell(int row, int column, Direction moveMade)
	{
		Direction[] movesAvailable = new Direction[4];
		int movesAvailableCount = 0;
		yield return GetCurrentMaze();
		do
		{
			movesAvailableCount = 0;

			//check move right
			if (column + 1 < ColumnCount && !GetMazeCell(row, column + 1).IsVisited)
			{
				movesAvailable[movesAvailableCount] = Direction.Right;
				movesAvailableCount++;
			}
			else if (!GetMazeCell(row, column).IsVisited && moveMade != Direction.Left)
			{
				GetMazeCell(row, column).WallRight = true;
			}
			//check move forward
			if (row + 1 < RowCount && !GetMazeCell(row + 1, column).IsVisited)
			{
				movesAvailable[movesAvailableCount] = Direction.Front;
				movesAvailableCount++;
			}
			else if (!GetMazeCell(row, column).IsVisited && moveMade != Direction.Back)
			{
				GetMazeCell(row, column).WallFront = true;
			}
			//check move left
			if (column > 0 && column - 1 >= 0 && !GetMazeCell(row, column - 1).IsVisited)
			{
				movesAvailable[movesAvailableCount] = Direction.Left;
				movesAvailableCount++;
			}
			else if (!GetMazeCell(row, column).IsVisited && moveMade != Direction.Right)
			{
				GetMazeCell(row, column).WallLeft = true;
			}
			//check move backward
			if (row > 0 && row - 1 >= 0 && !GetMazeCell(row - 1, column).IsVisited)
			{
				movesAvailable[movesAvailableCount] = Direction.Back;
				movesAvailableCount++;
			}
			else if (!GetMazeCell(row, column).IsVisited && moveMade != Direction.Front)
			{
				GetMazeCell(row, column).WallBack = true;
			}

			if (movesAvailableCount == 0 && !GetMazeCell(row, column).IsVisited)
			{
				GetMazeCell(row, column).IsGoal = true;
			}

			GetMazeCell(row, column).IsVisited = true;

			if (movesAvailableCount > 0)
			{
				switch (movesAvailable[Random.Range(0, movesAvailableCount)])
				{
					case Direction.Start:
						break;
					case Direction.Right:
						VisitCell(row, column + 1, Direction.Right);
						break;
					case Direction.Front:
						VisitCell(row + 1, column, Direction.Front);
						break;
					case Direction.Left:
						VisitCell(row, column - 1, Direction.Left);
						break;
					case Direction.Back:
						VisitCell(row - 1, column, Direction.Back);
						break;
				}
			}
			yield return GetCurrentMaze();
		} while (movesAvailableCount > 0);
		yield return GetCurrentMaze();
	}
}

//<summary>
//Maze generation by dividing area in two, adding spaces in walls and repeating recursively.
//Non-recursive realisation of algorithm.
//Yields snapshots
//</summary>
public class DivisionMazeGeneratorAnim : BasicMazeGeneratorAnim
{

	public DivisionMazeGeneratorAnim(int row, int column) : base(row, column)
	{

	}

	//<summary>
	//Class representing maze area to be divided
	//</summary>
	private struct IntRect
	{
		public int left;
		public int right;
		public int top;
		public int bottom;

		public override string ToString()
		{
			return string.Format("[IntRect {0}-{1} {2}-{3}]", left, right, bottom, top);
		}
	}

	private System.Collections.Generic.Queue<IntRect> rectsToDivide = new System.Collections.Generic.Queue<IntRect>();

	public override IEnumerable<MazeCell[,]> YieldGenerateMaze()
	{
		yield return GetCurrentMaze();
		for (int row = 0; row < RowCount; row++)
		{
			GetMazeCell(row, 0).WallLeft = true;
			//GetMazeCell(row, 0).IsVisited = true;

			GetMazeCell(row, ColumnCount - 1).WallRight = true;
			//GetMazeCell(row, ColumnCount - 1).IsVisited = true;

			//yield return GetCurrentMaze();
		}
		for (int column = 0; column < ColumnCount; column++)
		{
			GetMazeCell(0, column).WallBack = true;
			//GetMazeCell(0, column).IsVisited = true;

			GetMazeCell(RowCount - 1, column).WallFront = true;
			//GetMazeCell(RowCount - 1, column).IsVisited = true;

			//yield return GetCurrentMaze();
		}

		rectsToDivide.Enqueue(new IntRect() { left = 0, right = ColumnCount, bottom = 0, top = RowCount });
		while (rectsToDivide.Count > 0)
		{
			IntRect currentRect = rectsToDivide.Dequeue();
			int width = currentRect.right - currentRect.left;
			int height = currentRect.top - currentRect.bottom;
			if (width > 1 && height > 1)
			{
				if (width > height)
				{
					divideVertical(currentRect);
				}
				else if (height > width)
				{
					divideHorizontal(currentRect);
				}
				else if (height == width)
				{
					if (Random.Range(0, 100) > 42)
					{
						divideVertical(currentRect);
					}
					else
					{
						divideHorizontal(currentRect);
					}
				}
			}
			else if (width > 1 && height <= 1)
			{
				divideVertical(currentRect);
			}
			else if (width <= 1 && height > 1)
			{
				divideHorizontal(currentRect);
			}
			yield return GetCurrentMaze();
		}
		yield return GetCurrentMaze();
	}

	//<summary>
	//Divides selected area vertically
	//</summary>
	private void divideVertical(IntRect rect)
	{
		int divCol = Random.Range(rect.left, rect.right - 1);
		for (int row = rect.bottom; row < rect.top; row++)
		{
			GetMazeCell(row, divCol).WallRight = true;
			GetMazeCell(row, divCol).IsVisited = true;

			GetMazeCell(row, divCol + 1).WallLeft = true;
			GetMazeCell(row, divCol + 1).IsVisited = true;

		}
		int space = Random.Range(rect.bottom, rect.top);
		GetMazeCell(space, divCol).WallRight = false;
		GetMazeCell(space, divCol).IsVisited = true;

		if (divCol + 1 < rect.right)
		{
			GetMazeCell(space, divCol + 1).WallLeft = false;
			GetMazeCell(space, divCol + 1).IsVisited = true;

		}
		rectsToDivide.Enqueue(new IntRect() { left = rect.left, right = divCol + 1, bottom = rect.bottom, top = rect.top });
		rectsToDivide.Enqueue(new IntRect() { left = divCol + 1, right = rect.right, bottom = rect.bottom, top = rect.top });
	}

	//<summary>
	//Divides selected area horiszontally
	//</summary>
	private void divideHorizontal(IntRect rect)
	{
		int divRow = Random.Range(rect.bottom, rect.top - 1);
		for (int col = rect.left; col < rect.right; col++)
		{
			GetMazeCell(divRow, col).WallFront = true;
			GetMazeCell(divRow, col).IsVisited = true;

			GetMazeCell(divRow + 1, col).WallBack = true;
			GetMazeCell(divRow + 1, col).IsVisited = true;

		}
		int space = Random.Range(rect.left, rect.right);
		GetMazeCell(divRow, space).WallFront = false;
		GetMazeCell(divRow, space).IsVisited = true;

		if (divRow + 1 < rect.top)
		{
			GetMazeCell(divRow + 1, space).WallBack = false;
			GetMazeCell(divRow + 1, space).IsVisited = true;

		}
		rectsToDivide.Enqueue(new IntRect() { left = rect.left, right = rect.right, bottom = rect.bottom, top = divRow + 1 });
		rectsToDivide.Enqueue(new IntRect() { left = rect.left, right = rect.right, bottom = divRow + 1, top = rect.top });
	}
}


/// <summary>
/// Basic Tree Maze Generate that gives snapshots
/// </summary>
public abstract class TreeMazeGeneratorAnim : BasicMazeGeneratorAnim
{

	//<summary>
	//Class representation of target cell
	//</summary>
	private struct CellToVisit
	{
		public int Row;
		public int Column;
		public Direction MoveMade;

		public CellToVisit(int row, int column, Direction move)
		{
			Row = row;
			Column = column;
			MoveMade = move;
		}

		public override string ToString()
		{
			return string.Format("[MazeCell {0} {1}]", Row, Column);
		}
	}

	System.Collections.Generic.List<CellToVisit> mCellsToVisit = new System.Collections.Generic.List<CellToVisit>();

	public TreeMazeGeneratorAnim(int row, int column) : base(row, column)
	{

	}

	public override IEnumerable<MazeCell[,]> YieldGenerateMaze()
	{
		Direction[] movesAvailable = new Direction[4];
		int movesAvailableCount = 0;
		mCellsToVisit.Add(new CellToVisit(Random.Range(0, RowCount), Random.Range(0, ColumnCount), Direction.Start));
		yield return GetCurrentMaze();
		while (mCellsToVisit.Count > 0)
		{
			movesAvailableCount = 0;
			CellToVisit ctv = mCellsToVisit[GetCellInRange(mCellsToVisit.Count - 1)];

			//check move right
			if (ctv.Column + 1 < ColumnCount && !GetMazeCell(ctv.Row, ctv.Column + 1).IsVisited && !IsCellInList(ctv.Row, ctv.Column + 1))
			{
				movesAvailable[movesAvailableCount] = Direction.Right;
				movesAvailableCount++;
			}
			else if (!GetMazeCell(ctv.Row, ctv.Column).IsVisited && ctv.MoveMade != Direction.Left)
			{
				GetMazeCell(ctv.Row, ctv.Column).WallRight = true;
				if (ctv.Column + 1 < ColumnCount)
				{
					GetMazeCell(ctv.Row, ctv.Column + 1).WallLeft = true;
				}
			}
			//check move forward
			if (ctv.Row + 1 < RowCount && !GetMazeCell(ctv.Row + 1, ctv.Column).IsVisited && !IsCellInList(ctv.Row + 1, ctv.Column))
			{
				movesAvailable[movesAvailableCount] = Direction.Front;
				movesAvailableCount++;
			}
			else if (!GetMazeCell(ctv.Row, ctv.Column).IsVisited && ctv.MoveMade != Direction.Back)
			{
				GetMazeCell(ctv.Row, ctv.Column).WallFront = true;
				if (ctv.Row + 1 < RowCount)
				{
					GetMazeCell(ctv.Row + 1, ctv.Column).WallBack = true;
				}
			}
			//check move left
			if (ctv.Column > 0 && ctv.Column - 1 >= 0 && !GetMazeCell(ctv.Row, ctv.Column - 1).IsVisited && !IsCellInList(ctv.Row, ctv.Column - 1))
			{
				movesAvailable[movesAvailableCount] = Direction.Left;
				movesAvailableCount++;
			}
			else if (!GetMazeCell(ctv.Row, ctv.Column).IsVisited && ctv.MoveMade != Direction.Right)
			{
				GetMazeCell(ctv.Row, ctv.Column).WallLeft = true;
				if (ctv.Column > 0 && ctv.Column - 1 >= 0)
				{
					GetMazeCell(ctv.Row, ctv.Column - 1).WallRight = true;
				}
			}
			//check move backward
			if (ctv.Row > 0 && ctv.Row - 1 >= 0 && !GetMazeCell(ctv.Row - 1, ctv.Column).IsVisited && !IsCellInList(ctv.Row - 1, ctv.Column))
			{
				movesAvailable[movesAvailableCount] = Direction.Back;
				movesAvailableCount++;
			}
			else if (!GetMazeCell(ctv.Row, ctv.Column).IsVisited && ctv.MoveMade != Direction.Front)
			{
				GetMazeCell(ctv.Row, ctv.Column).WallBack = true;
				if (ctv.Row > 0 && ctv.Row - 1 >= 0)
				{
					GetMazeCell(ctv.Row - 1, ctv.Column).WallFront = true;
				}
			}

			if (!GetMazeCell(ctv.Row, ctv.Column).IsVisited && movesAvailableCount == 0)
			{
				GetMazeCell(ctv.Row, ctv.Column).IsGoal = true;
			}

			GetMazeCell(ctv.Row, ctv.Column).IsVisited = true;

			if (movesAvailableCount > 0)
			{
				switch (movesAvailable[Random.Range(0, movesAvailableCount)])
				{
					case Direction.Start:
						break;
					case Direction.Right:
						mCellsToVisit.Add(new CellToVisit(ctv.Row, ctv.Column + 1, Direction.Right));
						break;
					case Direction.Front:
						mCellsToVisit.Add(new CellToVisit(ctv.Row + 1, ctv.Column, Direction.Front));
						break;
					case Direction.Left:
						mCellsToVisit.Add(new CellToVisit(ctv.Row, ctv.Column - 1, Direction.Left));
						break;
					case Direction.Back:
						mCellsToVisit.Add(new CellToVisit(ctv.Row - 1, ctv.Column, Direction.Back));
						break;
				}
			}
			else
			{
				mCellsToVisit.Remove(ctv);
			}
			yield return GetCurrentMaze();
		}
		yield return GetCurrentMaze();
	}

	private bool IsCellInList(int row, int column)
	{
		return mCellsToVisit.FindIndex((other) => other.Row == row && other.Column == column) >= 0;
	}

	//<summary>
	//Abstract method for cell selection strategy
	//</summary>
	protected abstract int GetCellInRange(int max);
}

public class OldestTreeMazeGeneratorAnim : TreeMazeGeneratorAnim
{

	public OldestTreeMazeGeneratorAnim(int row, int column) : base(row, column)
	{

	}

	protected override int GetCellInRange(int max)
	{
		return 0;
	}
}

public class RandomTreeMazeGeneratorAnim : TreeMazeGeneratorAnim
{

	public RandomTreeMazeGeneratorAnim(int row, int column) : base(row, column)
	{

	}

	protected override int GetCellInRange(int max)
	{
		return Random.Range(0, max + 1);
	}
}

public class RecursiveTreeMazeGeneratorAnim : TreeMazeGeneratorAnim
{

	public RecursiveTreeMazeGeneratorAnim(int row, int column) : base(row, column)
	{

	}

	protected override int GetCellInRange(int max)
	{
		return max;
	}
}

#endregion

