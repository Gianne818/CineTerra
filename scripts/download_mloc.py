import urllib.request
import os

headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36',
    'Referer': 'https://www.movie-locations.com/movies/i/Inception.php'
}

base_url = 'https://www.movie-locations.com/movies/i/'
files = [
    'Inception_Pont_Bir_Hakeim.jpg',
    'Inception-Pont-Bir-Hakeim-steps.jpg',
    'Inception-Heims-Bridge.jpg'
]

for f in files:
    url = base_url + f
    dest = f"temp_trailers/{f}"
    try:
        req = urllib.request.Request(url, headers=headers)
        with urllib.request.urlopen(req, timeout=10) as resp:
            data = resp.read()
            with open(dest, 'wb') as f:
                f.write(data)
            print(f"Downloaded {dest}: {len(data)} bytes")
    except Exception as e:
        print(f"Failed {f}: {e}")
