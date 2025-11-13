using System.Reflection;
using System.Windows.Input;

namespace DI;

public static class ObjectExtensions
{
    public static void InjectDependencies(this object obj, ICustomServiceProvider serviceProvider)
    {
        var type = obj.GetType();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .Where(x => x.GetCustomAttribute<DIAttribute>() != null)
                    .Where(f => !f.IsInitOnly);

        foreach (var field in fields)
        {
            var value = serviceProvider.Resolve(field.FieldType);
            if (value != null)
                field.SetValue(obj, value);
        }

        var props = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(p => p.CanWrite && p.GetCustomAttribute<DIAttribute>() != null);

        foreach (var prop in props)
        {
            var value = serviceProvider.Resolve(prop.PropertyType);
            if (value != null)
                prop.SetValue(obj, value);
        }
    }
}
