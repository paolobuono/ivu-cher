using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Explore.Data.Resources;
using AvengersUtd.Explore.Environment.Controls.Elements;

namespace AvengersUtd.Explore.Environment.Controls
{
    public struct Link : IEquatable<Link>
    {

        public Element SourceElement{ get; private set; }
        public Element TargetElement { get; private set; }

        public string From
        {
            get
            {
                return SourcePosition + SourceElement.ElementCode;
            }
        }

        public string To
        {
            get { return TargetPosition + TargetElement.ElementCode; }
        }

        public ThumbPosition SourcePosition { get; private set; }
        public ThumbPosition TargetPosition { get; private set; }

        public Link(Element from, ThumbPosition sourcePosition = ThumbPosition.NotSet, Element to = null, ThumbPosition targetPosition = ThumbPosition.NotSet, bool isOutgoing=true) : this()
        {
            SourceElement = from;
            TargetElement = to;
            SourcePosition = sourcePosition;
            TargetPosition = targetPosition;
        }

        public override string ToString()
        {
            return string.Format("{0}To{1}", SourceElement.Caption.Replace(' ', '_'), TargetElement == null ? string.Empty : TargetElement.Caption.Replace(' ', '_'));
        }

        #region Equality
        public bool Equals(Link other)
        {
            return Equals(other.SourceElement, SourceElement)
                   && Equals(other.TargetElement, TargetElement)
                   && Equals(other.SourcePosition, SourcePosition)
                   && Equals(other.TargetPosition, TargetPosition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Link)) return false;
            return Equals((Link)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (SourceElement != null ? SourceElement.GetHashCode() : 0);
                result = (result * 397) ^ (TargetElement != null ? TargetElement.GetHashCode() : 0);
                result = (result * 397) ^ SourcePosition.GetHashCode();
                result = (result * 397) ^ TargetPosition.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(Link left, Link right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Link left, Link right)
        {
            return !left.Equals(right);
        } 
        #endregion
    }

    public struct ConnectionType
    {
        public ThumbPosition SourcePosition { get; private set; }
        public Type SourceType { get; private set; }
        public ThumbPosition TargetPosition { get; private set; }
        public Type[] TargetTypes { get; private set; }
        public bool MultiConnector { get; private set; }


        public ConnectionType(ThumbPosition sourcePosition, Type sourceType, ThumbPosition targetPosition, Type[] targetTypes, bool multiConnector = false) : this()
        {
            SourcePosition = sourcePosition;
            SourceType = sourceType;
            TargetPosition = targetPosition;
            TargetTypes = targetTypes;
            MultiConnector = multiConnector;
        }
    }

    public class SearchEventArgs : EventArgs
    {
        public SearchEventArgs(IQueryable<AbstractResource> data)
        {
            Data = data;
        }

        public IQueryable<AbstractResource> Data { get; private set; }
    }
}
