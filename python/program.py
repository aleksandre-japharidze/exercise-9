import asyncio
import aiohttp
import time

async def fetch_api(session, url, semaphore, max_timeout=0.8):
    async with semaphore:
        print("FETCHING: " + url)
        start_time = time.time()
        try:
            timeout = aiohttp.ClientTimeout(total=max_timeout)
            async with session.get(url, timeout=timeout) as response:
                elapsed = time.time() - start_time
                if response.status != 200:
                    print("FAILED: " + url + " -- TIME: " + str(elapsed))
                    return
                else:
                    print("SUCCESS: " + url + " -- TIME: " + str(elapsed))
        except asyncio.TimeoutError:
            print("TIMEOUT: " + url + " -- TIME: " + str(elapsed))
        except Exception as e:
            print("ERROR: " + e)
        finally:
            print("DONE: " + url)

async def main():
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

    async with aiohttp.ClientSession() as session:
        semaphore = asyncio.Semaphore(4)
        tasks = [fetch_api(session, url, semaphore) for url in urls]
        await asyncio.gather(*tasks)
        
        print("ALL DONE")

if __name__ == "__main__":
    asyncio.run(main())