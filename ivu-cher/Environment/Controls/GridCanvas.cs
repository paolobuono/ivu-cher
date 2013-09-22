using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using AvengersUtd.Explore.Data.Elements;
using AvengersUtd.Explore.Environment.Controls.Elements;

namespace AvengersUtd.Explore.Environment.Controls
{
	public class GridCanvas : Canvas
	{
		public const double BezierYOffset = 92;
		private const int GridFrequency = 32;
		private readonly List<Line> lineList;
		private readonly List<Element> elementList;
		private readonly List<Link> linkList;

		public Element SelectedElement { get; internal set; }
		public bool IsDragging { get; internal set; }
		public bool DesignMode { get; set; }

		internal SortedDictionary<string, Path> Curves { get; private set; }
		internal List<Element> Elements { get { return elementList; } }

        public int elementId;

		public GridCanvas()
		{
            elementId = 0;
			lineList = new List<Line>();
			elementList = new List<Element>();
			linkList = new List<Link>();
			Curves = new SortedDictionary<string, Path>();
			Focusable = false;
			AllowDrop = true;
		}

		#region Drag code
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			if (!e.Data.GetDataPresent("Resource")) return;
			e.Effects = DragDropEffects.None;
			e.Handled = false;
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (!e.Data.GetDataPresent("Resource")) return;
			e.Effects = DragDropEffects.None;
			e.Handled = false;
		}

		protected override void OnDragLeave(DragEventArgs e)
		{
			base.OnDragLeave(e);
			if (e.Data.GetDataPresent("Resource"))
			{
				e.Effects = DragDropEffects.Copy;
				e.Handled = false;
			}
		}

		protected override void OnDrop(DragEventArgs e)
		{
			base.OnDrop(e);
			if (!e.Data.GetDataPresent("ElementType")) return;

			ElementType type = (ElementType)e.Data.GetData("ElementType");
			AddElement(type, MouseUtilities.CorrectGetPosition(this));
		} 
		#endregion

		public void AddElement(ElementType type, Point position)
		{

			Element newElement;
			switch (type)
			{
				case ElementType.Connector:
					newElement = new Connector();
					break;

				case ElementType.MuseumGuideApp:
					newElement= new MuseumGuideElement { Caption = "Museum Guide"};
					break;

				case ElementType.HistoryPuzzleApp:
					newElement = new HistoryPuzzleElement { Caption = "History-Puzzle" };
					break;

				case ElementType.ExcursionGameApp:
					newElement = new ExcursionGameElement { Caption = "Excursion-Game" };
					break;

				case ElementType.Prologue:
					newElement = new PrologueElement();
					break;

				case ElementType.Mission:
					newElement = new MissionElement();
					break;

				case ElementType.Goal:
					newElement = new GoalElement();
					break;

				case ElementType.CSound:
					newElement = new ContextSoundElement();
					break;

				case ElementType.OracleHint:
					newElement = new OracleElement();
					break;

				case ElementType.MissionList:
					newElement = new MissionListElement();
					break;

				case ElementType.QuestionsAnswers:
					newElement = new QuestionElement { Caption = "Q&A" };
					break;

				case ElementType.Theme:
					newElement = new ThemeElement() { Caption = "Theme" };
					break;

				case ElementType.Item:
					newElement = new PageElement() { Caption = "Item" };
					break;

				case ElementType.Map:
					newElement = new MapElement() { Caption = "Map" };
					break;

				case ElementType.UG_ContentItem:
					newElement = new AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements.UG_ContentItem() { Caption = "Monument" };
					break;
				case ElementType.UG_GoalElement:
					newElement = new AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements.UG_GoalElement() { Caption = "Goal Monument" };
					break;
				case ElementType.UG_CityElement:
					if (elementList.Count(x => x is AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements.UG_CityElement) > 0)
					{
                        MessageBox.Show("Can not insert more than one item for City element");
						return;
					}
					newElement = new AvengersUtd.Explore.Environment.Controls.Elements.UG_Elements.UG_CityElement() { Caption = "City element", Height=300 };
					break;
				default:
				case ElementType.UG_ResourceElement:
					newElement = new AvengersUtd.Explore.Environment.Controls.Elements.ResourceElement() { Caption="Resource element"  };
					newElement.BuildingBlockLabel = "UG";
					((AvengersUtd.Explore.Environment.Controls.Elements.ResourceElement)newElement).ResourceRules = new Dictionary<Data.Resources.ResourceType, ResourceRule>();
					((AvengersUtd.Explore.Environment.Controls.Elements.ResourceElement)newElement).ResourceRules.Add(Data.Resources.ResourceType.Image, new ResourceRule(Data.Resources.ResourceType.Image, 0, 10));
					((AvengersUtd.Explore.Environment.Controls.Elements.ResourceElement)newElement).ResourceRules.Add(Data.Resources.ResourceType.Audio, new ResourceRule(Data.Resources.ResourceType.Audio, 0, 10));
					((AvengersUtd.Explore.Environment.Controls.Elements.ResourceElement)newElement).ResourceRules.Add(Data.Resources.ResourceType.Text, new ResourceRule(Data.Resources.ResourceType.Text, 0, 10));
					((AvengersUtd.Explore.Environment.Controls.Elements.ResourceElement)newElement).ResourceRules.Add(Data.Resources.ResourceType.Video, new ResourceRule(Data.Resources.ResourceType.Video, 0, 10));


					break; 
				case ElementType.Puzzle:
					newElement = new PuzzleElement { Caption = "Puzzle" };
					break;      
			}

			SetLeft(newElement, position.X);
			SetTop(newElement, position.Y);
            
			AddElement(newElement);
		}

