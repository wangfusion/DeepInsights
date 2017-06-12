using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        #region Private Fields

        private bool _SuppressNotification;

        #endregion

        #region Constructors

        public RangeObservableCollection() : base() { }

        public RangeObservableCollection(IEnumerable<T> collection) : base(collection) { }

        public RangeObservableCollection(IList<T> list) : base(list) { }

        #endregion

        #region Public Methods

        public void AddRange(IEnumerable<T> list)
        {
            InternalAddRange(list, false);
        }

        public void ClearAndAddRange(IEnumerable<T> list)
        {
            InternalAddRange(list, true);
        }

        public void RemoveWhere(Func<T, bool> condition)
        {
            bool modified = false;
            _SuppressNotification = true;

            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (condition(Items[i]))
                {
                    modified = true;
                    RemoveAt(i);
                }
            }

            _SuppressNotification = false;

            if (modified)
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        #endregion

        #region ObservableCollection Overrides

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_SuppressNotification)
            {
                base.OnCollectionChanged(e);
            }
        }

        #endregion

        #region Private Methods

        private void InternalAddRange(IEnumerable<T> list, bool clearFirst)
        {
            list.ThrowIfNull("list");

            _SuppressNotification = true;

            if (clearFirst)
            {
                Clear();
            }

            foreach (T item in list)
            {
                Add(item);
            }

            _SuppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion
    }
}
