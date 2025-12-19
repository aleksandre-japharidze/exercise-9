import pLimit from "p-limit";
const urls = [
    "https://ipinfo.io/161.185.160.93/geo",
    "https://official-joke-api.appspot.com/random_joke",
    "https://randomuser.me/api/",
    "https://api.zippopotam.us/us/33162",
    "https://api.agify.io?name=meelad",
    "https://api.zippopotam.us/us/33162",
    "https://api.nationalize.io?name=nathaniel",
    "https://api.genderize.io?name=luc",
];
const limit = pLimit(5);
async function fetchApi(url) {
    console.log("FETCHING", url);
    const response = await fetch(url);
    if (response.ok) {
        console.log("SUCCESS", url);
    }
    else {
        console.log("FAIL", url);
    }
    console.log("DONE", url);
    return response;
}
function limitedFetch(url) {
    return limit(() => fetchApi(url));
}
async function main() {
    try {
        await Promise.all(urls.map(limitedFetch));
        console.log("ALL DONE");
    }
    catch (error) {
        console.error("ERROR", error);
    }
}
main();
//# sourceMappingURL=program.js.map