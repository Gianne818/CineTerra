import urllib.request
import urllib.parse
import json

headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)'}

url = "https://inception.fandom.com/api.php?action=query&list=allimages&ailimit=100&format=json"
req = urllib.request.Request(url, headers=headers)
try:
    with urllib.request.urlopen(req) as resp:
        data = json.loads(resp.read().decode('utf-8'))
        images = data.get('query', {}).get('allimages', [])
        print(f"Total images on inception fandom: {len(images)}")
        for img in images:
            name = img.get('name', '')
            if any(k in name.lower() for k in ['paris', 'bridge', 'ariadne', 'cobb', 'mirror', 'scene', 'trailer']):
                print(img.get('name'), '->', img.get('url'))
except Exception as e:
    print("Error:", e)
