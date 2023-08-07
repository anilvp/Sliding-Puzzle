//Generate new puzzle boards until we get a solvable one
PuzzleBoard initialPuzzleBoard = new PuzzleBoard();
while (!initialPuzzleBoard.IsSolvable())
{
    initialPuzzleBoard = new PuzzleBoard();
}
initialPuzzleBoard.DisplayBoard();
//Perform depth first search algorithm to find the shortest path to the end board arrangement
List<PuzzleBoard> boardQueue;
PuzzleBoard? finalBoard = null;
boardQueue = initialPuzzleBoard.GetNeighbours();
int n = 0;
while (finalBoard == null)
{
    n += 1;
    for (int i = boardQueue.Count - 1; i >= 0; i--)
    {
        //Checks if final board is reached
        if (PuzzleBoard.CompareBoards(boardQueue[i].Board(), PuzzleBoard.END_BOARD))
        {
            finalBoard = boardQueue[i];
            break;
        }
        else
        {
            //Adds neighbours to the queue
            boardQueue.AddRange(boardQueue[i].GetNeighbours());
            boardQueue.RemoveAt(i);
        }
    }
}
//Output number of moves and sequence of moves
Console.WriteLine("Number of moves: " + n);
Console.WriteLine("Sequence of moves:");
List<PuzzleBoard> moveSequence = new List<PuzzleBoard>();
moveSequence.Add(finalBoard);
for (int i = 0; i < n; i++)
{
    moveSequence.Add(moveSequence[i].PreviousPuzzleBoard());
}
moveSequence.Reverse();
for (int i = 0; i < moveSequence.Count; i++)
{
    moveSequence[i].DisplayBoard();
}


class PuzzleBoard
{
    //The end board arrangement
    public static readonly int[,] END_BOARD = { { 1, 2, 3 },
                                                { 4, 5, 6 },
                                                { 7, 8, 0 } };
    public static readonly int[,] NULL_BOARD = { { 0, 0, 0 },
                                                 { 0, 0, 0 },
                                                 { 0, 0, 0 } };
    //Stores the board arrangement
    private int[,] board = new int[3, 3];
    //The puzzle board before the previous move
    private PuzzleBoard previousPuzzleBoard;
    //Coordinates of the blank (0) in this puzzle board
    private int blankRow, blankColumn;

    //Generates a random board arrangement
    public PuzzleBoard()
    {
        List<int> numbersRemaining = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        Random random = new Random();
        for (int i = 0; i < 9; i++)
        {
            int randomIndex = random.Next(numbersRemaining.Count);
            board[i / 3, i % 3] = numbersRemaining[randomIndex];
            if (numbersRemaining[randomIndex] == 0)
            {
                blankRow = i / 3;
                blankColumn = i % 3;
            }
            numbersRemaining.RemoveAt(randomIndex);
        }
        //Set to a non-null default value
        previousPuzzleBoard = new PuzzleBoard(NULL_BOARD, 2, 2, this);
    }

    //Creates an instance of PuzzleBoard with a given board arrangement
    public PuzzleBoard(int[,] givenBoard, int _blankRow, int _blankColumn, PuzzleBoard _previousPuzzleBoard)
    {
        board = givenBoard;
        blankRow = _blankRow;
        blankColumn = _blankColumn;
        previousPuzzleBoard = _previousPuzzleBoard;
    }

    //Prints the current board arrangement to the console
    public void DisplayBoard()
    {
        Console.WriteLine(board[0, 0] + " " + board[0, 1] + " " + board[0, 2]);
        Console.WriteLine(board[1, 0] + " " + board[1, 1] + " " + board[1, 2]);
        Console.WriteLine(board[2, 0] + " " + board[2, 1] + " " + board[2, 2]);
        Console.WriteLine();
    }

