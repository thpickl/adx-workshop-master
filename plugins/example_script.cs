range x from 1 to 360 step 1
| as T
| evaluate csharp(
    typeof(*, fx:double), //  Output schema: append a new fx column to original table 
    'IEnumerable<TOutput> Process(IEnumerable<TInput> input, ICSharpSandboxContext context)' //  The C# decorated script
    '{'
    '    var n = context.GetArgument("count", int.Parse);'
    '    var g = context.GetArgument("gain", int.Parse);'
    '    var f = context.GetArgument("cycles", int.Parse);'
    ''
    '    foreach (var row in input) '
    '    {'
    '       yield return new TOutput { x = row.x, fx = MyCalculator.Calculate(g, n, f, row.x) };'
    '    }'
    '}'
    ''
    'public static class MyCalculator'
    '{'
    '    public static double Calculate(int g, int n, int f, long? value)'
    '    {'
    '        return value.HasValue ? g * Math.Sin((double)value / n * 2 * Math.PI * f) : 0.0;'
    '    }'
    '}',
    pack('gain', 5, 'cycles', 8, 'count', toscalar(T | count)) // dictionary of parameters
)
| render linechart
