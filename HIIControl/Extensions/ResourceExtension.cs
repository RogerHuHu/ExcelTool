using System;
using System.Collections;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

namespace HIIControl.Extensions
{
    public abstract class ResourceExtension : MarkupExtension
    {
        public string ResourceKey { get; set; }
        public string Name { get; set; }

        public ResourceExtension()
        { }

        public ResourceExtension(string resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _ProvideLocalValue(serviceProvider) ?? _ProviderApplicationValue();
        }

        private object _ProvideLocalValue(IServiceProvider serviceProvider)
        {
            var rootObjectProvider = (IRootObjectProvider)serviceProvider.GetService(typeof(IRootObjectProvider));
            if (rootObjectProvider == null) return null;
            var dictionary = rootObjectProvider.RootObject as IDictionary;
            if (dictionary == null) return null;
            return dictionary.Contains(ResourceKey) ? dictionary[ResourceKey] : null;
        }

        private object _ProviderApplicationValue()
        {
            var value = Application.Current.TryFindResource(Name);
            return value;
        }
    }
}