import asyncio
import aiohttp
import time

urls = [
    "https://ipinfo.io/161.185.160.93/geo",
    "https://official-joke-api.appspot.com/random_joke",
    "https://randomuser.me/api/",
    "https://api.zippopotam.us/us/33162",
    "https://api.agify.io?name=meelad",
    "https://api.zippopotam.us/us/33162",
    "https://api.nationalize.io?name=nathaniel",
    "https://api.genderize.io?name=luc"
]

semaphore = asyncio.Semaphore(4)

async def fetch_api(url, session, timeout=800):
    async with semaphore:
        print("FETCHING: " + url)
        start_time = time.time()
        try:
            fetch_timeout = aiohttp.ClientTimeout(total=timeout)
            async with session.get(url, timeout=fetch_timeout) as response:
                if response.status != 200:
                    print("FAILED: " + url + " -- TIME: " + str(time.time() - start_time))
                    return
                print("SUCCESS: " + url + " -- TIME: " + str(time.time() - start_time))
        except asyncio.TimeoutError:
            print("TIMEOUT: " + url + " -- TIME: " + str(time.time() - start_time))
        except Exception as e:
            print("ERROR: " + e)
        finally:
            print("DONE: " + url)

async def main():
    session = aiohttp.ClientSession()
    await asyncio.gather(*[fetch_api(url, session) for url in urls])
    await session.close()

asyncio.run(main())