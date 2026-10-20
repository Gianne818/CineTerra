import urllib.request
import os

headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36',
    'Referer': 'https://www.movieworldmap.com/filming-locations/bridge-with-mirrors/'
}

for i in range(1, 7):
    fn = f"Inception-Bridge-with-Mirrors-{i}-movieworldmap.com_.jpg"
    url = f"https://www.movieworldmap.com/content/images/{fn}"
    dest = f"temp_trailers/{fn}"
    try:
        req = urllib.request.Request(url, headers=headers)
        with urllib.request.urlopen(req, timeout=10) as resp:
            data = resp.read()
            with open(dest, 'wb') as f:
                f.write(data)
            print(f"Downloaded {dest}: {len(data)} bytes")
    except Exception as e:
        print(f"Failed {fn}: {e}")
