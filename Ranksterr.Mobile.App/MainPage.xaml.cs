using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System.ComponentModel;
using System.Reflection;

namespace Ranksterr.Mobile.App;

public class CellPosition
{
  public int Row { get; set; }
  public int Column { get; set; }

  public CellPosition( int row, int column )
  {
    Row = row;
    Column = column;
  }
}
public class Cell : INotifyPropertyChanged
{
  private int _value;
  public int Value
  {
    get => _value;
    set
    {
      if( _value != value )
      {
        _value = value;
        OnPropertyChanged( nameof( Value ) );
        DisplayValue = value <= 0 ? string.Empty : value.ToString();
      }
    }
  }

  private string _displayValue;
  public string DisplayValue
  {
    get => _displayValue;
    set
    {
      if( _displayValue != value )
      {
        _displayValue = value;
        OnPropertyChanged( nameof( DisplayValue ) );
      }
    }
   }
  


  public bool IsEditable { get; set; }

  public Cell( int value, bool isEditable )
  {
    Value = value;
    IsEditable = isEditable;
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void OnPropertyChanged( string propertyName = null )
  {
    PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
  }
}


public class SudokuBoard
{
  public Cell[,] Board { get; private set; }

  public async Task SetCell(int row, int col, int val){
    //await Task.Run( async () =>
    //{
    MainThread.BeginInvokeOnMainThread( () =>
    {
      Board[row, col].Value = val;
      //await Task.Delay( 1 );
    } );
    await Task.Delay( 1 );
    // 
    //} );
  }

  public SudokuBoard(int gridNumber = 1)
  {
    Board = new Cell[9, 9];

    LoadFromResource( "sudoku.txt", gridNumber );
  }
  public Cell[,] GetCopy()
  {
    var board = new Cell[9, 9];

    for(var i = 0;i<9;i++)
    {
      for(var j = 0; j<9; j++)
      {
        board[i, j] = new Cell( Board[i, j].Value, Board[i, j].IsEditable );
      }
    }
    return board;
  }
  public (int, int) FindNextEmpty(Cell[,] board) {
    for( var r = 0; r < 9; r++ )
    {
      for( var c = 0; c < 9; c++ )
      {
        if( board[r, c].Value == 0 )
        {
          return (r, c);
        }
      }
    }
    return (-1, -1);
  }

  public bool IsValidBoard( Cell[,] board, int guess, int row, int col )
  {
    for(var i = 0; i<9; i++)
    {
      if( board[row, i].Value == guess || board[i, col].Value == guess )
      {
        return false;
      }
    }

    var rowStart = ( row / 3 ) * 3;
    var colStart = (col/3) * 3;
    for(var i = rowStart; i < rowStart + 3; i++)
    {
      for(var j = colStart; j < colStart + 3; j++)
      {
        if( board[i, j].Value == guess )
        {
          return false;
        }
      }
    }
    return true;

  }

  public async Task<bool> SolveBoard(Cell[,] board) {
    var rowCol = FindNextEmpty( board );
    var row = rowCol.Item1;
    var col = rowCol.Item2;
    if (rowCol.Item1 == -1)
    {
      return true;
    }

    for( var guess = 1; guess < 10; guess++ )
    {
      await SetCell(row, col, guess);
      //Task.Delay( 500 ).Wait();
      //Board[rowCol.Item1, rowCol.Item2].Value = guess;
      if( IsValidBoard( board, guess, rowCol.Item1, rowCol.Item2 ) )
      {
        board[rowCol.Item1, rowCol.Item2].Value = guess;
        //Task.Delay(200).Wait();
        if( await SolveBoard( board ) )
        {
          return true;
        }
        board[rowCol.Item1, rowCol.Item2].Value = 0;
        await SetCell( row, col, 0 );
      }
    }
    await SetCell( row, col, 0 );
    return false;
  }

  public bool LoadFromResource( string resourceName, int gridNumber )
  {
    string gridHeader = $"Grid {gridNumber:00}";
    Assembly assembly = Assembly.GetExecutingAssembly();

    // Use the fully qualified resource name, which includes the namespace
    string resourcePath = assembly.GetManifestResourceNames()
        .Single( str => str.EndsWith( resourceName ) );

    using( Stream stream = assembly.GetManifestResourceStream( resourcePath ) )
    using( StreamReader reader = new StreamReader( stream ) )
    {
      string contents = reader.ReadToEnd();
      var lines = contents.Split( new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries );

      // Find the start index of the desired grid
      int startIndex = Array.IndexOf( lines, gridHeader );
      if( startIndex == -1 )
      {
        Console.WriteLine( $"Grid {gridNumber:00} not found." );
        return false; // Grid not found
      }

      // Load the grid as before
      for( int i = startIndex + 1, row = 0; row < 9; i++, row++ )
      {
        string line = lines[i];
        for( int col = 0; col < 9; col++ )
        {
          int value = (int)Char.GetNumericValue( line[col] );
          Board[row, col] = new Cell( value, value == 0 );
        }
      }
    }

    return true; // Successfully loaded the grid
  }

