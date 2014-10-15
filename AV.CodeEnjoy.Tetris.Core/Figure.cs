using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace AV.CodeEnjoy.Tetris.Core
{
    public class Figure
    {
      /// <summary>
      /// Clockwise rotation count
      /// </summary>
      private int _rotate = 0;

      /// <summary>
      /// Count of left shifting
      /// </summary>
      private int _leftShift = 0;

      /// <summary>
      /// Count of right shifting
      /// </summary>
      private int _rightShift = 0;

      /// <summary>
      /// Have the figure was dropped
      /// </summary>
      private bool _isDroped = false;
 
      private readonly List<Point[]> _figure = new List<Point[]>();
      private Matrix _rotationMatrix = new Matrix(); 

      public Point Location { get; private set; }
      public FigureType Type { get; private set; }

      public Figure(FigureType type, Point location)
      {
        Location = location;
        InitFigure(type);
        _rotationMatrix.RotateAt(90, Location);
      }

      public string GetRecordedManipulations()
      {
        var commands = GetCommands();
        return commands.Aggregate("", (s, s1) => s + ", " + s1);
      }

      private IEnumerable<string> GetCommands()
      {
        var distance = _rightShift - _leftShift;
        if (distance < 0) yield return string.Format("left={0}", -distance);
        else if (0 < distance) yield return string.Format("right={0}", distance);
        if (_rotate != 0) yield return string.Format("rotate={0}", _rotate);
        if (_isDroped) yield return "drop";
      }

      public Point TopLeftPoint
      {
        get { return GetTopLeftPoint(); }
      }

      public Point BottomRightPoint 
      {
        get { return GetBottomRightPoint(); }
      }

      private void InitFigure(FigureType type)
      {
        switch (type)
        {
          case FigureType.Line:
            _figure.AddRange(new [] { 
              new [] { new Point(Location.X, Location.Y - 1), new Point(Location.X, Location.Y + 2) } });
            break;
          case FigureType.Square:
            _figure.AddRange(new []
            {
              new [] { new Point(Location.X, Location.Y), new Point(Location.X + 1, Location.Y) },
              new [] { new Point(Location.X, Location.Y + 1), new Point(Location.X + 1, Location.Y + 1) }
            });
            break;
          case FigureType.L:
            _figure.AddRange(new []
            {
              new [] { new Point(Location.X, Location.Y - 1), new Point(Location.X , Location.Y + 1) },
              new [] { new Point(Location.X, Location.Y + 1), new Point(Location.X + 1, Location.Y + 1) }
            });
            break;
          case FigureType.J:
            _figure.AddRange(new []
            {
              new [] { new Point(Location.X + 1, Location.Y - 1), new Point(Location.X + 1, Location.Y + 1) },
              new [] { new Point(Location.X, Location.Y + 1), new Point(Location.X + 1, Location.Y + 1) }
            });
            break;
          case FigureType.S:
            _figure.AddRange(new []
            {
              new [] { new Point(Location.X, Location.Y - 1), new Point(Location.X + 1, Location.Y - 1)},
              new [] { new Point(Location.X - 1, Location.Y), new Point(Location.X, Location.Y)}
            });
            break;
          case FigureType.Z:
            _figure.AddRange(new []
            {
              new [] { new Point(Location.X - 1, Location.Y - 1), new Point(Location.X, Location.Y - 1)},
              new [] { new Point(Location.X, Location.Y), new Point(Location.X + 1, Location.Y)}
            });
            break;
          case FigureType.T:
            _figure.AddRange(new []
            {
              new [] { new Point(Location.X, Location.Y - 1), new Point(Location.X, Location.Y) },
              new [] { new Point(Location.X - 1, Location.Y), new Point(Location.X + 1, Location.Y)}
            });
            break;
        }
      }

      private Point GetTopLeftPoint()
      {
        var xpoints = GetXcoordinates();
        var ypoints = GetYcoordiantes();
        return new Point(xpoints.Min(), ypoints.Min());
      }

      private Point GetBottomRightPoint()
      {
        var xpoints = GetXcoordinates();
        var ypoints = GetYcoordiantes();
        return new Point(xpoints.Max(), ypoints.Max());
      }

      private IEnumerable<int> GetYcoordiantes()
      {
        return GetPoints().Select( p => p.Y );
      }

      private IEnumerable<int> GetXcoordinates()
      {
        return GetPoints().Select(p => p.X);
      }

      public IEnumerable<Point> GetPoints()
      {
        return _figure.SelectMany(l => l);
      }

      /// <summary>
      /// Get Mask of the figure.
      /// </summary>
      /// <remarks>It's a glass of the figure size. Which contains true in offsets the the figure is located.</remarks>
      /// <returns></returns>
      public Mask GetFigureMask()
      {
        var fieldSize = new Size(BottomRightPoint) - new Size(TopLeftPoint);
        fieldSize = new Size(fieldSize.Width + 1, fieldSize.Height + 1);
        var figureMask = new Mask(TopLeftPoint, fieldSize);
        foreach (var line in _figure)
        {
          var points = line.OrderBy(p => p.X + p.Y).ToArray(); // will return only 2 points
          if (points[0].X == points[1].X)
          {
            var column = points[0].X - TopLeftPoint.X;
            var row = 0;
            for (int index = points[0].Y; index <= points[1].Y; index++)
            {
              figureMask[row, column] = true;
              row++;
            }
          }
          else
          {
            var row = points[0].Y - TopLeftPoint.Y;
            var column = 0;
            for (int index = points[0].X; index <= points[1].X; index++)
            {
              figureMask[row, column] = true;
              column++;
            }
          }
        }
        return figureMask;
      }

        /// <summary>
        /// Rotate figure clockwise 90 degrees N times
        /// </summary>
        public Figure Rotate(int nTimes)
        {
            nTimes %= 4;
            // rotate
            foreach (var line in _figure)
            {
                _rotationMatrix.TransformPoints(line);
            }

            // register rotation
            _rotate += nTimes;
            _rotate %= 4;
            return this;
        }

        /// <summary>
      /// Mark figure as dropped
      /// </summary>
      public void Drop()
      {
        _isDroped = true;
      }

      /// <summary>
      /// Shift figure using horizontal offset.
      /// </summary>
      /// <param name="offset"></param>
      /// <returns></returns>
      public Figure MoveHorizontaly(int offset)
      {
        if (offset < 0) _leftShift += Math.Abs(offset);
        else _rightShift += offset;

        foreach (var line in _figure)
        {
            _rotationMatrix.TransformPoints(line);
        }
        Location = new Point(Location.X + offset, Location.Y);
        return this;
      }
    }
}
