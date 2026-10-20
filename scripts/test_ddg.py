import urllib.request
import urllib.parse
import json
import re

HEADERS = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36'
}

def search_ddg_images(query, max_results=5):
    url = f"https://duckduckgo.com/?q={urllib.parse.quote(query)}"
    req = urllib.request.Request(url, headers=HEADERS)
    with urllib.request.urlopen(req) as resp:
        content = resp.read().decode('utf-8', errors='ignore')
    
    m = re.search(r'vqd=([\d-]+)', content) or re.search(r'vqd="([^"]+)"', content)
    if not m:
        print("Could not find vqd token, len content:", len(content))
        return []
    vqd = m.group(1)
    
    api_url = f"https://duckduckgo.com/i.js?l=us-en&o=json&q={urllib.parse.quote(query)}&vqd={vqd}&f=,,,&p=1"
    req = urllib.request.Request(api_url, headers=HEADERS)
    with urllib.request.urlopen(req) as resp:
        data = json.loads(resp.read().decode('utf-8'))
        return data.get('results', [])[:max_results]

results = search_ddg_images("Suzume Bungo Mori Roundhouse")
print("Found", len(results), "results")
for r in results:
    print(r.get('title'), "->", r.get('image'))