  // Additional methods to manipulate the board could go here.
}



public partial class MainPage : ContentPage
{
  private SudokuBoard _board;

  public MainPage()
  {
    InitializeComponent();
    CreateSudokuBoard();
  }

  public void CreateSudokuBoard( int gridNumber = 15)
  {
    MainThread.BeginInvokeOnMainThread( () =>
    {
      // Remove the old SudokuGrid
      //MainLayout.Children.Remove( SudokuGrid );

      // Create a new Grid
      //SudokuGrid = new Grid()
      //{
      //  BackgroundColor = Colors.Black,
      //            RowSpacing = 1,
      //            ColumnSpacing = 1,
      //            HorizontalOptions = LayoutOptions.Center,
      //            VerticalOptions = LayoutOptions.Start,
      //            WidthRequest = 660,
      //            HeightRequest = 660,
      //};
      //SudokuFrame.AddLogicalChild( SudokuGrid );
      SudokuGrid.Children.Clear();
      SudokuGrid.RowDefinitions.Clear();
      SudokuGrid.ColumnDefinitions.Clear();
      _board = new SudokuBoard( gridNumber );
      PopulateGrid();
      //MainLayout.Children.Add( SudokuGrid );
    } );
  }

  private void PopulateGrid()
  {
    const int size = 9;
    for( int i = 0; i < size; i++ )
    {
      SudokuGrid.RowDefinitions.Add( new RowDefinition { Height = new GridLength( 1, GridUnitType.Star ) } );
      SudokuGrid.ColumnDefinitions.Add( new ColumnDefinition { Width = new GridLength( 1, GridUnitType.Star ) } );
    }

    for( int row = 0; row < size; row++ )
    {
      for( int col = 0; col < size; col++ )
      {
        var label = new Label
        {
          Text = _board.Board[row, col].Value.ToString(),
          HorizontalOptions = LayoutOptions.Center,
          VerticalOptions = LayoutOptions.Center,
          TextColor = _board.Board[row, col].Value >0 ? Colors.Black : Colors.Red,
          FontSize = 32,
          Margin = 5, // Adds padding around the label within the cell
          //BackgroundColor = Color.FromArgb( "#f0f0f0" ), // Light gray background, tweak as needed
                                                         // Add borders or differentiate sections as needed for visual clarity
        };

        // Determine if the cell is at the edge of a subgrid
        bool isRightEdgeOfSubgrid = ( col + 1 ) % 3 == 0 && col != size - 1;
        bool isBottomEdgeOfSubgrid = ( row + 1 ) % 3 == 0 && row != size - 1;

        var cellFrame = new Frame
        {
          Content = label,
          BorderColor = Colors.Black,
          Padding = 0,
          // Increase margin for cells at the right or bottom edge of a subgrid
          Margin = new Thickness( 1, 1, isRightEdgeOfSubgrid ? 4 : 1, isBottomEdgeOfSubgrid ? 4 : 1 ),
          CornerRadius = 0,
          BindingContext = new CellPosition( row, col )
        };
        var cell = _board.Board[row, col];
        label.BindingContext = cell; 
        label.SetBinding( Label.TextProperty, new Binding( "DisplayValue", BindingMode.OneWay ) );
        SudokuGrid.Children.Add( cellFrame );
        Grid.SetRow( cellFrame, row );
        Grid.SetColumn( cellFrame, col );

        // Bind label's text to Cell.Value for dynamic update if you plan to change values programmatically
        // This requires implementing INotifyPropertyChanged in your Cell class

        // Add gesture recognizer for tap if cells are interactive
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += ( s, e ) => {
          // Handle cell tap, e.g., open a number input
          Console.WriteLine( "I Tapped" );
          if( s is View senderView && senderView.BindingContext is CellPosition position )
          {
            Console.WriteLine( $"Cell tapped at Row: {position.Row}, Column: {position.Column}" );
            // Now you can use position.Row and position.Column as needed
          }
        };
        cellFrame.GestureRecognizers.Add( tapGestureRecognizer );

        //SudokuGrid.Children.Add( label ); // Add the child element to the grid first
        //Grid.SetRow( label, row ); // Set the row position
        //Grid.SetColumn( label, col ); // Set the column position
      }
    }
  }

  private async void OnSolvePuzzleClicked( object sender, EventArgs e )
  {
    //_board.SetCell(0, 0, 3).Wait();

    await _board.SolveBoard( _board.GetCopy() );
    //_board.SolveBoard( _board.GetCopy() );
  }
  private void OnResetPuzzleClicked( object sender, EventArgs e )
  {

    _board.Board[0,4].Value = 8;

    //CreateSudokuBoard( 2 );

  }
  private void OnNewPuzzleClicked( object sender, EventArgs e )
  {
    Random random = new Random();
    CreateSudokuBoard( random.Next(50) + 1 );

  }
}

