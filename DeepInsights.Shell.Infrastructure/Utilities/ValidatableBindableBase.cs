using DeepInsights.Shell.Infrastructure.Utilities;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DeepInsights.Shell.Infrastructure
{
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        #region Private Fields

        Lazy<ErrorsContainer<string>> _ErrorsContainer;

        #endregion

        public ValidatableBindableBase()
        {
            _ErrorsContainer = new Lazy<ErrorsContainer<string>>(() => new ErrorsContainer<string>(pn => RaiseErrorsChanged(pn)), true);
        }

        #region Public Fields

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        #endregion

        #region Properties

        protected ErrorsContainer<string> ErrorsContainer
        {
            get
            {
                return _ErrorsContainer.Value;
            }
        }

        #endregion

        #region Public Methods

        public bool ValidateProperty(string propertyName)
        {
            propertyName.ThrowIfNull("propertyName");

            var propertyInfo = GetType().GetRuntimeProperty(propertyName);
            if (propertyInfo == null) throw new ArgumentException("Invalid property name", propertyName);

            var propertyErrors = new List<string>();
            bool isValid = TryValidateProperty(propertyInfo, propertyErrors);
            ErrorsContainer.SetErrors(propertyInfo.Name, propertyErrors);

            return isValid;
        }

        public bool ValidateProperties()
        {
            var propertiesWithChangedErrors = new List<string>();

            // Get all properties annotated for validation using attributes
            var propertiesToValidate = GetType().GetRuntimeProperties()
                                                .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any());

            foreach (PropertyInfo propertyInfo in propertiesToValidate)
            {
                var propertyErrors = new List<string>();
                TryValidateProperty(propertyInfo, propertyErrors);

                ErrorsContainer.SetErrors(propertyInfo.Name, propertyErrors);
            }

            return ErrorsContainer.HasErrors;
        }
        public bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyInfo.Name };
            var propertyValue = propertyInfo.GetValue(this);

            // Validate the property
            bool isValid = Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                propertyErrors.AddRange(results.Select(c => c.ErrorMessage));
            }

            return isValid;
        }

        #endregion

        #region Private/Protected Methods

        private void RaiseErrorsChanged(string propertyName)
        {
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged(this, e);
        }

        #endregion

        #region BindableBase overrides

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, propertyName);

            if (result && !string.IsNullOrEmpty(propertyName))
            {
                ValidateProperty(propertyName);
            }

            return result;
        }

        #endregion

        #region INotifyDataErrorInfo Implementation

        public IEnumerable GetErrors(string propertyName)
        {
            return ErrorsContainer.GetErrors(propertyName);
        }

        public void SetErrors<TProperty>(Expression<Func<TProperty>> propertyExpression, IEnumerable<string> errors)
        {
            ErrorsContainer.SetErrors(propertyExpression, errors);
        }

        public bool HasErrors
        {
            get
            {
                return ErrorsContainer.HasErrors;
            }
        }

        #endregion
    }
}
