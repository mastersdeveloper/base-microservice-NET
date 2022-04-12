using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using System;
using System.Collections.Generic;

namespace BASE.MICRONET.Cross.Tracing.Dir
{
    public static class TracingExtension
    {
        public static IScope StartServerSpan(ITracer tracer, IDictionary<string, string> headers, string operationName)
        {
            ISpanBuilder spanBuilder;
            try
            {
                ISpanContext parentSpanCtx = tracer.Extract(BuiltinFormats.TextMap, new TextMapExtractAdapter(headers));

                spanBuilder = tracer.BuildSpan(operationName);
                if (parentSpanCtx != null)
                {
                    spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
                }
            }
            catch (Exception)
            {
                spanBuilder = tracer.BuildSpan(operationName);
            }

            return spanBuilder.WithTag(Tags.SpanKind, Tags.SpanKindConsumer).StartActive(true);
        }


        public static Dictionary<string, string> keys(ITracer tracer, IScope scope)
        {
            var span = scope.Span.SetTag(Tags.SpanKind, Tags.SpanKindClient);
            var dictionary = new Dictionary<string, string>();
            tracer.Inject(span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(dictionary));

            return dictionary;
        }

        public static Dictionary<string, string> keysScope(ITracer tracer, string tracerName)
        {
            using (var scope = tracer.BuildSpan(tracerName).StartActive(finishSpanOnDispose: true))
            {
                var span = scope.Span.SetTag(Tags.SpanKind, Tags.SpanKindClient);
                var dictionary = new Dictionary<string, string>();
                tracer.Inject(span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(dictionary));

                return dictionary;
            }
        }
    }
}