		public void AddElement(Element element)
		{
			elementList.Add(element);
			Children.Add(element);
			element.OwnerCanvas = this;
		}

		#region Links & Connections
		public IEnumerable<Link> GetIncomingLinks(Element target)
		{
			return (from l in linkList where l.TargetElement == target select l);
		}

		public IEnumerable<Link> GetOutgoingLinks(Element source)
		{
			return (from l in linkList where l.SourceElement == source select l);
		}

		public IEnumerable<Element> UnconnectedElements
		{
			get
			{
				return from e in elementList where !HasConnections(e) select e;
			}
		}

		bool IsElementConnectedTo(Element source, Element target)
		{
			return linkList.Any(l => l.SourceElement == source && l.TargetElement == target);
		}

		bool HasConnections(Element element)
		{
			return linkList.Any(l => l.SourceElement == element || l.TargetElement == element);
		}

		public Element[] GetElementsDirectlyConnectedTo(Element source)
		{
			return (from e in elementList
					where IsElementConnectedTo(source, e)
					select e).ToArray();
		}

		public Element[] GeIncomingConnectedElements(Element target)
		{
			return (from e in elementList
					where IsElementConnectedTo(e, target)
					select e).ToArray();
		}

		/// <summary>
		/// Restituisce gli elementi connessi all'elemento source tramite un elemento connector.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public Element[] GetElementsConnectedTo(Element source)
		{
			Element[] elements = GetElementsDirectlyConnectedTo(source);
			var connectors = from ce in elements.OfType<Connector>()
							 select ce;
			List<Element> elementList = new List<Element>();
			elementList.AddRange(elements);

			if (connectors.Count() > 0)
			{
				
				foreach (Connector connector in connectors)
				{
					elementList.AddRange(GetElementsConnectedTo(connector));
				}
				return elementList.ToArray();
			}
			else
				return elements;
		}

		public void HighlightAvailableConnectors(Element element, ThumbPosition sourcePosition)
		{
			if (!element.HasConnectionsAllowedFrom(sourcePosition))
				return;

			var connectors = from e in UnconnectedElements
							 let c = element.Connections[sourcePosition]
							 where c.TargetTypes.Contains(e.GetType())
							 select new { Element = e, ThumbPosition = c.TargetPosition };
			foreach (var connector in connectors)
			{
				if (connector.Element == element)
					continue;
				connector.Element.CircleAdorner.SetBackground(connector.ThumbPosition, (SolidColorBrush)FindResource("ConnectorAvailable"));
			}
		}

		public void UnhighlightConnectors(Element element, ThumbPosition sourcePosition)
		{
			if (!element.HasConnectionsAllowedFrom(sourcePosition))
				return;

			var connectors = (from e in UnconnectedElements
							  let c = element.Connections[sourcePosition]
							  where c.TargetTypes.Contains(e.GetType())
							  select new { Element = e, ThumbPosition = c.TargetPosition });
			foreach (var connector in connectors)
			{
				connector.Element.CircleAdorner.SetBackground(connector.ThumbPosition,
															  (SolidColorBrush)FindResource("ConnectorUnconnected"));
			}

		} 
		#endregion

