using System;
using System.ComponentModel;
using System.Reflection;

namespace NLib.AtpNetCore.Testing.Mvc.Controllers
{
    public static class ParameterDefaultValues
    {
        public static object[] GetParameterDefaultValues(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            var parameters = methodInfo.GetParameters();
            var values = new object[parameters.Length];


            for (var i = 0; i < parameters.Length; i++)
            {
                var parameterInfo = parameters[i];
                object defaultValue;

                if (parameterInfo.HasDefaultValue)
                {
                    defaultValue = parameterInfo.DefaultValue;
                }
                else
                {
                    var defaultValueAttribute = parameterInfo
                        .GetCustomAttribute<DefaultValueAttribute>(inherit: false);

                    if (defaultValueAttribute?.Value == null)
                    {
                        defaultValue = parameterInfo.ParameterType.GetTypeInfo().IsValueType
                            ? Activator.CreateInstance(parameterInfo.ParameterType)
                            : null;
                    }
                    else
                    {
                        defaultValue = defaultValueAttribute.Value;
                    }
                }

                values[i] = defaultValue;
            }

            return values;
        }
    }
}
