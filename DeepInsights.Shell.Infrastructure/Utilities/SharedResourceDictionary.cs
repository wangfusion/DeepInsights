using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    public class SharedResourceDictionary : ResourceDictionary
    {
        #region Private Fields

        private Uri _SourceUri;
        private static Dictionary<Uri, ResourceDictionary> _SharedDictionaries = new Dictionary<Uri, ResourceDictionary>();
        private static bool _IsInDesignerMode = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

        #endregion

        #region Properties

        public static Dictionary<Uri, ResourceDictionary> SharedDictionaries
        {
            get
            {
                return _SharedDictionaries;
            }
        }

        public new Uri Source
        {
            get
            {
                return _SourceUri;
            }
            set
            {
                _SourceUri = value;
                if (!_SharedDictionaries.ContainsKey(value) || _IsInDesignerMode)
                {
                    try
                    {
                        base.Source = value;
                    }
                    catch (Exception)
                    {
                        if (!_IsInDesignerMode)
                        {
                            throw;
                        }
                    }

                    if (!_IsInDesignerMode)
                    {
                        _SharedDictionaries.Add(value, this);
                    }
                }
                else
                {
                    MergedDictionaries.Add(_SharedDictionaries[value]);
                }
            }
        }

        #endregion
    }
}
