import urllib.request
import re

headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)'}

# 1. Check alphacoders
urls = [
    'https://wall.alphacoders.com/by_sub_category.php?id=171960&name=Inception+Wallpapers',
    'https://wall.alphacoders.com/by_sub_category.php?id=171960&name=Inception+Wallpapers&page=2'
]

for url in urls:
    req = urllib.request.Request(url, headers=headers)
    try:
        with urllib.request.urlopen(req, timeout=10) as resp:
            html = resp.read().decode('utf-8', errors='ignore')
        imgs = re.findall(r'href="(https://images\d*\.alphacoders\.com/[^"]+)"', html)
        if not imgs:
            imgs = re.findall(r'src="(https://images\d*\.alphacoders\.com/[^"]+)"', html)
        print(f"AlphaCoders {url}: {len(imgs)} images found")
        for img in imgs[:5]:
            print(" ", img)
    except Exception as e:
        print("AlphaCoders error:", e)
