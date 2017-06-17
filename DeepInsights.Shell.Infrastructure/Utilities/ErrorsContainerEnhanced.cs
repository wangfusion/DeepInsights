using DeepInsights.Shell.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DeepInsights.Shell.Infrastructure
{
    public class ErrorsContainer<T>
    {
        private readonly T[] noErrors = new T[0];
        private readonly Action<string> raiseErrorsChanged;
        private readonly Dictionary<string, List<T>> validationResults;

        public ErrorsContainer(Action<string> raiseErrorsChanged)
        {
            raiseErrorsChanged.ThrowIfNull("raiseErrorsChanged");

            this.raiseErrorsChanged = raiseErrorsChanged;
            validationResults = new Dictionary<string, List<T>>();
        }

        public bool HasErrors
        {
            get
            {
                return validationResults.Count != 0;
            }
        }

        public Dictionary<string, List<T>> GetAllErrors()
        {
            return validationResults;
        }

        public IEnumerable<T> GetErrors(string propertyName)
        {
            var localPropertyName = propertyName ?? string.Empty;
            List<T> currentValidationResults = null;
            if (validationResults.TryGetValue(localPropertyName, out currentValidationResults))
            {
                return currentValidationResults;
            }
            else
            {
                return noErrors;
            }
        }

        public IEnumerable<T> GetErrors<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            var propertyName = Microsoft.Practices.Prism.Mvvm.PropertySupport.ExtractPropertyName(propertyExpression);
            return GetErrors(propertyName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void ClearErrors<TProperty>(Expression<Func<TProperty>> propertyExpression)
        {
            var propertyName = Microsoft.Practices.Prism.Mvvm.PropertySupport.ExtractPropertyName(propertyExpression);
            ClearErrors(propertyName);
        }

        public void ClearErrors(string propertyName)
        {
            SetErrors(propertyName, new List<T>());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void SetErrors<TProperty>(Expression<Func<TProperty>> propertyExpression, IEnumerable<T> propertyErrors)
        {
            var propertyName = Microsoft.Practices.Prism.Mvvm.PropertySupport.ExtractPropertyName(propertyExpression);
            SetErrors(propertyName, propertyErrors);
        }

        public void SetErrors(string propertyName, IEnumerable<T> newValidationResults)
        {
            var localPropertyName = propertyName ?? string.Empty;
            var hasCurrentValidationResults = validationResults.ContainsKey(localPropertyName);
            var hasNewValidationResults = newValidationResults != null && newValidationResults.Count() > 0;

            if (hasCurrentValidationResults || hasNewValidationResults)
            {
                if (hasNewValidationResults)
                {
                    validationResults[localPropertyName] = new List<T>(newValidationResults);
                    raiseErrorsChanged(localPropertyName);
                }
                else
                {
                    validationResults.Remove(localPropertyName);
                    raiseErrorsChanged(localPropertyName);
                }
            }
        }
    }
}