    //Checks if the current board arrangement is solvable
    public bool IsSolvable()
    {
        //Count the number of inversions in the current board arangement
        int numberOfInversions = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                if (board[i / 3, i % 3] == 0 || board[j / 3, j % 3] == 0)
                {
                    continue;
                }
                if (board[i / 3, i % 3] > board[j / 3, j % 3])
                {
                    numberOfInversions += 1;
                }
            }
        }
        //Puzzle is solvable if and only if the number of inversions is even
        if (numberOfInversions % 2 == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Returns a list of the puzzle boards that are obtainable after one move from this one (excluding the previous board)
    public List<PuzzleBoard> GetNeighbours()
    {
        //Works out neighbours based on the position of the blank tile in the board.
        //Ignores previous board from list of neighbours
        List<PuzzleBoard> neighbours = new List<PuzzleBoard>();
        if (blankRow == 0)
        {
            //Neighbour below
            int[,] newBoard = (int[,])board.Clone();
            newBoard[0, blankColumn] = board[1, blankColumn];
            newBoard[1, blankColumn] = 0;
            if (!CompareBoards(newBoard, previousPuzzleBoard.board))
            {
                PuzzleBoard neighbour = new PuzzleBoard(newBoard, 1, blankColumn, this);
                neighbours.Add(neighbour);
            }
        }
        else if (blankRow == 1)
        {
            //Neighbour below
            int[,] newBoard = (int[,])board.Clone();
            newBoard[1, blankColumn] = board[2, blankColumn];
            newBoard[2, blankColumn] = 0;
            if (!CompareBoards(newBoard, previousPuzzleBoard.board))
            {
                PuzzleBoard neighbour = new PuzzleBoard(newBoard, 2, blankColumn, this);
                neighbours.Add(neighbour);
            }
            //Neighbour above
            newBoard = (int[,])board.Clone();
            newBoard[1, blankColumn] = board[0, blankColumn];
            newBoard[0, blankColumn] = 0;
            if (!CompareBoards(newBoard, previousPuzzleBoard.board))
            {
                PuzzleBoard neighbour = new PuzzleBoard(newBoard, 0, blankColumn, this);
                neighbours.Add(neighbour);
            }
        }
        else //blankRow == 2
        {
            //Neighbour above
            int[,] newBoard = (int[,])board.Clone();
            newBoard[2, blankColumn] = board[1, blankColumn];
            newBoard[1, blankColumn] = 0;
            if (!CompareBoards(newBoard, previousPuzzleBoard.board))
            {
                PuzzleBoard neighbour = new PuzzleBoard(newBoard, 1, blankColumn, this);
                neighbours.Add(neighbour);
            }
        }

        if (blankColumn == 0)
        {
            //Neighbour on the right
            int[,] newBoard = (int[,])board.Clone();
            newBoard[blankRow, 0] = board[blankRow, 1];
            newBoard[blankRow, 1] = 0;
            if (!CompareBoards(newBoard, previousPuzzleBoard.board))
            {
                PuzzleBoard neighbour = new PuzzleBoard(newBoard, blankRow, 1, this);
                neighbours.Add(neighbour);
            }
        }
        else if (blankColumn == 1)
        {
            //Neighbour on the right
            int[,] newBoard = (int[,])board.Clone();
            newBoard[blankRow, 1] = board[blankRow, 2];
            newBoard[blankRow, 2] = 0;
            if (!CompareBoards(newBoard, previousPuzzleBoard.board))
            {
                PuzzleBoard neighbour = new PuzzleBoard(newBoard, blankRow, 2, this);
                neighbours.Add(neighbour);
            }
            //Neighbour on the left
            newBoard = (int[,])board.Clone();
            newBoard[blankRow, 1] = board[blankRow, 0];
            newBoard[blankRow, 0] = 0;
            if (!CompareBoards(newBoard, previousPuzzleBoard.board))
            {
                PuzzleBoard neighbour = new PuzzleBoard(newBoard, blankRow, 0, this);
                neighbours.Add(neighbour);
            }
        }
        else //blankColumn == 2
        {
            //Neighbour on the left
            int[,] newBoard = (int[,])board.Clone();
            newBoard[blankRow, 2] = board[blankRow, 1];
            newBoard[blankRow, 1] = 0;
            if (!CompareBoards(newBoard, previousPuzzleBoard.board))
            {
                PuzzleBoard neighbour = new PuzzleBoard(newBoard, blankRow, 1, this);
                neighbours.Add(neighbour);
            }
        }
        return neighbours;
    }

    public int[,] Board() { return board; }
    public PuzzleBoard PreviousPuzzleBoard() { return previousPuzzleBoard; }

    //Equality operator for boards (assumes input arrays are 3x3 for computational speed)
    public static bool CompareBoards(int[,] board1, int[,] board2)
    {
        //Check array elements match
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board1[i, j] != board2[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }
}