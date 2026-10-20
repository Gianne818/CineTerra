import urllib.request
import re

headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36'}

def check_filmgrab():
    try:
        url = 'https://film-grab.com/?s=inception'
        req = urllib.request.Request(url, headers=headers)
        with urllib.request.urlopen(req, timeout=10) as resp:
            html = resp.read().decode('utf-8', errors='ignore')
        links = re.findall(r'href="(https://film-grab\.com/\d{4}/\d{2}/\d{2}/[^"/]+/)"', html)
        print("FilmGrab links:", set(links))
        for post in set(links):
            req2 = urllib.request.Request(post, headers=headers)
            with urllib.request.urlopen(req2, timeout=10) as resp2:
                html2 = resp2.read().decode('utf-8', errors='ignore')
            imgs = re.findall(r'src="(https://[^"]+\.jpg)"', html2)
            print(f"Found {len(imgs)} stills on {post}")
            for img in imgs:
                if any(w in img.lower() for w in ['bridge', 'mirror', 'paris', 'inception', 'wp-content/uploads']):
                    print("  ", img)
    except Exception as e:
        print("FilmGrab error:", e)

check_filmgrab()
