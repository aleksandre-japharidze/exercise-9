import pLimit from "p-limit";
import fetch from "node-fetch";

const urls: string[] = [
    "https://ipinfo.io/161.185.160.93/geo",
    "https://official-joke-api.appspot.com/random_joke",
    "https://randomuser.me/api/",
    "https://api.zippopotam.us/us/33162",
    "https://api.agify.io?name=meelad",
    "https://api.zippopotam.us/us/33162",
    "https://api.nationalize.io?name=nathaniel",
    "https://api.genderize.io?name=luc",
];

const limit = pLimit(4);

async function fetchApi(url: string, timeout: number = 800): Promise<void> {
    const controller = new AbortController();
    const timeoutId = setTimeout(() => controller.abort(), timeout);
    const signal = controller.signal;

    console.log("FETCHING: ", url);
    try {
        const startTime = performance.now();
        const response = await fetch(url, { signal });
        const endTime = performance.now();
        if (response.status !== 200) {
            console.log("FAILED: ", url, " -- TIME: ", endTime - startTime);
            return;
        }
        console.log("SUCCESS: ", url, "TIME:", endTime - startTime);
    } catch (error) {
        console.error("TIMEOUT: ", error, " -- TIME: ", timeout);
    } finally {
        clearTimeout(timeoutId);
        console.log("DONE: ", url);
    }
}

function limitedFetch(url: string): Promise<void> {
    return limit(() => fetchApi(url));
}

async function main(): Promise<void> {
    try {
        await Promise.all(urls.map(limitedFetch));
        console.log("ALL DONE");
    } catch (error) {
        console.error("ERROR: ", error);
    }
}

main();