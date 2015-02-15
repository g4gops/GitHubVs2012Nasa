using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nasa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtResults.Clear();
            try
            {
                string input = string.Empty;
                input = txtInput.Text;

                string[] splitStrs = input.Split(' ');
                int gridSize = Int32.Parse(splitStrs[1]);
                var arr = new int[gridSize, gridSize];


                string stemp = String.Join(",", splitStrs, 2, splitStrs.Length - 2);
                string[] splitStrsActual = stemp.Split(',');
                int index = 0;

                Dictionary<string, int> dictResults = new Dictionary<string, int>();

                // convert 1d - 2d
                for (int row = 0; row < gridSize; row++)
                {
                    for (int column = 0; column < gridSize; column++)
                    {
                        arr[row, column] = int.Parse(splitStrsActual[index++]);
                        Console.WriteLine(string.Format("{0},{1} = {2}", row, column, arr[row, column]));
                    }
                }


                // get the Scores and co-Ordinates
                for (int row = 0; row < gridSize; row++)
                {
                    for (int column = 0; column < gridSize; column++)
                    {
                        var neighbMeasures = GetNeighbourMeasurements(arr, row, column);
                        int total = neighbMeasures.Sum();
                        dictResults.Add("(" + column.ToString() + ',' + row.ToString() + " Score:" + total.ToString() + ')', total);
                    }
                }


                // Sort and Display only the top few.
                int resultsCount = Int32.Parse(splitStrs[0]);
                int dictIndex = 0;

                foreach (KeyValuePair<string, int> item in dictResults.OrderByDescending(key => key.Value))
                {
                    Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                    txtResults.AppendText(item.Key);
                    ++dictIndex;
                    if (dictIndex == resultsCount)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); ;
            }


        }

        /// <summary>
        /// Method to get the nearest numbers in an 2D array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public IEnumerable<T> GetNeighbourMeasurements<T>(T[,] arr, int row, int column)
        {
            int rows = arr.GetLength(0); // number of rows
            int columns = arr.GetLength(1); // number of columns


            // for each row, for cach column, return the current array of Row and Column

            for (int rCtr = row - 1; rCtr <= row + 1; rCtr++)
                for (int cCtr = column - 1; cCtr <= column + 1; cCtr++)
                    if (cCtr >= 0 && rCtr >= 0 && cCtr < columns && rCtr < rows)
                        yield return arr[rCtr, cCtr];
        }

        /// <summary>
        /// Basic Validation for the Input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtInput_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid input");
            }
        }

        /// <summary>
        /// Sets the Text box to be focussed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtInput;
            txtInput.Focus();
        }
    }
}
