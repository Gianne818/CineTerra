import urllib.request
import urllib.parse
import json

HEADERS = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)'}

def search_fandom(wiki, query, limit=3):
    url = f"https://{wiki}.fandom.com/api.php?action=query&generator=search&gsrsearch={urllib.parse.quote(query)}&gsrnamespace=6&prop=imageinfo&iiprop=url&format=json"
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
        print(f"Error on {wiki}: {e}")
        return []

tests = [
    ("shinkai", "Suzume door"),
    ("lotr", "Hobbiton Gandalf"),
    ("starwars", "Ahch-To Luke"),
    ("gameofthrones", "King's Landing"),
    ("dune", "Arrakis ornithopter"),
]

for wiki, q in tests:
    res = search_fandom(wiki, q, 2)
    print(f"=== {wiki} : {q} ===")
    for title, url in res:
        print(" ", title, "->", url)
