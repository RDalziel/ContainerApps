from fastapi import FastAPI, Response
import os
import pandas as pd
from opentelemetry.instrumentation.fastapi import FastAPIInstrumentor
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import (BatchSpanProcessor)
from azure.monitor.opentelemetry.exporter import AzureMonitorTraceExporter

FilePath = os.getenv('FileShareBasePath')
AppInsightsConnectionString = os.getenv('APPINSIGHTS_CONNECTION_STRING')

tracer = TracerProvider()

app = FastAPI()

@app.on_event("startup")
def startup_event():
    exporter = AzureMonitorTraceExporter.from_connection_string(AppInsightsConnectionString)
    tracer.add_span_processor(BatchSpanProcessor(exporter))
    FastAPIInstrumentor.instrument_app(app, tracer_provider=tracer)

@app.get("/{filepath}")
async def load_file(filepath: str, response: Response):
    path = os.path.join(FilePath, filepath)

    if not os.path.exists(path):
        response.status_code = 404
        return

    df = pd.read_csv(path, dtype=str)
    return {"numRows": len(df.index)}
