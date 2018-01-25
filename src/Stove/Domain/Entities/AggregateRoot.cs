using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stove.Domain.Entities
{
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
    }

    public class AggregateRoot<TPrimaryKey> : IAggregateRoot<TPrimaryKey>
    {
        private readonly EventRecorder _recorder;
        private readonly IConfigureInstanceEventRouter _router;

        public AggregateRoot()
        {
            _recorder = new EventRecorder();
            _router = new InstanceEventRouter();
        }

        public virtual TPrimaryKey Id { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Determines whether this instance has state changes.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance has state changes; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasChanges()
        {
            return _recorder.Any();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the state changes applied to this instance.
        /// </summary>
        /// <returns>A list of recorded state changes.</returns>
        public virtual IEnumerable<object> GetChanges()
        {
            return _recorder.ToArray();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Clears the state changes.
        /// </summary>
        public virtual void ClearChanges()
        {
            _recorder.Reset();
        }

        /// <summary>
        ///     Registers the state handler to be invoked when the specified event is applied.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to register the handler for.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="handler" /> is null.</exception>
        protected void Register<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _router.ConfigureRoute(handler);
        }

        /// <summary>
        ///     Applies the specified event to this instance and invokes the associated state handler.
        /// </summary>
        /// <param name="event">The event to apply.</param>
        protected void ApplyChange(object @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            BeforeApplyChange(@event);
            Play(@event);
            Record(@event);
            AfterApplyChange(@event);
        }

        private void Play(object @event)
        {
            _router.Route(@event);
        }

        private void Record(object @event)
        {
            _recorder.Record(@event);
        }

        /// <summary>
        ///     Called before an event is applied, exposed as a point of interception.
        /// </summary>
        /// <param name="event">The event that will be applied.</param>
        protected virtual void BeforeApplyChange(object @event)
        {
        }

        /// <summary>
        ///     Called after an event has been applied, exposed as a point of interception.
        /// </summary>
        /// <param name="event">The event that has been applied.</param>
        protected virtual void AfterApplyChange(object @event)
        {
        }

        #region overrides

        /// <inheritdoc />
        /// <summary>
        ///     Checks if this entity is transient (it has not an Id).
        /// </summary>
        /// <returns>True, if this entity is transient</returns>
        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(Id, default(TPrimaryKey)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(TPrimaryKey) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(TPrimaryKey) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Entity<TPrimaryKey>))
            {
                return false;
            }

            //Same instances must be considered as equal
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            //Transient objects are not considered as equal
            var other = (Entity<TPrimaryKey>)obj;
            if (IsTransient() && other.IsTransient())
            {
                return false;
            }

            //Must have a IS-A relation of types or must be same type
            Type typeOfThis = GetType();
            Type typeOfOther = other.GetType();
            if (!typeOfThis.GetTypeInfo().IsAssignableFrom(typeOfOther) && !typeOfOther.GetTypeInfo().IsAssignableFrom(typeOfThis))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <inheritdoc />
        public static bool operator ==(AggregateRoot<TPrimaryKey> left, AggregateRoot<TPrimaryKey> right)
        {
            if (Equals(left, null))
            {
                return Equals(right, null);
            }

            return left.Equals(right);
        }

        /// <inheritdoc />
        public static bool operator !=(AggregateRoot<TPrimaryKey> left, AggregateRoot<TPrimaryKey> right)
        {
            return !(left == right);
        }

        #endregion
    }
}
