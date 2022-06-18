from fastapi import FastAPI
import os
import pandas as pd
from opencensus.ext.azure.trace_exporter import AzureExporter
from opencensus.trace.samplers import ProbabilitySampler
from opencensus.trace.tracer import Tracer
from opencensus.trace.span import SpanKind
from opencensus.trace.attributes_helper import COMMON_ATTRIBUTES

FilePath = os.getenv('FileShareBasePath')
AppInsightsConnectionString = os.getenv('APPINSIGHTS_CONNECTION_STRING')

HTTP_URL = COMMON_ATTRIBUTES['HTTP_URL']
HTTP_STATUS_CODE = COMMON_ATTRIBUTES['HTTP_STATUS_CODE']

APPINSIGHTS_CONNECTION_STRING=AppInsightsConnectionString
exporter=AzureExporter(connection_string=f'{APPINSIGHTS_CONNECTION_STRING}')
sampler=ProbabilitySampler(1.0)

app = FastAPI()


@app.middleware("http")
async def middlewareOpencensus(request: Request, call_next):
    tracer = Tracer(exporter=exporter, sampler=sampler)
    with tracer.span("main") as span:
        span.span_kind = SpanKind.SERVER

        response = await call_next(request)

        tracer.add_attribute_to_current_span(
            attribute_key=HTTP_STATUS_CODE,
            attribute_value=response.status_code)
        tracer.add_attribute_to_current_span(
            attribute_key=HTTP_URL,
            attribute_value=str(request.url))

    return response

@app.get("/{filepath}")
async def load_file(filepath: str):
    path = os.path.join(FilePath, filepath)
    df = pd.read_csv(path, dtype=str)
    return {"numRows": len(df.index)}
