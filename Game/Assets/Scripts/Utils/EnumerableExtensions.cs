using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Map;

namespace Utils
{
    public static class EnumerableExtensions
    {
        private static Func<Direction, Direction, Direction> _direction;
        
        public static Direction Combine(this IEnumerable<Direction> list)
        {
            if (_direction != null)
            {
                return list.Aggregate(_direction);
            }
            
            var underlyingType = Enum.GetUnderlyingType(typeof(Direction));
            var v1 = Expression.Parameter(typeof(Direction));
            var v2 = Expression.Parameter(typeof(Direction));

            var expr = Expression.Lambda<Func<Direction, Direction, Direction>>(
                Expression.Convert(
                    Expression.Or( // combine the flags with an OR
                        Expression.Convert(v1,
                            underlyingType), // convert the values to a bit maskable type (i.e. the underlying numeric type of the enum)
                        Expression.Convert(v2, underlyingType)
                    ),
                    typeof(Direction) // convert the result of the OR back into the enum type
                ),
                v1, // the first argument of the function
                v2 // the second argument of the function
            );

            _direction = expr.Compile();

            return list.Aggregate(_direction);
        }
    }
}