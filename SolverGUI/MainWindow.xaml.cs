using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolverGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        private String puzzle;
        private Grid puzzleGrid;
        private TextBlock[,] textBlocks;

        public MainWindow()
        {
            InitializeComponent();
            createGrid();
            loadPuzzle("004300209005009001070060043006002087190007400050083000600000105003508690042910300");
        }

        public void createGrid()
        {
            // Set grid 
            // Create the Grid
            puzzleGrid = new Grid();
            puzzleGrid.ShowGridLines = true;          

            // Add the Grid as the Content of the Parent Window Object
            Content = puzzleGrid;
        }

        public void loadPuzzle(string puz)
        {
            this.puzzle = puz;
            int gridSize = (int)Math.Sqrt(puzzle.Length);

            // Define the Columns
            ColumnDefinition[] colDefs = new ColumnDefinition[gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                colDefs[i] = new ColumnDefinition();
                colDefs[i].Width = new GridLength(1.0 / gridSize, GridUnitType.Star);
                puzzleGrid.ColumnDefinitions.Add(colDefs[i]);
            }

            // Define the Rows
            RowDefinition[] rowDefs = new RowDefinition[gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                rowDefs[i] = new RowDefinition();
                colDefs[i].Width = new GridLength(1.0 / gridSize, GridUnitType.Star);
                puzzleGrid.RowDefinitions.Add(rowDefs[i]);
            }

            // Add textblocks to the grid
            textBlocks = new TextBlock[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    textBlocks[i, j] = new TextBlock();
                    Grid.SetRow(textBlocks[i, j], i);
                    Grid.SetColumn(textBlocks[i, j], j);
                    puzzleGrid.Children.Add(textBlocks[i, j]);
                }
            }

            // Add content to the textblocks
            CharEnumerator chEnum = puzzle.GetEnumerator();
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (chEnum.MoveNext() == true)
                    {
                        if (chEnum.Current.Equals('0'))
                        {
                            textBlocks[i, j].Text = "";
                        }
                        else
                        {
                            textBlocks[i, j].Text = chEnum.Current.ToString();
                        }
                    }
                }
            }

            // show grid
            this.Show();
        }
    }
}
