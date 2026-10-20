import urllib.request
import urllib.parse
import re
import json

HEADERS = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36'}

def search_yahoo_images(query, limit=3):
    url = f"https://images.search.yahoo.com/search/images?p={urllib.parse.quote(query)}"
    req = urllib.request.Request(url, headers=HEADERS)
    try:
        with urllib.request.urlopen(req) as resp:
            html = resp.read().decode('utf-8', errors='ignore')
        # Find imgurl in json or tags
        matches = re.findall(r'"imgurl":"(https:[^"]+)"', html)
        if not matches:
            matches = re.findall(r'&imgurl=([^&"]+)', html)
            matches = [urllib.parse.unquote(m) for m in matches]
        return matches[:limit]
    except Exception as e:
        print("Yahoo error:", e)
        return []

results = search_yahoo_images("Suzume Bungo Mori door still")
print("Yahoo results for Suzume:", results)