		#region Collision handling
		public bool HandleCollisions(Point position, Element caller, ThumbPosition sourcePosition, out Element targetElement, out ThumbPosition targetPosition)
		{
			double distance;
			targetElement = ElementNearestToPoint(position, caller, out distance, out targetPosition);

			if (Math.Abs(distance)<= 20 && (caller.IsConnectionAllowedTo(targetPosition, targetElement)))
			{
				targetElement.CircleAdorner.SetBackground(targetPosition, (SolidColorBrush)FindResource("ConnectorAvailable"));
				LinkCurve(caller.GetLink(sourcePosition).From,
					caller, sourcePosition, targetElement, targetPosition);
				return true;
			}
			return false;
		}

		Element ElementNearestToPoint(Point position, Element caller, out double minDistance, out ThumbPosition nearestThumbPosition)
		{
			minDistance = double.PositiveInfinity;
			Element hitElement = null;
			nearestThumbPosition = ThumbPosition.NotSet;

			foreach (Element element in UnconnectedElements)
			{
				if(element == caller) continue;

				foreach (ThumbPosition thumbPosition in element.AvailableIncomingConnectors)
				{
					Point elementLocation = element.GetCanvasThumbPosition(thumbPosition);
					double distance = position.Distance(elementLocation);
					if(distance < minDistance)
					{
						minDistance = distance;
						hitElement = element;
						nearestThumbPosition = thumbPosition;
					}
				}
			}

			return hitElement;
		} 
	#endregion

		#region Curve methods
		public bool CurveExists(string id, out Path curve)
		{
			return Curves.TryGetValue(id, out curve);
		}

		public void UpdateCurveSource(string id, Element source, ThumbPosition thumbPosition)
		{
			var curves = Children.OfType<Path>();
			curves.Count().ToString();

			foreach (Path curve in Curves.Values.Where(curve => curve.Name.StartsWith(id)))
			{
				Point thumbLocation = source.GetCanvasThumbPosition(thumbPosition);
				PathGeometry curveGeometry = (PathGeometry)curve.Data;
				PathFigure curveFigure = curveGeometry.Figures[0];
				curveFigure.StartPoint = thumbLocation;
				BezierSegment bezierSegment = (BezierSegment)curveFigure.Segments[0];
				bezierSegment.Point1 = ComputeBezierSegmentPoint(thumbPosition, thumbLocation);
			}
		}

		public void UpdateCurveTarget(string id, Element target, ThumbPosition thumbPosition)
		{
			var curves = Children.OfType<Path>();
			curves.Count().ToString();

			foreach (Path curve in Curves.Values.Where(curve => curve.Name.StartsWith(id)))
			{
				Point thumbLocation = target.GetCanvasThumbPosition(thumbPosition);
				PathGeometry curveGeometry = (PathGeometry)curve.Data;
				PathFigure curveFigure = curveGeometry.Figures[0];
				BezierSegment bezierSegment = (BezierSegment)curveFigure.Segments[0];
				bezierSegment.Point2 = ComputeBezierSegmentPoint(thumbPosition, thumbLocation);
				bezierSegment.Point3 = thumbLocation;
			}
		}

		public void LinkCurve(string curveId, Element source, ThumbPosition sourcePosition, Element target, ThumbPosition targetPosition)
		{
			foreach (Path curve in Curves.Values.Where(curve => curve.Name.StartsWith(curveId)))
			{
				Point thumbLocation = target.GetCanvasThumbPosition(targetPosition);
				PathGeometry curveGeometry = (PathGeometry)curve.Data;
				PathFigure curveFigure = curveGeometry.Figures[0];
				BezierSegment bezierSegment = (BezierSegment)curveFigure.Segments[0];

				bezierSegment.Point2 = ComputeBezierSegmentPoint(targetPosition, thumbLocation);
				bezierSegment.Point3 = thumbLocation;
			}

		}

		public void EstablishLink(string curveId, Element source, ThumbPosition sourcePosition, Element target, ThumbPosition targetPosition)
		{
			Link sourceLink = new Link(source, sourcePosition, target, targetPosition, true);

			source.SetLink(sourcePosition, sourceLink);
			linkList.Add(sourceLink);

			Path curve = Curves.Values.First(c => c.Name == curveId + "_i");
			curve.Stroke = (Brush)FindResource("ConnectedLink");

			source.CircleAdorner.SetBackground(sourcePosition, Brushes.LightGreen);
			target.CircleAdorner.SetBackground(targetPosition, Brushes.LightGreen);

			OnLinkEstablished(new LinkArgs(source, target));
		}

