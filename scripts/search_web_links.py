import urllib.request
import urllib.parse
import re

headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36'}

def search_ddg_html(query):
    url = 'https://html.duckduckgo.com/html/?q=' + urllib.parse.quote(query)
    req = urllib.request.Request(url, headers=headers)
    try:
        with urllib.request.urlopen(req, timeout=10) as resp:
            html = resp.read().decode('utf-8', errors='ignore')
        links = re.findall(r'href="(//duckduckgo\.com/l/\?[^"]+)"', html)
        res = []
        for l in links:
            m = re.search(r'uddg=([^&]+)', l)
            if m:
                res.append(urllib.parse.unquote(m.group(1)))
        return res
    except Exception as e:
        print("Error:", e)
        return []

links = search_ddg_html("Inception pont de bir hakeim movie still")
print(f"Found {len(links)} links:")
for l in links[:10]:
    print(" ", l)
