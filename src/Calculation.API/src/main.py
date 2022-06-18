from fastapi import FastAPI
import os
import pandas as pd

FilePath = os.getenv('FileShareBasePath')

app = FastAPI()

@app.get("/{filepath}")
async def load_file(filepath: str):
    path = os.path.join(FilePath, filepath)
    df = pd.read_csv(path, dtype=str)
    return {"numRows": len(df.index)}