		void RemoveCurve(string curvePrefix)
		{
			Children.Remove(Curves[curvePrefix + "_i"]);
			Children.Remove(Curves[curvePrefix + "_o"]);
			Curves.Remove(curvePrefix + "_i");
			Curves.Remove(curvePrefix + "_o");
		}

		public void RemoveLink(Link link)
		{
			link.SourceElement.CircleAdorner.SetBackground(link.SourcePosition, (SolidColorBrush)FindResource("ConnectorUnconnected"));
			if (link.TargetElement != null)
				link.TargetElement.CircleAdorner.SetBackground(link.TargetPosition, (SolidColorBrush)FindResource("ConnectorUnconnected"));
			linkList.Remove(link);
			link.SourceElement.RemoveLink(link.SourcePosition);
			OnLinkRemoved(new LinkArgs(link.SourceElement, link.TargetElement));
		}

		public void RemoveLinksToSource(Element source)
		{
			var links = GetOutgoingLinks(source).ToArray();
			foreach (Link l in links)
			{
				RemoveCurve(l.From);
				RemoveLink(l);
			}
		}

		public void RemoveLinksToTarget(Element target)
		{
			var links= GetIncomingLinks(target).ToArray();
			foreach (Link l in links)
			{
				RemoveCurve(l.From);
				RemoveLink(l);
			}
		}

		public void RemoveElement(Element element)
		{
			elementList.Remove(element);
			Children.Remove(element);
			OnElementRemoved(new ElementArgs(element));
		}
		#endregion

		#region Exposed events
		public event EventHandler<LinkArgs> LinkEstablished;
		public event EventHandler<LinkArgs> LinkRemoved;

		protected void OnLinkEstablished(LinkArgs e)
		{
			EventHandler<LinkArgs> handler = LinkEstablished;
			if (handler != null)
				handler(this, e);
		}

		protected void OnLinkRemoved(LinkArgs e)
		{
			EventHandler<LinkArgs> handler = LinkRemoved;
			if (handler != null)
				handler(this, e);
		}

		public event EventHandler<ElementArgs> ElementRemoved;

		protected void OnElementRemoved(ElementArgs e)
		{
			EventHandler<ElementArgs> handler = ElementRemoved;
			if (handler != null)
				handler(this, e);
		}

		protected internal event EventHandler<ElementCaptionArgs> ElementCaptionChanged;
		protected internal void OnElementCaptionChanged(ElementCaptionArgs e)
		{
			EventHandler<ElementCaptionArgs> handler = ElementCaptionChanged;
			if (handler != null)
				handler(this, e);
		}
		#endregion

		#region Grid rendering
		void DrawGrid()
		{
			int horizontalLines = 1 + (int)ActualHeight / GridFrequency;
			int verticalLines = 1 + (int)ActualWidth / GridFrequency;

			DoubleCollection dashes = new DoubleCollection { 2, 2 };

			for (int i = 0; i < horizontalLines; i++)
			{
				Line line = new Line
				{
					X1 = 0,
					Y1 = i * GridFrequency,
					X2 = ActualWidth,
					Y2 = i * GridFrequency,
					StrokeDashArray = dashes,
					StrokeDashCap = PenLineCap.Round,
					Stroke = Brushes.LightGray,
					IsHitTestVisible = false
				};
				SetZIndex(line,0);
				Children.Add(line);
				lineList.Add(line);
			}

			for (int i = 0; i < verticalLines; i++)
			{
				Line line = new Line
				{
					X1 = i * GridFrequency,
					Y1 = 0,
					X2 = i * GridFrequency,
					Y2 = ActualHeight,
					StrokeDashArray = dashes,
					StrokeDashCap = PenLineCap.Round,
					Stroke = Brushes.LightGray,
					IsHitTestVisible = false
				};

				SetZIndex(line,0);
				Children.Add(line);
				lineList.Add(line);
			}
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			foreach (Line line in lineList)
				Children.Remove(line);
			lineList.Clear();
			DrawGrid();
		}
		#endregion

		static Point ComputeBezierSegmentPoint(ThumbPosition position, Point thumbLocation)
		{
			switch (position)
			{
				default:
				case ThumbPosition.Top:
					return new Point(thumbLocation.X, thumbLocation.Y - BezierYOffset);

				case ThumbPosition.Bottom:
					return new Point(thumbLocation.X, thumbLocation.Y + BezierYOffset);

				case ThumbPosition.Right:
					return new Point(thumbLocation.X+BezierYOffset, thumbLocation.Y);

				case ThumbPosition.Left:
					return new Point(thumbLocation.X - BezierYOffset, thumbLocation.Y);
			}
		}
	}
}
