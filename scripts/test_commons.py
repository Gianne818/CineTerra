import urllib.request
import urllib.parse
import json

HEADERS = {'User-Agent': 'CineTerraApp/1.0 (contact: info@cineterra.app)'}

def search_commons(query, limit=3):
    url = f"https://commons.wikimedia.org/w/api.php?action=query&generator=search&gsrsearch={urllib.parse.quote(query)}&gsrnamespace=6&prop=imageinfo&iiprop=url&format=json"
    req = urllib.request.Request(url, headers=HEADERS)
    try:
        with urllib.request.urlopen(req) as resp:
            data = json.loads(resp.read().decode('utf-8'))
            pages = data.get('query', {}).get('pages', {})
            results = []
            for k, v in list(pages.items())[:limit]:
                info = v.get('imageinfo', [{}])[0]
                results.append((v.get('title'), info.get('url')))
            return results
    except Exception as e:
        print("Error searching", query, ":", e)
        return []

locations = [
    "Bungo-Mori Roundhouse",
    "Hijiribashi Bridge Ochanomizu",
    "Wadi Rum Jordan",
    "Hobbiton Matamata",
    "Skellig Michael",
    "Dubrovnik city walls",
    "Pont de Bir-Hakeim",
    "Suga Shrine Yotsuya stairs"
]

for loc in locations:
    res = search_commons(loc, 2)
    print(f"=== {loc} ===")
    for title, url in res:
        print(" ", title, "->", url)
