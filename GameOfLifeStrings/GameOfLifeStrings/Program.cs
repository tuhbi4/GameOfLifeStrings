using System.Text;

namespace GameOfLifeStrings
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(CodingChallenge(Console.ReadLine()));
            Console.WriteLine(CodingChallenge("000_111_000"));
        }

        public static string CodingChallenge(string str)
        {
            ValidateInputString(str);

            var matrix = GetValidMatrix(str);

            var rowsCount = matrix.Length;
            var columnsCount = matrix.First().Length;

            var outputStringBuilder = new StringBuilder();

            for (var rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    var targetCell = matrix[rowIndex][columnIndex];
                    var isAlive = targetCell == '1';

                    var liveNeighbours = GetLiveNeighbours(matrix, rowIndex, columnIndex);

                    outputStringBuilder.Append(isAlive switch
                    {
                        true when liveNeighbours is 2 or 3 => "1",
                        true => "0",
                        false when liveNeighbours is 3 => "1",
                        _ => targetCell
                    });
                }

                if (rowIndex != rowsCount - 1)
                {
                    outputStringBuilder.Append("_");
                }
            }

            return outputStringBuilder.ToString();
        }

        private static void ValidateInputString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str), "The input string can't be null or empty.");
            }

            if (str.Distinct().Any(c => c != '0' && c != '1' && c != '_'))
            {
                throw new ArgumentException("The input string must contain only '1' or '0' or '_' symbols.", nameof(str));
            }
        }

        private static char[][] GetValidMatrix(string str)
        {
            var separatedRows = str.Split('_');

            if (separatedRows.Any(row => row.Length != separatedRows.First().Length))
            {
                throw new ArgumentException("The input string must contain substrings of equal length.", nameof(str));
            }

            return separatedRows.Select(s => s.ToArray()).ToArray();
        }

        private static int GetLiveNeighbours(char[][] matrix, int targetRowIndex, int targetColumnIndex)
        {
            return GetNeighbours(matrix, targetRowIndex, targetColumnIndex).Count(x => x == '1');
        }

        private static string GetNeighbours(char[][] matrix, int targetRowIndex, int targetColumnIndex)
        {
            var rowsCount = matrix.Length;
            var columnsCount = matrix[0].Length;

            var neighbours = new StringBuilder();

            var (startRowIndex, endRowIndex, startColumnIndex, endColumnIndex) = GetMinMaxIndexes(rowsCount, columnsCount, targetRowIndex, targetColumnIndex);

            for (var rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
            {
                for (var columnIndex = startColumnIndex; columnIndex <= endColumnIndex; columnIndex++)
                {
                    if (rowIndex != targetRowIndex || columnIndex != targetColumnIndex)
                    {
                        neighbours.Append(matrix[rowIndex][columnIndex]);
                    }
                }
            }

            return neighbours.ToString();
        }

        private static (int startRowIndex, int endRowIndex, int startColumnIndex, int endColumnIndex) GetMinMaxIndexes(
             int rowsCount, int columnsCount, int targetRowIndex, int targetColumnIndex)
        {
            var startRowIndex = Math.Max(0, targetRowIndex - 1);
            var endRowIndex = Math.Min(rowsCount - 1, targetRowIndex + 1);
            var startColumnIndex = Math.Max(0, targetColumnIndex - 1);
            var endColumnIndex = Math.Min(columnsCount - 1, targetColumnIndex + 1);

            return (startRowIndex, endRowIndex, startColumnIndex, endColumnIndex);
        }
    }
}
