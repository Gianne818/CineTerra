import urllib.request
import urllib.parse
import re
import json

headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36',
    'Accept-Language': 'en-US,en;q=0.9',
}

def test_bing(query):
    url = f"https://www.bing.com/images/search?q={urllib.parse.quote(query)}"
    req = urllib.request.Request(url, headers=headers)
    try:
        with urllib.request.urlopen(req, timeout=10) as resp:
            html = resp.read().decode('utf-8', errors='ignore')
        # Bing encodes image details in m="{&quot;murl&quot;:&quot;...&quot;
        murls = re.findall(r'murl&quot;:&quot;(https?://[^&]+)&quot;', html)
        print(f"Bing found {len(murls)} images for '{query}'")
        for m in murls[:5]:
            print(" ", m)
    except Exception as e:
        print("Bing error:", e)

test_bing("Inception 2010 Pont de Bir Hakeim movie still")
test_bing("Inception mirror bridge Ariadne Cobb movie still")
