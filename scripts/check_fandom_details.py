import urllib.request
import urllib.parse
import json

headers = {'User-Agent': 'Mozilla/5.0'}

targets = [
    'Paris_Streets_Infobox.png',
    'Parisian_cafe_dream.jpg',
    'Paradox.jpg',
    'Ariadne.jpg',
    'Ariadne.png',
    'Ariadne_questioning_Cobb.png',
    'Curling_city.png',
    'Architecture_city.png',
    'The_Architect_HD_Poster.png'
] + [f'Trailer{i:02d}.jpg' for i in range(1, 39)]

titles_param = "|".join([f"File:{t}" for t in targets])
url = f"https://inception.fandom.com/api.php?action=query&titles={urllib.parse.quote(titles_param)}&prop=imageinfo&iiprop=url|size|mime|comment&format=json"

req = urllib.request.Request(url, headers=headers)
try:
    with urllib.request.urlopen(req) as resp:
        data = json.loads(resp.read().decode('utf-8'))
        pages = data.get('query', {}).get('pages', {})
        for k, v in pages.items():
            title = v.get('title')
            info = v.get('imageinfo', [{}])[0]
            print(f"{title}: {info.get('width')}x{info.get('height')} - {info.get('url')} - comment: {info.get('comment')}")
except Exception as e:
    print("Error:", e)
