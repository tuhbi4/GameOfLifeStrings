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
            var columnsCount = matrix[0].Length;

            var outputStringBuilder = new StringBuilder();

            for (var rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                {
                    var targetCell = matrix[rowIndex][columnIndex];
                    var isAlive = targetCell == '1';

                    var liveNeighbors = GetLiveNeighbors(matrix, rowIndex, columnIndex);

                    outputStringBuilder.Append(isAlive switch
                    {
                        true when liveNeighbors is 2 or 3 => "1",
                        true => "0",
                        false when liveNeighbors is 3 => "1",
                        _ => targetCell
                    });
                }

                if (rowIndex != rowsCount - 1)
                {
                    outputStringBuilder.Append('_');
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
            if (Array.Exists(str.Distinct().ToArray(), c => c != '0' && c != '1' && c != '_'))
            {
                throw new ArgumentException("The input string must contain only '1' or '0' or '_' symbols.", nameof(str));
            }
        }

        private static char[][] GetValidMatrix(string str)
        {
            var separatedRows = str.Split('_');

            if (!Array.TrueForAll(separatedRows, row => row.Length == separatedRows[0].Length))
            {
                throw new ArgumentException("The input string must contain substrings of equal length.", nameof(str));
            }

            return separatedRows.Select(s => s.ToCharArray()).ToArray();
        }

        private static int GetLiveNeighbors(char[][] matrix, int targetRowIndex, int targetColumnIndex)
        {
            var rowsCount = matrix.Length;
            var columnsCount = matrix[0].Length;

            var liveNeighbors = 0;

            var (startRowIndex, endRowIndex, startColumnIndex, endColumnIndex) = GetMinMaxIndexes(rowsCount, columnsCount, targetRowIndex, targetColumnIndex);

            for (var rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
            {
                for (var columnIndex = startColumnIndex; columnIndex <= endColumnIndex; columnIndex++)
                {
                    if ((rowIndex != targetRowIndex || columnIndex != targetColumnIndex)
                        && matrix[rowIndex][columnIndex] == '1')
                    {
                        liveNeighbors++;
                    }
                }
            }

            return liveNeighbors;
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
