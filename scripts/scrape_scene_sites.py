import urllib.request
import re

headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36'
}

urls = [
    'https://moviescenemap.com/locations/pont-de-bir-hakeim/',
    'https://ftrc.blog/inception-bridge-aka-pont-de-bir-hakeim/',
    'https://www.setjett.com/movie-location/inception/'
]

for url in urls:
    req = urllib.request.Request(url, headers=headers)
    try:
        with urllib.request.urlopen(req, timeout=10) as resp:
            html = resp.read().decode('utf-8', errors='ignore')
        imgs = re.findall(r'<img[^>]+src=["\']([^"\']+)["\']', html)
        print(f"=== {url} ({len(imgs)} images) ===")
        for img in imgs:
            if any(k in img.lower() for k in ['inception', 'bir', 'hakeim', 'still', 'scene', 'mirror', 'bridge', 'wp-content']):
                print(" ", img)
    except Exception as e:
        print(f"Failed {url}: {e}")
