using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace LogViewer.Infrastructure
{
    public class CommandHelper
    {
        public static async Task RunCommandAsync(Expression<Func<bool>> updatingFlag, Func<Task> action, int delayTaskInMilliseconds ) 
        {
            if (updatingFlag.Compile().Invoke())
                return;

            var expression = (updatingFlag as LambdaExpression).Body as MemberExpression;

            var propertyInfo = (PropertyInfo)expression.Member;
            var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            propertyInfo.SetValue(target, true);

            await Task.Delay(delayTaskInMilliseconds);
            try
            {
                await action();
            }
            finally
            {
                propertyInfo.SetValue(target, false);
            }
        }

    }
}
