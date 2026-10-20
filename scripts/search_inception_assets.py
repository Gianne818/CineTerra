import urllib.request
import urllib.parse
import json
import re

HEADERS = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36'
}

def search_yahoo(query, limit=10):
    url = f"https://images.search.yahoo.com/search/images?p={urllib.parse.quote(query)}"
    req = urllib.request.Request(url, headers=HEADERS)
    try:
        with urllib.request.urlopen(req, timeout=15) as resp:
            html = resp.read().decode('utf-8', errors='ignore')
        matches = re.findall(r'"imgurl":"(https:[^"\\]+)"', html)
        if not matches:
            matches = re.findall(r'&imgurl=([^&"]+)', html)
            matches = [urllib.parse.unquote(m) for m in matches]
        return matches[:limit]
    except Exception as e:
        print("Yahoo error:", e)
        return []

queries = [
    "Inception 2010 Pont de Bir-Hakeim Leonardo DiCaprio Elliot Page",
    "Inception mirror scene bridge Paris still",
    "Inception 2010 movie still bir hakeim",
    "Inception Cobb Ariadne bridge mirror",
    "site:fandom.com inception pont de bir hakeim"
]

for q in queries:
    print(f"=== Yahoo Search: {q} ===")
    results = search_yahoo(q, 5)
    for r in results:
        print(" ", r)
