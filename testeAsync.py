import requests
import asyncio
from concurrent.futures import ThreadPoolExecutor
from timeit import default_timer

START_TIME = default_timer()

def fetch(session, dados):
    base_url = "https://marcapontoapi.azurewebsites.net/BatePonto/Marcar"
    with session.post(base_url + dados) as response:
        data = response.text
        print(data, dados)
        if response.status_code != 200:
            print("FAILURE::{0}".format(base_url + dados))

        elapsed = default_timer() - START_TIME
        time_completed_at = "{:5.2f}s".format(elapsed)
        print("SUCCESS::{0:<30} {1:>20}".format(dados, time_completed_at))

        return data

async def get_data_asynchronous():
    print("{0:<30} {1:>20}".format("File", "Completed at"))
    with ThreadPoolExecutor(max_workers=5) as executor:
        with requests.Session() as session:
            loop = asyncio.get_event_loop()
            START_TIME = default_timer()
            tasks = [
                loop.run_in_executor(
                    executor,
                    fetch,
                    *(session, "?IncludedAt=2021-01-01&EmployeeId="+ str(i) +"&EmployerId="+ str(i))
                )
                for i in range(1, 51)
            ]
            
            for response in await asyncio.gather(*tasks):
                pass

def main():
    loop = asyncio.get_event_loop()
    future = asyncio.ensure_future(get_data_asynchronous())
    loop.run_until_complete(future)

main()